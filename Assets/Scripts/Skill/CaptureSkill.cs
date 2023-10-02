using UnityEngine;

public class CaptureSkill : Skill
{
    public override float CalculateHitChance(Creature emitter, Creature receiver)
    {
        Stats eStats = emitter.GetCurrentStats();
        Stats rStats = receiver.GetCurrentStats();

        float accuracyEffect = 1f - Mathf.Max(rStats.evasion - eStats.accuracy, 0) / (float)rStats.evasion;
        float distanceEffect = this.currentDistancePenalization;
        float healthEffect = 1f - (rStats.hp / rStats.maxhp);
        float levelEffect = eStats.level - rStats.level;

        float captureChance =
            (.3f * accuracyEffect) +
            (.3f * distanceEffect) +
            (.15f * healthEffect) +
            (.05f * levelEffect);

        return captureChance;
    }
}