using System.Collections;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Vector2Int localPosition;

    public GameObject selectionIndicator;

    public float movementSpeed = 4f;

    private bool isSelected = false;

    public Master master;

    public Stats stats;

    void Start()
    {
        this.Recharge();

        this.SetSelectionStatus(false);
    }

    public Stats GetCurrentStats()
    {
        // TODO: Aplicar powerups y demás historia
        return this.stats;
    }

    public void ModifyHealth(int amount)
    {
        int newHP = this.stats.hp + amount;

        this.stats.hp = Mathf.Clamp(newHP, 0, this.stats.maxhp);
    }

    public void Recharge()
    {
        this.UpdateEnergy(this.stats.maxEnergy);
    }

    public int CurrentMaxDistance()
    {
        return this.stats.speed * this.stats.energy;
    }

    private void UpdateEnergy(int e)
    {
        this.stats.energy = e;
        CreatureUI.current.DisplayEnergy(e);
    }

    public bool CanExecuteSkill(Skill skill)
    {
        return this.stats.energy >= skill.cost;
    }

    public void ConsumeEnergyFor(Skill skill)
    {
        this.UpdateEnergy(this.stats.energy - skill.cost);
    }

    public void SetSelectionStatus(bool isSelected)
    {
        this.selectionIndicator.SetActive(isSelected);
        this.isSelected = isSelected;
    }

    public void FollowPath(Vector3[] worldPath)
    {
        StopAllCoroutines();
        StartCoroutine(this.FollowPathRutine(worldPath));
    }

    private IEnumerator FollowPathRutine(Vector3[] worldPath)
    {
        int pathLength = Mathf.Min(this.CurrentMaxDistance(), worldPath.Length);
        int cost = this.GetEnergyCostForPathLength(pathLength);

        this.UpdateEnergy(this.stats.energy - cost);

        for (int i = 0; i < pathLength; i++)
        {
            Vector3 target = worldPath[i];

            float percent = 0;

            Vector3 start = this.transform.position;

            while (percent < 1f)
            {
                this.transform.position = Vector3.Lerp(start, target, percent);

                percent += Time.deltaTime * this.movementSpeed;
                yield return null;
            }

            this.transform.position = target;
        }
    }

    public int GetEnergyCostForPathLength(int length)
    {
        // speed = 4
        // 2 / 4 = 0.5 -> ceil 1
        // 5 / 4 = 1.25   -> ceil 2

        int cost = Mathf.CeilToInt(length / (float)this.stats.speed);

        return cost;
    }

    public Skill[] GetSkills()
    {
        return this.GetComponentsInChildren<Skill>();
    }
}
