using UnityEngine;

[System.Serializable]
public class CreatureData
{
    public string id;

    public GameObject prefab;

    public Stats stats;

    public CreatureData(string id, GameObject prefab, Stats stats)
    {
        this.id = id;
        this.prefab = prefab;
        this.stats = stats;
    }

    public CreatureData Clone()
    {
        return new CreatureData(
            this.id,
            this.prefab,
            this.stats.Clone()
        );
    }
}