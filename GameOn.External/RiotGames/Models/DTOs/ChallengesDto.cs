// <copyright file="ChallengesDto.cs" company="LeadOn's Corp'">
// Copyright (c) LeadOn's Corp'. All rights reserved.
// </copyright>

namespace GameOn.External.RiotGames.Models.DTOs
{
    using Newtonsoft.Json;

    /// <summary>
    /// ChallengesDto class.
    /// </summary>
    public class ChallengesDto
    {
        /// <summary>
        /// Gets or sets 12 Assist Streak count.
        /// </summary>
        [JsonProperty("12AssistStreakCount")]
        public int OneTwoAssistStreakCount { get; set; }

        /// <summary>
        /// Gets or sets barron buff gold advantage over threshold.
        /// </summary>
        [JsonProperty("baronBuffGoldAdvantageOverThreshold")]
        public int BaronBuffGoldAdvantageOverThreshold { get; set; }

        /// <summary>
        /// Gets or sets control wqrd time coverage in river or ennemy half.
        /// </summary>
        [JsonProperty("controlWardTimeCoverageInRiverOrEnemyHalf")]
        public float ControlWardTimeCoverageInRiverOrEnemyHalf { get; set; }

        /// <summary>
        /// Gets or sets Earliest baron.
        /// </summary>
        [JsonProperty("earliestBaron")]
        public int EarliestBaron { get; set; }

        /// <summary>
        /// Gets or sets Earliest Dragon Takedown.
        /// </summary>
        [JsonProperty("earliestDragonTakedown")]
        public int EarliestDragonTakedown { get; set; }

        /// <summary>
        /// Gets or sets earliest elder dragon.
        /// </summary>
        [JsonProperty("earliestElderDragon")]
        public int EarliestElderDragon { get; set; }

        /// <summary>
        /// Gets or sets Early Laning Phase Gold Exp Advantage.
        /// </summary>
        [JsonProperty("earlyLaningPhaseGoldExpAdvantage")]
        public int EarlyLaningPhaseGoldExpAdvantage { get; set; }

        /// <summary>
        /// Gets or sets faster support quest completion.
        /// </summary>
        [JsonProperty("fasterSupportQuestCompletion")]
        public int FasterSupportQuestCompletion { get; set; }

        /// <summary>
        /// Gets or sets fastest legendary.
        /// </summary>
        [JsonProperty("fastestLegendary")]
        public int FastestLegendary { get; set; }

        /// <summary>
        /// Gets or sets had AFK Teammate.
        /// </summary>
        [JsonProperty("hadAfkTeammate")]
        public int HadAfkTeammate { get; set; }

        /// <summary>
        /// Gets or sets highest champion damage.
        /// </summary>
        [JsonProperty("highestChampionDamage")]
        public int HighestChampionDamage { get; set; }

        /// <summary>
        /// Gets or sets Highest crowd control score.
        /// </summary>
        [JsonProperty("highestCrowdControlScore")]
        public int HighestCrowdControlScore { get; set; }

        /// <summary>
        /// Gets or sets highest ward kills.
        /// </summary>
        [JsonProperty("highestWardKills")]
        public int HighestWardKills { get; set; }

        /// <summary>
        /// Gets or sets jungler kills early jungle.
        /// </summary>
        [JsonProperty("junglerKillsEarlyJungle")]
        public int JunglerKillsEarlyJungle { get; set; }

        /// <summary>
        /// Gets or sets kills on laners early jungle as jungler.
        /// </summary>
        [JsonProperty("killsOnLanersEarlyJungleAsJungler")]
        public int KillsOnLanersEarlyJungleAsJungler { get; set; }

        /// <summary>
        /// Gets or sets laning phase gold experience advantage.
        /// </summary>
        [JsonProperty("laningPhaseGoldExpAdvantage")]
        public int LaningPhaseGoldExpAdvantage { get; set; }

        /// <summary>
        /// Gets or sets legendary count.
        /// </summary>
        [JsonProperty("legendaryCount")]
        public int LegendaryCount { get; set; }

        /// <summary>
        /// Gets or sets max CS Advantage on Lane opponent.
        /// </summary>
        [JsonProperty("maxCsAdvantageOnLaneOpponent")]
        public float MaxCsAdvantageOnLaneOpponent { get; set; }

        /// <summary>
        /// Gets or sets max level lead lane opponent.
        /// </summary>
        [JsonProperty("maxLevelLeadLaneOpponent")]
        public int MaxLevelLeadLaneOpponent { get; set; }

        /// <summary>
        /// Gets or sets most wards destroyed one sweeper.
        /// </summary>
        [JsonProperty("mostWardsDestroyedOneSweeper")]
        public int MostWardsDestroyedOneSweeper { get; set; }

        /// <summary>
        /// Gets or sets Mythic item used.
        /// </summary>
        [JsonProperty("mythicItemUsed")]
        public int MythicItemUsed { get; set; }

        /// <summary>
        /// Gets or sets played champ select position.
        /// </summary>
        [JsonProperty("playedChampSelectPosition")]
        public int PlayedChampSelectPosition { get; set; }

        /// <summary>
        /// Gets or sets solo turrets lategame.
        /// </summary>
        [JsonProperty("soloTurretsLategame")]
        public int SoloTurretsLategame { get; set; }

        /// <summary>
        /// Gets or sets takedowns first 25 minutes.
        /// </summary>
        [JsonProperty("takedownsFirst25Minutes")]
        public int TakedownsFirst25Minutes { get; set; }

        /// <summary>
        /// Gets or sets teleport takedowns.
        /// </summary>
        [JsonProperty("teleportTakedowns")]
        public int TeleportTakedowns { get; set; }

        /// <summary>
        /// Gets or sets third inhibitor destroyed time.
        /// </summary>
        [JsonProperty("thirdInhibitorDestroyedTime")]
        public int ThirdInhibitorDestroyedTime { get; set; }

        /// <summary>
        /// Gets or sets three wards one sweeper count.
        /// </summary>
        [JsonProperty("threeWardsOneSweeperCount")]
        public int ThreeWardsOneSweeperCount { get; set; }

        /// <summary>
        /// Gets or sets vision Score Advantage Lane Opponent.
        /// </summary>
        [JsonProperty("visionScoreAdvantageLaneOpponent")]
        public float VisionScoreAdvantageLaneOpponent { get; set; }

        /// <summary>
        /// Gets or sets infernal scale pickup.
        /// </summary>
        [JsonProperty("InfernalScalePickup")]
        public int InfernalScalePickup { get; set; }

        /// <summary>
        /// Gets or sets fist bump participation.
        /// </summary>
        [JsonProperty("fistBumpParticipation")]
        public int FistBumpParticipation { get; set; }

        /// <summary>
        /// Gets or sets void monster kill.
        /// </summary>
        [JsonProperty("voidMonsterKill")]
        public int VoidMonsterKill { get; set; }

        /// <summary>
        /// Gets or sets Ability Uses.
        /// </summary>
        [JsonProperty("abilityUses")]
        public int AbilityUses { get; set; }

        /// <summary>
        /// Gets or sets aces before 15 minutes.
        /// </summary>
        [JsonProperty("acesBefore15Minutes")]
        public int AcesBefore15Minutes { get; set; }

        /// <summary>
        /// Gets or sets Allied Jungle Monster Kills.
        /// </summary>
        [JsonProperty("alliedJungleMonsterKills")]
        public float AlliedJungleMonsterKills { get; set; }

        /// <summary>
        /// Gets or sets baron Takedowns.
        /// </summary>
        [JsonProperty("baronTakedowns")]
        public int BaronTakedowns { get; set; }

        /// <summary>
        /// Gets or sets blast cone opposite opponent count.
        /// </summary>
        [JsonProperty("blastConeOppositeOpponentCount")]
        public int BlastConeOppositeOpponentCount { get; set; }

        /// <summary>
        /// Gets or sets bounty gold.
        /// </summary>
        [JsonProperty("bountyGold")]
        public int BountyGold { get; set; }

        /// <summary>
        /// Gets or sets buffs stolen.
        /// </summary>
        [JsonProperty("buffsStolen")]
        public int BuffsStolen { get; set; }

        /// <summary>
        /// Gets or sets complete support quest in time.
        /// </summary>
        [JsonProperty("completeSupportQuestInTime")]
        public int CompleteSupportQuestInTime { get; set; }

        /// <summary>
        /// Gets or sets control wards placed.
        /// </summary>
        [JsonProperty("controlWardsPlaced")]
        public int ControlWardsPlaced { get; set; }

        /// <summary>
        /// Gets or sets damages per minute.
        /// </summary>
        [JsonProperty("damagePerMinute")]
        public float DamagePerMinute { get; set; }

        /// <summary>
        /// Gets or sets damage taken on team percentage.
        /// </summary>
        [JsonProperty("damageTakenOnTeamPercentage")]
        public float DamageTakenOnTeamPercentage { get; set; }

        /// <summary>
        /// Gets or sets danced with rift herald.
        /// </summary>
        [JsonProperty("dancedWithRiftHerald")]
        public int DancedWithRiftHerald { get; set; }

        /// <summary>
        /// Gets or sets deaths by enemy champs.
        /// </summary>
        [JsonProperty("deathsByEnemyChamps")]
        public int DeathsByEnemyChamps { get; set; }

        /// <summary>
        /// Gets or sets dodge skill shots small window.
        /// </summary>
        [JsonProperty("dodgeSkillShotsSmallWindow")]
        public int DodgeSkillShotsSmallWindow { get; set; }

        /// <summary>
        /// Gets or sets double Aces.
        /// </summary>
        [JsonProperty("doubleAces")]
        public int DoubleAces { get; set; }

        /// <summary>
        /// Gets or sets dragon takedowns.
        /// </summary>
        [JsonProperty("dragonTakedowns")]
        public int DragonTakedowns { get; set; }

        /// <summary>
        /// Gets or sets legendary item used.
        /// </summary>
        [JsonProperty("legendaryItemUsed")]
        public List<int> LegendaryItemUsed { get; set; } = new List<int>();

        /// <summary>
        /// Gets or sets effective heal and shielding.
        /// </summary>
        [JsonProperty("effectiveHealAndShielding")]
        public float EffectiveHealAndShielding { get; set; }

        /// <summary>
        /// Gets or sets elder Dragon Kills With Opposing Soul.
        /// </summary>
        [JsonProperty("elderDragonKillsWithOpposingSoul")]
        public int ElderDragonKillsWithOpposingSoul { get; set; }

        /// <summary>
        /// Gets or sets elder dragon multi kills.
        /// </summary>
        [JsonProperty("elderDragonMultikills")]
        public int ElderDragonMultikills { get; set; }

        /// <summary>
        /// Gets or sets enemy champion immobilizations.
        /// </summary>
        [JsonProperty("enemyChampionImmobilizations")]
        public int EnemyChampionImmobilizations { get; set; }

        /// <summary>
        /// Gets or sets enemy jungle monster kills.
        /// </summary>
        [JsonProperty("enemyJungleMonsterKills")]
        public float EnemyJungleMonsterKills { get; set; }

        // TODO: finish this entity
    }
}
