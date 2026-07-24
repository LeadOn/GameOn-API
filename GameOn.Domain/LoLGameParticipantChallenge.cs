// <copyright file="LoLGameParticipantChallenge.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.Domain
{
    using System.Text.Json.Serialization;

    /// <summary>
    /// LoLGameParticipantChallenge class. Mirrors Riot's own derived "challenges" stats for a
    /// participant (KDA, kill participation, damage/min, ...), persisted as-is so the app doesn't
    /// have to recompute what Riot already computes. <see cref="GameOn.External.RiotGames.Models.DTOs.ChallengesDto"/>
    /// is the source DTO this entity mirrors field-for-field, except <c>legendaryItemUsed</c> (a list, not persisted).
    /// </summary>
    public class LoLGameParticipantChallenge
    {
        /// <summary>
        /// Gets or sets the LoL Game Participant ID (shared primary key / foreign key).
        /// </summary>
        public int LoLGameParticipantId { get; set; }

        /// <summary>
        /// Gets or sets 12 assist streak count.
        /// </summary>
        public int OneTwoAssistStreakCount { get; set; }

        /// <summary>
        /// Gets or sets baron buff gold advantage over threshold.
        /// </summary>
        public int BaronBuffGoldAdvantageOverThreshold { get; set; }

        /// <summary>
        /// Gets or sets control ward time coverage in river or enemy half.
        /// </summary>
        public float ControlWardTimeCoverageInRiverOrEnemyHalf { get; set; }

        /// <summary>
        /// Gets or sets earliest baron.
        /// </summary>
        public float EarliestBaron { get; set; }

        /// <summary>
        /// Gets or sets earliest dragon takedown.
        /// </summary>
        public float EarliestDragonTakedown { get; set; }

        /// <summary>
        /// Gets or sets earliest elder dragon.
        /// </summary>
        public float EarliestElderDragon { get; set; }

        /// <summary>
        /// Gets or sets early laning phase gold experience advantage.
        /// </summary>
        public int EarlyLaningPhaseGoldExpAdvantage { get; set; }

        /// <summary>
        /// Gets or sets faster support quest completion.
        /// </summary>
        public int FasterSupportQuestCompletion { get; set; }

        /// <summary>
        /// Gets or sets fastest legendary.
        /// </summary>
        public float FastestLegendary { get; set; }

        /// <summary>
        /// Gets or sets had AFK teammate.
        /// </summary>
        public int HadAfkTeammate { get; set; }

        /// <summary>
        /// Gets or sets highest champion damage.
        /// </summary>
        public int HighestChampionDamage { get; set; }

        /// <summary>
        /// Gets or sets highest crowd control score.
        /// </summary>
        public int HighestCrowdControlScore { get; set; }

        /// <summary>
        /// Gets or sets highest ward kills.
        /// </summary>
        public int HighestWardKills { get; set; }

        /// <summary>
        /// Gets or sets jungler kills early jungle.
        /// </summary>
        public int JunglerKillsEarlyJungle { get; set; }

        /// <summary>
        /// Gets or sets kills on laners early jungle as jungler.
        /// </summary>
        public int KillsOnLanersEarlyJungleAsJungler { get; set; }

        /// <summary>
        /// Gets or sets laning phase gold experience advantage.
        /// </summary>
        public int LaningPhaseGoldExpAdvantage { get; set; }

        /// <summary>
        /// Gets or sets legendary count.
        /// </summary>
        public int LegendaryCount { get; set; }

        /// <summary>
        /// Gets or sets max CS advantage on lane opponent.
        /// </summary>
        public float MaxCsAdvantageOnLaneOpponent { get; set; }

        /// <summary>
        /// Gets or sets max level lead lane opponent.
        /// </summary>
        public int MaxLevelLeadLaneOpponent { get; set; }

        /// <summary>
        /// Gets or sets most wards destroyed one sweeper.
        /// </summary>
        public int MostWardsDestroyedOneSweeper { get; set; }

        /// <summary>
        /// Gets or sets mythic item used.
        /// </summary>
        public int MythicItemUsed { get; set; }

        /// <summary>
        /// Gets or sets played champ select position.
        /// </summary>
        public int PlayedChampSelectPosition { get; set; }

        /// <summary>
        /// Gets or sets solo turrets lategame.
        /// </summary>
        public int SoloTurretsLategame { get; set; }

        /// <summary>
        /// Gets or sets takedowns first 25 minutes.
        /// </summary>
        public int TakedownsFirst25Minutes { get; set; }

        /// <summary>
        /// Gets or sets teleport takedowns.
        /// </summary>
        public int TeleportTakedowns { get; set; }

        /// <summary>
        /// Gets or sets third inhibitor destroyed time.
        /// </summary>
        public float ThirdInhibitorDestroyedTime { get; set; }

        /// <summary>
        /// Gets or sets three wards one sweeper count.
        /// </summary>
        public int ThreeWardsOneSweeperCount { get; set; }

        /// <summary>
        /// Gets or sets vision score advantage lane opponent.
        /// </summary>
        public float VisionScoreAdvantageLaneOpponent { get; set; }

        /// <summary>
        /// Gets or sets infernal scale pickup.
        /// </summary>
        public int InfernalScalePickup { get; set; }

        /// <summary>
        /// Gets or sets fist bump participation.
        /// </summary>
        public int FistBumpParticipation { get; set; }

        /// <summary>
        /// Gets or sets void monster kill.
        /// </summary>
        public int VoidMonsterKill { get; set; }

        /// <summary>
        /// Gets or sets ability uses.
        /// </summary>
        public int AbilityUses { get; set; }

        /// <summary>
        /// Gets or sets aces before 15 minutes.
        /// </summary>
        public int AcesBefore15Minutes { get; set; }

        /// <summary>
        /// Gets or sets allied jungle monster kills.
        /// </summary>
        public float AlliedJungleMonsterKills { get; set; }

        /// <summary>
        /// Gets or sets baron takedowns.
        /// </summary>
        public int BaronTakedowns { get; set; }

        /// <summary>
        /// Gets or sets blast cone opposite opponent count.
        /// </summary>
        public int BlastConeOppositeOpponentCount { get; set; }

        /// <summary>
        /// Gets or sets bounty gold.
        /// </summary>
        public float BountyGold { get; set; }

        /// <summary>
        /// Gets or sets buffs stolen.
        /// </summary>
        public int BuffsStolen { get; set; }

        /// <summary>
        /// Gets or sets complete support quest in time.
        /// </summary>
        public int CompleteSupportQuestInTime { get; set; }

        /// <summary>
        /// Gets or sets control wards placed.
        /// </summary>
        public int ControlWardsPlaced { get; set; }

        /// <summary>
        /// Gets or sets damage per minute.
        /// </summary>
        public float DamagePerMinute { get; set; }

        /// <summary>
        /// Gets or sets damage taken on team percentage.
        /// </summary>
        public float DamageTakenOnTeamPercentage { get; set; }

        /// <summary>
        /// Gets or sets danced with rift herald.
        /// </summary>
        public int DancedWithRiftHerald { get; set; }

        /// <summary>
        /// Gets or sets deaths by enemy champs.
        /// </summary>
        public int DeathsByEnemyChamps { get; set; }

        /// <summary>
        /// Gets or sets dodge skill shots small window.
        /// </summary>
        public int DodgeSkillShotsSmallWindow { get; set; }

        /// <summary>
        /// Gets or sets double aces.
        /// </summary>
        public int DoubleAces { get; set; }

        /// <summary>
        /// Gets or sets dragon takedowns.
        /// </summary>
        public int DragonTakedowns { get; set; }

        /// <summary>
        /// Gets or sets effective heal and shielding.
        /// </summary>
        public float EffectiveHealAndShielding { get; set; }

        /// <summary>
        /// Gets or sets elder dragon kills with opposing soul.
        /// </summary>
        public int ElderDragonKillsWithOpposingSoul { get; set; }

        /// <summary>
        /// Gets or sets elder dragon multikills.
        /// </summary>
        public int ElderDragonMultikills { get; set; }

        /// <summary>
        /// Gets or sets enemy champion immobilizations.
        /// </summary>
        public int EnemyChampionImmobilizations { get; set; }

        /// <summary>
        /// Gets or sets enemy jungle monster kills.
        /// </summary>
        public float EnemyJungleMonsterKills { get; set; }

        /// <summary>
        /// Gets or sets epic monster kills near enemy jungler.
        /// </summary>
        public int EpicMonsterKillsNearEnemyJungler { get; set; }

        /// <summary>
        /// Gets or sets epic monster kills within 30 seconds of spawn.
        /// </summary>
        public int EpicMonsterKillsWithin30SecondsOfSpawn { get; set; }

        /// <summary>
        /// Gets or sets epic monster steals.
        /// </summary>
        public int EpicMonsterSteals { get; set; }

        /// <summary>
        /// Gets or sets epic monster stolen without smite.
        /// </summary>
        public int EpicMonsterStolenWithoutSmite { get; set; }

        /// <summary>
        /// Gets or sets flawless aces.
        /// </summary>
        public int FlawlessAces { get; set; }

        /// <summary>
        /// Gets or sets full team takedown.
        /// </summary>
        public int FullTeamTakedown { get; set; }

        /// <summary>
        /// Gets or sets game length, in seconds.
        /// </summary>
        public float GameLength { get; set; }

        /// <summary>
        /// Gets or sets gold earned per minute.
        /// </summary>
        public float GoldPerMinute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the team had an open nexus.
        /// </summary>
        public int HadOpenNexus { get; set; }

        /// <summary>
        /// Gets or sets immobilize and kill with ally.
        /// </summary>
        public int ImmobilizeAndKillWithAlly { get; set; }

        /// <summary>
        /// Gets or sets jungle CS before 10 minutes.
        /// </summary>
        public float JungleCsBefore10Minutes { get; set; }

        /// <summary>
        /// Gets or sets jungler takedowns near damaged epic monster.
        /// </summary>
        public int JunglerTakedownsNearDamagedEpicMonster { get; set; }

        /// <summary>
        /// Gets or sets the KDA ratio, as computed by Riot.
        /// </summary>
        public float Kda { get; set; }

        /// <summary>
        /// Gets or sets kill after hidden with ally.
        /// </summary>
        public int KillAfterHiddenWithAlly { get; set; }

        /// <summary>
        /// Gets or sets the kill participation percentage (0-1), as computed by Riot.
        /// </summary>
        public float KillParticipation { get; set; }

        /// <summary>
        /// Gets or sets kills near enemy turret.
        /// </summary>
        public int KillsNearEnemyTurret { get; set; }

        /// <summary>
        /// Gets or sets kills on other lanes early as jungler on a laner.
        /// </summary>
        public int KillsOnOtherLanesEarlyJungleAsLaner { get; set; }

        /// <summary>
        /// Gets or sets kills under own turret.
        /// </summary>
        public int KillsUnderOwnTurret { get; set; }

        /// <summary>
        /// Gets or sets kills with help from an epic monster buff.
        /// </summary>
        public int KillsWithHelpFromEpicMonster { get; set; }

        /// <summary>
        /// Gets or sets knock enemy into team and kill.
        /// </summary>
        public int KnockEnemyIntoTeamAndKill { get; set; }

        /// <summary>
        /// Gets or sets turrets destroyed before plates fall.
        /// </summary>
        public int KTurretsDestroyedBeforePlatesFall { get; set; }

        /// <summary>
        /// Gets or sets land skill shots early game.
        /// </summary>
        public int LandSkillShotsEarlyGame { get; set; }

        /// <summary>
        /// Gets or sets lane minions killed in the first 10 minutes.
        /// </summary>
        public int LaneMinionsFirst10Minutes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether an inhibitor was lost.
        /// </summary>
        public int LostAnInhibitor { get; set; }

        /// <summary>
        /// Gets or sets the maximum kill deficit.
        /// </summary>
        public int MaxKillDeficit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Mejai's was fully stacked in time.
        /// </summary>
        public int MejaisFullStackInTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether more enemy jungle was taken than the opponent jungler.
        /// </summary>
        public float MoreEnemyJungleThanOpponent { get; set; }

        /// <summary>
        /// Gets or sets multikills with a single spell.
        /// </summary>
        public int MultiKillOneSpell { get; set; }

        /// <summary>
        /// Gets or sets the total number of multikills.
        /// </summary>
        public int Multikills { get; set; }

        /// <summary>
        /// Gets or sets multikills after an aggressive flash.
        /// </summary>
        public int MultikillsAfterAggressiveFlash { get; set; }

        /// <summary>
        /// Gets or sets multi rift herald turret count.
        /// </summary>
        public int MultiTurretRiftHeraldCount { get; set; }

        /// <summary>
        /// Gets or sets outer turret executes before 10 minutes.
        /// </summary>
        public int OuterTurretExecutesBefore10Minutes { get; set; }

        /// <summary>
        /// Gets or sets outnumbered kills.
        /// </summary>
        public int OutnumberedKills { get; set; }

        /// <summary>
        /// Gets or sets outnumbered nexus kill.
        /// </summary>
        public int OutnumberedNexusKill { get; set; }

        /// <summary>
        /// Gets or sets perfect dragon souls taken.
        /// </summary>
        public int PerfectDragonSoulsTaken { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the game was a perfect game.
        /// </summary>
        public int PerfectGame { get; set; }

        /// <summary>
        /// Gets or sets pick kill with ally.
        /// </summary>
        public int PickKillWithAlly { get; set; }

        /// <summary>
        /// Gets or sets poro explosions.
        /// </summary>
        public int PoroExplosions { get; set; }

        /// <summary>
        /// Gets or sets quick cleanse.
        /// </summary>
        public int QuickCleanse { get; set; }

        /// <summary>
        /// Gets or sets quick first turret.
        /// </summary>
        public int QuickFirstTurret { get; set; }

        /// <summary>
        /// Gets or sets rift herald takedowns.
        /// </summary>
        public int RiftHeraldTakedowns { get; set; }

        /// <summary>
        /// Gets or sets save ally from death.
        /// </summary>
        public int SaveAllyFromDeath { get; set; }

        /// <summary>
        /// Gets or sets scuttle crab kills.
        /// </summary>
        public int ScuttleCrabKills { get; set; }

        /// <summary>
        /// Gets or sets skill shots dodged.
        /// </summary>
        public int SkillshotsDodged { get; set; }

        /// <summary>
        /// Gets or sets skill shots hit.
        /// </summary>
        public int SkillshotsHit { get; set; }

        /// <summary>
        /// Gets or sets snowballs hit.
        /// </summary>
        public int SnowballsHit { get; set; }

        /// <summary>
        /// Gets or sets solo baron kills.
        /// </summary>
        public int SoloBaronKills { get; set; }

        /// <summary>
        /// Gets or sets solo kills.
        /// </summary>
        public int SoloKills { get; set; }

        /// <summary>
        /// Gets or sets stealth wards placed.
        /// </summary>
        public int StealthWardsPlaced { get; set; }

        /// <summary>
        /// Gets or sets survived single digit HP count.
        /// </summary>
        public int SurvivedSingleDigitHpCount { get; set; }

        /// <summary>
        /// Gets or sets survived three immobilizes in a single fight.
        /// </summary>
        public int SurvivedThreeImmobilizesInFight { get; set; }

        /// <summary>
        /// Gets or sets takedown on the first turret.
        /// </summary>
        public int TakedownOnFirstTurret { get; set; }

        /// <summary>
        /// Gets or sets the total number of takedowns.
        /// </summary>
        public int Takedowns { get; set; }

        /// <summary>
        /// Gets or sets takedowns after gaining a level advantage.
        /// </summary>
        public int TakedownsAfterGainingLevelAdvantage { get; set; }

        /// <summary>
        /// Gets or sets takedowns before jungle minions spawn.
        /// </summary>
        public int TakedownsBeforeJungleMinionSpawn { get; set; }

        /// <summary>
        /// Gets or sets takedowns in the enemy fountain.
        /// </summary>
        public int TakedownsInEnemyFountain { get; set; }

        /// <summary>
        /// Gets or sets team baron kills.
        /// </summary>
        public int TeamBaronKills { get; set; }

        /// <summary>
        /// Gets or sets the percentage of the team's total damage dealt to champions.
        /// </summary>
        public float TeamDamagePercentage { get; set; }

        /// <summary>
        /// Gets or sets team elder dragon kills.
        /// </summary>
        public int TeamElderDragonKills { get; set; }

        /// <summary>
        /// Gets or sets team rift herald kills.
        /// </summary>
        public int TeamRiftHeraldKills { get; set; }

        /// <summary>
        /// Gets or sets took large damage and survived.
        /// </summary>
        public int TookLargeDamageSurvived { get; set; }

        /// <summary>
        /// Gets or sets turret plates taken.
        /// </summary>
        public int TurretPlatesTaken { get; set; }

        /// <summary>
        /// Gets or sets turrets taken with rift herald.
        /// </summary>
        public int TurretsTakenWithRiftHerald { get; set; }

        /// <summary>
        /// Gets or sets turret takedowns.
        /// </summary>
        public int TurretTakedowns { get; set; }

        /// <summary>
        /// Gets or sets twenty minions killed in 3 seconds count.
        /// </summary>
        public int TwentyMinionsIn3SecondsCount { get; set; }

        /// <summary>
        /// Gets or sets unseen recalls.
        /// </summary>
        public int UnseenRecalls { get; set; }

        /// <summary>
        /// Gets or sets the vision score per minute.
        /// </summary>
        public float VisionScorePerMinute { get; set; }

        /// <summary>
        /// Gets or sets wards guarded.
        /// </summary>
        public int WardsGuarded { get; set; }

        /// <summary>
        /// Gets or sets ward takedowns.
        /// </summary>
        public int WardTakedowns { get; set; }

        /// <summary>
        /// Gets or sets ward takedowns before 20 minutes.
        /// </summary>
        public int WardTakedownsBefore20M { get; set; }

        /// <summary>
        /// Gets or sets the participant these challenges belong to.
        /// </summary>
        [JsonIgnore]
        public virtual LoLGameParticipant Participant { get; set; } = null!;
    }
}
