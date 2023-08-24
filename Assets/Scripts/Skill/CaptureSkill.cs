using UnityEngine;

public class CaptureSkill : Skill
{
    protected override bool CalculateIfCanHit(Stats eStats, Stats rStats)
    {
        float accuracyEffect = 1f - Mathf.Max(rStats.evasion - eStats.accuracy, 0) / (float)rStats.evasion;
        float distanceEffect = this.currentDistancePenalization;
        float healthEffect = 1f - (rStats.hp / rStats.maxhp);
        float levelEffect = eStats.level - rStats.level;

        float captureChance =
            (.3f * accuracyEffect) +
            (.3f * distanceEffect) +
            (.15f * healthEffect) +
            (.05f * levelEffect);

        float dice = Random.Range(0f, 1f);
        return dice < captureChance;
    }
}