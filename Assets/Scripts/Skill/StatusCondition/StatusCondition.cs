using UnityEngine;

public enum StatusConditionFilterType
{
    ARE_IMMUNE,
    ARE_VULNERABLE
}

public abstract class StatusCondition : MonoBehaviour
{
    public bool isDepleted { get => this.remainingTurns <= 0; }

    [Header("Immunities")]
    public StatusConditionFilterType filterType = StatusConditionFilterType.ARE_IMMUNE;
    public ElementalType[] typesFilter;

    public int turnCount = 1;
    protected int remainingTurns;

    protected Creature targetCreature;

    public void Configure(Creature targetCreature)
    {
        this.remainingTurns = this.turnCount;
        this.targetCreature = targetCreature;

        if (this.IsCreatureImmune())
        {
            this.remainingTurns = 0;
        }
    }

    protected bool IsCreatureImmune()
    {
        Stats creatureStats = this.targetCreature.GetCurrentStats();

        if (this.filterType == StatusConditionFilterType.ARE_IMMUNE)
        {
            foreach (var elemType in this.typesFilter)
            {
                if (creatureStats.elementalType == elemType)
                {
                    return true;
                }
            }

            return false;
        }
        else if (this.filterType == StatusConditionFilterType.ARE_VULNERABLE)
        {
            foreach (var elemType in this.typesFilter)
            {
                if (creatureStats.elementalType == elemType)
                {
                    return false;
                }
            }

            return true;
        }

        throw new System.Exception("IsCreatureInmune: Unreachable!");
    }

    public void ConsumeOneTurn()
    {
        this.remainingTurns--;

        if (this.isDepleted)
        {
            Destroy(this.gameObject);
        }
    }

    public void ApplyOnTurnStart(Stats targetStats)
    {
        if (this.isDepleted)
        {
            return;
        }

        this.ExecuteOnTurnStart(targetStats);
    }

    public void ApplyStatsModifiers(Stats targetStats)
    {
        if (this.isDepleted)
        {
            return;
        }

        this.ExecuteStatsModifiers(targetStats);
    }

    protected abstract void ExecuteOnTurnStart(Stats targetStats);
    protected abstract void ExecuteStatsModifiers(Stats targetStats);
}