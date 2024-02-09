using UnityEngine;

public class AdventureBossNode : AdventureMapNode
{
    private GameObject mapPrefab;
    private CreatureData[] creatures;
    private BattleReward[] posibleRewards;

    public void Configure(BattleEnemyGroup group, GameObject mapPrfb)
    {
        this.mapPrefab = mapPrfb;

        this.creatures = new CreatureData[group.creatureProfiles.Length];
        for (int i = 0; i < group.creatureProfiles.Length; i++)
        {
            int targetLevel = Random.Range(group.minLevel, group.maxLevel);

            CreatureProfile profile = group.creatureProfiles[i];
            this.creatures[i] = profile.GenerateDataForLevel(targetLevel);

            float loyalty = Random.Range(0.9f, 1f);
            this.creatures[i].stats.ModifyLoyalty(loyalty);
        }

        this.posibleRewards = group.posibleRewards;
    }

    public override void Visit()
    {
        BattleDescriptor descriptor = new BattleDescriptor
        {
            mapPrefab = this.mapPrefab,
            aiCreatures = this.creatures,
            posibleRewards = this.posibleRewards,

            onHumanWin = () =>
            {
                Debug.LogWarning("Hemos ganado pero no hay más niveles!");
                Debug.Break();
            },
            onHumanLoss = () =>
            {
                Debug.LogWarning("Hemos perdido!");
                Debug.Break();
            }
        };

        OverworldManager.current.StartBattle(descriptor);
    }
}
