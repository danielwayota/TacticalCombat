using UnityEngine;

[System.Serializable]
public class BattleEnemyGroup
{
    [Header("Level ranges")]
    public int minLevel;
    public int maxLevel;

    [Header("Profiles")]
    public CreatureProfile[] creatureProfiles;

    [Header("Rewards")]
    public BattleReward[] posibleRewards;

    public CreatureData[] GenerateCreatureData()
    {
        CreatureData[] creatures = new CreatureData[this.creatureProfiles.Length];
        for (int i = 0; i < this.creatureProfiles.Length; i++)
        {
            int targetLevel = Random.Range(this.minLevel, this.maxLevel + 1);
            creatures[i] = this.creatureProfiles[i].GenerateDataForLevel(targetLevel);
        }

        return creatures;
    }
}