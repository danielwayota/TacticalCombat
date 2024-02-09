using UnityEngine;

[System.Serializable]
public struct BattleReward
{
    public Item item;
    public int minAmount;
    public int maxAmount;

    [Range(0f, 1f)]
    public float chance;
}

[System.Serializable]
public struct BattleEnemyGroup
{
    [Header("Level ranges")]
    public int minLevel;
    public int maxLevel;

    [Header("Profiles")]
    public CreatureProfile[] creatureProfiles;

    [Header("Rewards")]
    public BattleReward[] posibleRewards;
}

public class BattleArea : MonoBehaviour
{
    public TextAsset mapData;

    public GameObject mapPreab;

    public BattleCategory battleCategory = BattleCategory.RANDOM_ENCOUNTER;

    public BattleEnemyGroup[] enemyGroups;

    private float coolDownTime = 0;

    void Update()
    {
        if (this.coolDownTime > 0)
        {
            this.coolDownTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.coolDownTime > 0)
        {
            // Está desactivado.
            return;
        }

        if (this.enemyGroups.Length == 0)
        {
            Debug.LogError("Este área no tiene grupos definidos!");
            return;
        }

        int index = Random.Range(0, this.enemyGroups.Length);
        BattleEnemyGroup group = this.enemyGroups[index];

        CreatureData[] aiCreatures = new CreatureData[group.creatureProfiles.Length];
        for (int i = 0; i < group.creatureProfiles.Length; i++)
        {
            int targetLevel = Random.Range(group.minLevel, group.maxLevel);
            aiCreatures[i] = group.creatureProfiles[i].GenerateDataForLevel(targetLevel);

            if (this.battleCategory == BattleCategory.BOSS || this.battleCategory == BattleCategory.VS_MASTER)
            {
                aiCreatures[i].stats.ModifyLoyalty(0.9f);
            }
        }

        BattleDescriptor descriptor = new BattleDescriptor
        {
            category = this.battleCategory,

            aiCreatures = aiCreatures,
            posibleRewards = group.posibleRewards
        };

        if (this.mapPreab != null)
        {
            descriptor.mapPrefab = this.mapPreab;
        }
        else if (this.mapData != null)
        {
            descriptor.mapStringData = this.mapData.text;
        }

        // Desactivamos durante 1 segundo.
        this.coolDownTime = 1f;
        OverworldManager.current.StartBattle(descriptor);
    }
}