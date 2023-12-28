using UnityEngine;

public class CaptureSkill : Skill
{
    public override float CalculateHitChance(Creature emitter, Creature receiver)
    {
        if (emitter.master == receiver.master)
        {
            return 0;
        }

        if (receiver.isMasterCreature)
        {
            // Las criaturas que representan al maestro no pueden ser capturadas
            return 0;
        }

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

        float roundedLoyalty = Mathf.Ceil(rStats.loyalty * 100f) / 100f;
        float inverseLoyalty = 1 - roundedLoyalty;

        return Mathf.Clamp01(captureChance) * inverseLoyalty;
    }
}