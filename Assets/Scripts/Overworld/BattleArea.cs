using UnityEngine;

public class BattleArea : MonoBehaviour
{
    public TextAsset mapData;

    [Header("Level ranges")]
    public int minLevel = 3;
    public int maxLevel = 5;

    [Header("Profiles")]
    public CreatureProfile[] aiCreatureProfiles;

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

        CreatureData[] aiCreatures = new CreatureData[this.aiCreatureProfiles.Length];
        for (int i = 0; i < this.aiCreatureProfiles.Length; i++)
        {
            int targetLevel = Random.Range(this.minLevel, this.maxLevel);
            aiCreatures[i] = this.aiCreatureProfiles[i].GenerateDataForLevel(targetLevel);
        }

        // Desactivamos durante 1 segundo.
        this.coolDownTime = 1f;
        OverworldManager.current.StartBattle(this.mapData.text, aiCreatures);
    }
}