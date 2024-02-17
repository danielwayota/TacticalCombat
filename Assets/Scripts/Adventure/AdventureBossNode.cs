using UnityEngine;

public class AdventureBossNode : AdventureMapNode
{
    private GameObject mapPrefab;
    private CreatureData[] creatures;
    private BattleReward[] posibleRewards;

    public void Configure(BattleEnemyGroup group, GameObject mapPrfb)
    {
        this.mapPrefab = mapPrfb;

        this.creatures = group.GenerateCreatureData();
        foreach (var creature in this.creatures)
        {
            creature.stats.ModifyLoyalty(0.9f);
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
