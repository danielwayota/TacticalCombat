using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Vector2Int localPosition;

    public GameObject selectionIndicator;

    public float movementSpeed = 4f;

    private bool isSelected = false;

    public Master master;

    public Stats stats;

    private List<StatusCondition> conditions = new List<StatusCondition>();

    public bool isMoving { get; protected set; }

    void Start()
    {
        this.isMoving = false;
        this.SetSelectionStatus(false);
    }

    public Stats GetCurrentStats()
    {
        Stats modedStats = this.stats.Clone();

        foreach (var cond in this.conditions)
        {
            cond.ApplyStatsModifiers(modedStats);
        }

        return modedStats;
    }

    public void ModifyHealth(int amount)
    {
        int newHP = this.stats.hp + amount;

        this.stats.hp = Mathf.Clamp(newHP, 0, this.stats.maxhp);

        if (this.stats.hp == 0)
        {
            this.master.OnCreatureDeath(this);
        }
    }

    public int DamageWithClamp(int amount)
    {
        int targetHp = Mathf.Clamp(this.stats.hp - amount, 1, this.stats.maxhp);
        int damageTaken = this.stats.hp - targetHp;

        this.stats.hp = targetHp;

        return damageTaken;
    }

    public void BeginTurn()
    {
        this.UpdateEnergy(this.stats.maxEnergy);

        for (int i = 0; i < this.conditions.Count; i++)
        {
            StatusCondition cond = this.conditions[i];

            cond.ApplyOnTurnStart(this.stats);
            cond.ConsumeOneTurn();

            if (cond.isDepleted)
            {
                this.conditions.RemoveAt(i);
            }
        }
    }

    public void AddStatusCondition(StatusCondition condition)
    {
        this.conditions.Add(condition);

        condition.transform.position = this.transform.position;
        condition.transform.SetParent(this.transform);
    }

    public int CurrentMaxDistance()
    {
        return this.stats.speed * this.stats.energy;
    }

    private void UpdateEnergy(int e)
    {
        this.stats.energy = e;
        MessageManager.current.Send(new CreatureUpdatedMessage(this));
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
        this.isMoving = true;

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
            MessageManager.current.Send(new CreatureMovedMessage(this));
        }

        this.isMoving = false;
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
