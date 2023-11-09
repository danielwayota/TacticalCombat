using System.Collections.Generic;
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

    public List<GameObject> skillPrefabs { get; protected set; }

    public CreatureData(
        string id,
        GameObject prefab,
        Stats stats,
        CreatureProfile profile,
        List<GameObject> skillPrefabs)
    {
        this.id = id;
        this.prefab = prefab;
        this.stats = stats;
        this.profile = profile;
        this.skillPrefabs = skillPrefabs;
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

    public void ChangeSkillOrderByIndex(int startIndex, int endIndex)
    {
        if (endIndex < 0 || startIndex < 0)
            return;

        if (endIndex >= this.skillPrefabs.Count || startIndex >= this.skillPrefabs.Count)
            return;

        GameObject tmp = this.skillPrefabs[startIndex];
        this.skillPrefabs[startIndex] = this.skillPrefabs[endIndex];
        this.skillPrefabs[endIndex] = tmp;
    }

    public void AddExperience(ShadowStats shadowExp)
    {
        this.stats.GetShadow().Sum(shadowExp);
    }

    public CreatureData Clone()
    {
        List<GameObject> skillPrefabCopy = new List<GameObject>();
        if (this.skillPrefabs != null)
        {
            foreach (var prfb in this.skillPrefabs)
            {
                skillPrefabCopy.Add(prfb);
            }
        }

        return new CreatureData(
            this.id,
            this.prefab,
            this.stats.Clone(),
            this.profile,
            skillPrefabCopy
        );
    }
}