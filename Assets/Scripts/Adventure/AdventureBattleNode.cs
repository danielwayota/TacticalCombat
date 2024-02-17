using UnityEngine;

public class AdventureBattleNode : AdventureMapNode
{
    private GameObject mapPrefab;
    private CreatureData[] creatures;
    private BattleReward[] posibleRewards;

    public void Configure(BattleEnemyGroup group, GameObject mapPrfb)
    {
        this.mapPrefab = mapPrfb;

        this.creatures = group.GenerateCreatureData();
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
                this.MarkAsVisited();
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