// <copyright file="LoLCoachContextBuilder.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Application.LeagueOfLegends.Coach.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using GameOn.Domain;

    /// <summary>
    /// Turns a stored game into the compact brief handed to the model.
    /// </summary>
    /// <remarks>
    /// This is the part that decides whether the coach is any good. A raw match-v5 timeline is 500 KB to 2 MB,
    /// i.e. hundreds of thousands of tokens, so the job here is not to dump data but to select the handful of
    /// facts a coach would actually look at, and to say nothing at all when a fact was not captured - a zero
    /// presented as a fact is what makes a model invent a weakness the player does not have.
    /// </remarks>
    public static class LoLCoachContextBuilder
    {
        /// <summary>How far a frame may sit from the requested minute and still be used, in milliseconds.</summary>
        private const int FrameToleranceMs = 45000;

        /// <summary>Half the width of Summoner's Rift, used to tell the blue half from the red half.</summary>
        private const int RiftMidpoint = 7435;

        /// <summary>Minutes at which the lane comparison is sampled.</summary>
        private static readonly int[] SampleMinutes = new[] { 10, 15, 20 };

        /// <summary>
        /// Builds the brief for one participant.
        /// </summary>
        /// <param name="game">The game, with its participants, their stats and challenges, and its teams.</param>
        /// <param name="target">The participant being coached.</param>
        /// <param name="frames">Timeline frames of the game, with their participant snapshots.</param>
        /// <param name="championKills">CHAMPION_KILL events of the game.</param>
        /// <returns>A plain-text brief, budgeted to stay in the low thousands of tokens.</returns>
        public static string Build(
            LoLGame game,
            LoLGameParticipant target,
            IReadOnlyCollection<LoLGameTimelineFrame> frames,
            IReadOnlyCollection<LoLGameTimelineEvent> championKills)
        {
            var builder = new StringBuilder();
            var durationSeconds = target.Stats?.GameDurationSeconds
                ?? (int)Math.Max(0, (game.GameEnd - game.GameStart).TotalSeconds);

            AppendGameSection(builder, game, target, durationSeconds);

            var opponent = FindLaneOpponent(game, target);

            AppendPlayerSection(builder, "TON JOUEUR", target);

            if (opponent is not null)
            {
                AppendPlayerSection(builder, "TON ADVERSAIRE DIRECT (même rôle, équipe adverse)", opponent);
                AppendLaneProgression(builder, frames, target, opponent);
            }

            AppendDeaths(builder, game, target, championKills, durationSeconds);
            AppendTeamObjectives(builder, game, target);
            AppendAdvancedStats(builder, target);
            AppendRosters(builder, game, target);

            return builder.ToString();
        }

        /// <summary>
        /// Appends the framing every other section is read against.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        /// <param name="game">The game.</param>
        /// <param name="target">The coached participant.</param>
        /// <param name="durationSeconds">Game duration, in seconds.</param>
        private static void AppendGameSection(StringBuilder builder, LoLGame game, LoLGameParticipant target, int durationSeconds)
        {
            builder.AppendLine("## PARTIE");
            builder.AppendLine($"Mode : {game.Queue?.Description ?? "inconnu"}");
            builder.AppendLine($"Durée : {durationSeconds / 60} min {durationSeconds % 60:00} s");
            builder.AppendLine($"Résultat : {(target.Win ? "Victoire" : "Défaite")}");

            if (game.IsRemake)
            {
                builder.AppendLine("Attention : partie marquée comme remake, les statistiques ne veulent pas dire grand-chose.");
            }

            builder.AppendLine();
        }

        /// <summary>
        /// Appends one participant's headline numbers, skipping every value that was not captured.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        /// <param name="title">Section title.</param>
        /// <param name="participant">Participant to describe.</param>
        private static void AppendPlayerSection(StringBuilder builder, string title, LoLGameParticipant participant)
        {
            builder.AppendLine($"## {title}");
            builder.AppendLine($"Champion : {participant.ChampionName}");

            if (!string.IsNullOrWhiteSpace(participant.TeamPosition))
            {
                builder.AppendLine($"Rôle : {participant.TeamPosition}");
            }

            builder.AppendLine($"KDA : {participant.Kills}/{participant.Deaths}/{participant.Assists}");

            var stats = participant.Stats;

            if (stats is not null)
            {
                builder.AppendLine($"Ratio KDA : {Number(stats.Kda)}");
                builder.AppendLine($"Participation aux kills : {Number(stats.KillParticipationPercent)} %");
                builder.AppendLine($"CS : {stats.CreepScore} ({Number(stats.CsPerMinute)}/min)");
                builder.AppendLine($"Or : {stats.GoldEarned} ({Number(stats.GoldPerMinute)}/min)");
                builder.AppendLine($"Dégâts aux champions : {stats.DamageDealtToChampions} ({Number(stats.DamagePerMinute)}/min)");
                builder.AppendLine($"Dégâts subis : {stats.DamageTaken}");

                // Wards come from timeline events, which the League client never provides on an imported
                // custom. Zero there means "not captured", so it is left out rather than shown as a weakness.
                if (stats.WardsPlaced > 0 || stats.WardsKilled > 0)
                {
                    builder.AppendLine($"Wards : {stats.WardsPlaced} posées, {stats.WardsKilled} détruites");
                }
            }

            if (participant.VisionScore > 0)
            {
                builder.AppendLine($"Score de vision : {participant.VisionScore}");
            }

            builder.AppendLine();
        }

        /// <summary>
        /// Appends the lane differential at the sampled minutes. A coach reads the shape of the curve, not the
        /// end-of-game totals: being 800 gold down at 10 minutes and even at 20 is a different game from the reverse.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        /// <param name="frames">Timeline frames.</param>
        /// <param name="target">The coached participant.</param>
        /// <param name="opponent">Its direct lane opponent.</param>
        private static void AppendLaneProgression(
            StringBuilder builder,
            IReadOnlyCollection<LoLGameTimelineFrame> frames,
            LoLGameParticipant target,
            LoLGameParticipant opponent)
        {
            if (frames.Count == 0)
            {
                return;
            }

            var lines = new List<string>();

            foreach (var minute in SampleMinutes)
            {
                var frame = NearestFrame(frames, minute * 60000);

                var mine = frame?.LoLGameTimelineFrameParticipants?.FirstOrDefault(x => x.ParticipantPUUID == target.Puuid);
                var theirs = frame?.LoLGameTimelineFrameParticipants?.FirstOrDefault(x => x.ParticipantPUUID == opponent.Puuid);

                if (mine is null || theirs is null)
                {
                    continue;
                }

                var goldDiff = mine.TotalGold - theirs.TotalGold;
                var csDiff = (mine.MinionsKilled + mine.JungleMinionsKilled) - (theirs.MinionsKilled + theirs.JungleMinionsKilled);
                var xpDiff = mine.Xp - theirs.Xp;

                lines.Add($"{minute} min : or {Signed(goldDiff)}, CS {Signed(csDiff)}, XP {Signed(xpDiff)}, niveau {mine.Level} contre {theirs.Level}");
            }

            if (lines.Count == 0)
            {
                return;
            }

            builder.AppendLine("## ÉVOLUTION FACE À TON ADVERSAIRE DIRECT (écarts, positif = à ton avantage)");
            lines.ForEach(x => builder.AppendLine(x));
            builder.AppendLine();
        }

        /// <summary>
        /// Appends the coached player's deaths, with when and where they happened.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        /// <param name="game">The game, used to resolve killers and the map.</param>
        /// <param name="target">The coached participant.</param>
        /// <param name="championKills">CHAMPION_KILL events of the game.</param>
        /// <param name="durationSeconds">Game duration, in seconds.</param>
        private static void AppendDeaths(
            StringBuilder builder,
            LoLGame game,
            LoLGameParticipant target,
            IReadOnlyCollection<LoLGameTimelineEvent> championKills,
            int durationSeconds)
        {
            var deaths = championKills
                .Where(x => x.VictimPUUID == target.Puuid)
                .OrderBy(x => x.Timestamp)
                .ToList();

            if (deaths.Count == 0)
            {
                return;
            }

            // Only meaningful on Summoner's Rift, and only when the queue carries its map: naming a lane on
            // ARAM, or guessing one, would hand the model a fact that is simply false.
            var onRift = game.Queue?.Map?.Contains("Summoner", StringComparison.OrdinalIgnoreCase) == true;

            builder.AppendLine($"## TES MORTS ({deaths.Count})");

            foreach (var death in deaths)
            {
                var killer = game.LeagueOfLegendsGameParticipants?.FirstOrDefault(x => x.Puuid == death.KillerPUUID);
                var line = $"{FormatTimestamp(death.Timestamp)} — tué par {killer?.ChampionName ?? "un ennemi"}";

                if (onRift && death.PositionX is int x && death.PositionY is int y)
                {
                    line += $" ({DescribeZone(x, y, target.TeamId)})";
                }

                builder.AppendLine(line);
            }

            if (durationSeconds > 0)
            {
                builder.AppendLine($"Soit {Number(deaths.Count / (durationSeconds / 60.0) * 10)} morts toutes les 10 minutes.");
            }

            builder.AppendLine();
        }

        /// <summary>
        /// Appends the objective counters of both teams.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        /// <param name="game">The game.</param>
        /// <param name="target">The coached participant.</param>
        private static void AppendTeamObjectives(StringBuilder builder, LoLGame game, LoLGameParticipant target)
        {
            var teams = game.LeagueOfLegendsGameTeams;

            if (teams is null || teams.Count == 0)
            {
                return;
            }

            var mine = teams.FirstOrDefault(x => x.TeamId == target.TeamId);
            var theirs = teams.FirstOrDefault(x => x.TeamId != target.TeamId);

            if (mine is null || theirs is null)
            {
                return;
            }

            builder.AppendLine("## OBJECTIFS (ton équipe contre l'équipe adverse)");
            builder.AppendLine($"Kills : {mine.ChampionKills} contre {theirs.ChampionKills}");
            builder.AppendLine($"Tours : {mine.TowerKills} contre {theirs.TowerKills}");
            builder.AppendLine($"Dragons : {mine.DragonKills} contre {theirs.DragonKills}");
            builder.AppendLine($"Nashors : {mine.BaronKills} contre {theirs.BaronKills}");
            builder.AppendLine($"Hérauts : {mine.RiftHeraldKills} contre {theirs.RiftHeraldKills}");
            builder.AppendLine($"Premier sang : {(mine.FirstBlood ? "ton équipe" : theirs.FirstBlood ? "l'équipe adverse" : "personne")}");
            builder.AppendLine($"Première tour : {(mine.FirstTower ? "ton équipe" : theirs.FirstTower ? "l'équipe adverse" : "personne")}");
            builder.AppendLine();
        }

        /// <summary>
        /// Appends the handful of Riot challenges a coach would actually comment on.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        /// <param name="target">The coached participant.</param>
        private static void AppendAdvancedStats(StringBuilder builder, LoLGameParticipant target)
        {
            var challenges = target.Challenges;

            if (challenges is null)
            {
                // Expected on games imported from the League client: the old match-v4 payload has no challenges.
                return;
            }

            var lines = new List<string>();

            AddIfPositive(lines, "Part des dégâts de l'équipe", challenges.TeamDamagePercentage * 100, " %");
            AddIfPositive(lines, "Part des dégâts subis par l'équipe", challenges.DamageTakenOnTeamPercentage * 100, " %");
            AddIfPositive(lines, "CS de lane avant 10 min", challenges.LaneMinionsFirst10Minutes);
            AddIfPositive(lines, "Avance de niveau sur ton adversaire", challenges.MaxLevelLeadLaneOpponent);
            AddIfPositive(lines, "Avance de score de vision sur ton adversaire", challenges.VisionScoreAdvantageLaneOpponent);
            AddIfPositive(lines, "Kills en solo", challenges.SoloKills);
            AddIfPositive(lines, "Plaques de tour prises", challenges.TurretPlatesTaken);
            AddIfPositive(lines, "Wards de contrôle posées", challenges.ControlWardsPlaced);
            AddIfPositive(lines, "Wards détruites", challenges.WardTakedowns);
            AddIfPositive(lines, "Skillshots esquivés", challenges.SkillshotsDodged);
            AddIfPositive(lines, "Skillshots touchés", challenges.SkillshotsHit);
            AddIfPositive(lines, "Prises de dragon", challenges.DragonTakedowns);
            AddIfPositive(lines, "Objectifs volés", challenges.EpicMonsterSteals);

            if (lines.Count == 0)
            {
                return;
            }

            builder.AppendLine("## STATS AVANCÉES");
            lines.ForEach(x => builder.AppendLine(x));
            builder.AppendLine();
        }

        /// <summary>
        /// Appends both rosters, so the model can reason about matchups and team composition.
        /// </summary>
        /// <param name="builder">Target builder.</param>
        /// <param name="game">The game.</param>
        /// <param name="target">The coached participant.</param>
        private static void AppendRosters(StringBuilder builder, LoLGame game, LoLGameParticipant target)
        {
            var participants = game.LeagueOfLegendsGameParticipants;

            if (participants is null || participants.Count == 0)
            {
                return;
            }

            builder.AppendLine("## COMPOSITIONS");
            builder.AppendLine($"Ton équipe : {DescribeRoster(participants.Where(x => x.TeamId == target.TeamId))}");
            builder.AppendLine($"Équipe adverse : {DescribeRoster(participants.Where(x => x.TeamId != target.TeamId))}");
        }

        /// <summary>
        /// Formats a roster as a comma-separated champion list with roles when known.
        /// </summary>
        /// <param name="participants">Participants of one side.</param>
        /// <returns>Readable roster.</returns>
        private static string DescribeRoster(IEnumerable<LoLGameParticipant> participants)
        {
            return string.Join(", ", participants.Select(x =>
                string.IsNullOrWhiteSpace(x.TeamPosition) ? x.ChampionName : $"{x.ChampionName} ({x.TeamPosition})"));
        }

        /// <summary>
        /// Finds the participant facing the coached player in lane.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="target">The coached participant.</param>
        /// <returns>The direct opponent, or null when roles were not resolved.</returns>
        private static LoLGameParticipant? FindLaneOpponent(LoLGame game, LoLGameParticipant target)
        {
            if (string.IsNullOrWhiteSpace(target.TeamPosition) || game.LeagueOfLegendsGameParticipants is null)
            {
                return null;
            }

            return game.LeagueOfLegendsGameParticipants
                .FirstOrDefault(x => x.TeamId != target.TeamId && x.TeamPosition == target.TeamPosition);
        }

        /// <summary>
        /// Returns the frame closest to the requested moment, provided it is close enough to be honest about.
        /// </summary>
        /// <param name="frames">Timeline frames.</param>
        /// <param name="timestampMs">Requested moment, in milliseconds.</param>
        /// <returns>The nearest frame, or null when the game ended before it.</returns>
        private static LoLGameTimelineFrame? NearestFrame(IReadOnlyCollection<LoLGameTimelineFrame> frames, int timestampMs)
        {
            var frame = frames.MinBy(x => Math.Abs(x.Timestamp - timestampMs));

            return frame is not null && Math.Abs(frame.Timestamp - timestampMs) <= FrameToleranceMs ? frame : null;
        }

        /// <summary>
        /// Describes a map position in the terms a player uses, relative to their own side.
        /// </summary>
        /// <param name="x">Map X coordinate.</param>
        /// <param name="y">Map Y coordinate.</param>
        /// <param name="teamId">Team of the coached player (100 = blue, 200 = red).</param>
        /// <returns>A readable zone, e.g. "bot, moitié ennemie".</returns>
        private static string DescribeZone(int x, int y, int teamId)
        {
            var lane = (y - x) > 2500 ? "top" : (x - y) > 2500 ? "bot" : "mid";
            var inBlueHalf = ((x + y) / 2) < RiftMidpoint;
            var isOwnHalf = (teamId == 100) == inBlueHalf;

            return $"{lane}, moitié {(isOwnHalf ? "alliée" : "ennemie")}";
        }

        /// <summary>
        /// Adds a labelled value to a list, unless it is zero or negative - an uncaptured stat must not be
        /// presented to the model as a measured zero.
        /// </summary>
        /// <param name="lines">Target list.</param>
        /// <param name="label">Human-readable label.</param>
        /// <param name="value">Measured value.</param>
        /// <param name="suffix">Optional unit.</param>
        private static void AddIfPositive(List<string> lines, string label, double value, string suffix = "")
        {
            if (value > 0)
            {
                lines.Add($"{label} : {Number(value)}{suffix}");
            }
        }

        /// <summary>
        /// Formats a timeline timestamp as minutes and seconds.
        /// </summary>
        /// <param name="timestampMs">Timestamp, in milliseconds.</param>
        /// <returns>Readable timestamp.</returns>
        private static string FormatTimestamp(int timestampMs)
        {
            var totalSeconds = timestampMs / 1000;

            return $"{totalSeconds / 60}:{totalSeconds % 60:00}";
        }

        /// <summary>
        /// Formats a number with a single decimal, culture-independently so that the brief does not change
        /// shape with the server locale.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <returns>Formatted number.</returns>
        private static string Number(double value)
        {
            return value.ToString("0.#", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a differential with an explicit sign.
        /// </summary>
        /// <param name="value">Value to format.</param>
        /// <returns>Signed number.</returns>
        private static string Signed(int value)
        {
            return value.ToString("+0;-0;0", CultureInfo.InvariantCulture);
        }
    }
}
