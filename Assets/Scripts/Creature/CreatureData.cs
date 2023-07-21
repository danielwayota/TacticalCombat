using UnityEngine;

[System.Serializable]
public class CreatureData
{
    public string id;

    public GameObject prefab;

    public Stats stats;

    public int level { get => this.stats.level; }

    public CreatureProfile profile { get; protected set; }

    public int experience { get => this.stats.experience; }

    public CreatureData(string id, GameObject prefab, Stats stats)
    {
        this.id = id;
        this.prefab = prefab;
        this.stats = stats;
    }

    public void SetParentProfile(CreatureProfile profile)
    {
        if (this.profile != null)
        {
            Debug.LogError("This creature already has a profile");
            return;
        }

        this.profile = profile;
    }

    public void AddExperience(ShadowStats shadowExp)
    {
        this.stats.GetShadow().Sum(shadowExp);
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