using UnityEngine;

public class ParalysisStatusCondition : StatusCondition
{
    public override void ApplyOnTurnStart(Stats targetStats)
    {
        int impediment = Random.Range(0, 2);
        targetStats.energy -= impediment;
    }

    public override void ApplyStatsModifiers(Stats targetStats)
    { }
}