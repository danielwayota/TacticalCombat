using UnityEngine;
using UnityEngine.Events;

public class BattleDescriptor
{
    public string mapStringData = null;
    public bool HasMapStringData { get => this.mapStringData != null; }

    public GameObject mapPrefab = null;
    public bool HasMapPrefab { get => this.mapPrefab != null; }

    public CreatureData[] humanCreatures = null;
    public CreatureData[] aiCreatures = null;

    public BattleReward[] posibleRewards = new BattleReward[0];

    public BattleCategory category = BattleCategory.RANDOM_ENCOUNTER;

    public UnityAction onHumanWin = () => { };
    public UnityAction onHumanLoss = () => { };

    public void Validate()
    {
        if (this.HasMapStringData == false && this.HasMapPrefab == false)
        {
            Debug.LogError("BattleDescriptor has no map data");
            Debug.Break();
            return;
        }

        if (this.humanCreatures == null || this.humanCreatures.Length == 0)
        {
            Debug.LogError("BattleDescriptor has no human creatures");
            Debug.Break();
            return;
        }

        if (this.aiCreatures == null || this.aiCreatures.Length == 0)
        {
            Debug.LogError("BattleDescriptor has no AI creatures");
            Debug.Break();
            return;
        }
    }
}