using UnityEngine;

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

    public void OnTriggerEnter2D(Collider2D other)
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

        CreatureData[] aiCreatures = group.GenerateCreatureData();
        if (this.battleCategory == BattleCategory.BOSS || this.battleCategory == BattleCategory.VS_MASTER)
        {
            foreach (var creature in aiCreatures)
            {
                creature.stats.ModifyLoyalty(0.9f);
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