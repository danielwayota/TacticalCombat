using UnityEngine;

public class HealEffect : MonoBehaviour, IEffect
{
    [Range(0f, 1f)]
    public float healPercent = 0.2f;

    public void Resolve(Creature emitter, Creature receiver)
    {
        Skill parentSkill = this.GetComponent<Skill>();

        Stats stats = receiver.GetCurrentStats();
        int pointsToHeal = Mathf.RoundToInt(stats.maxhp * this.healPercent);

        int healed = receiver.Heal(pointsToHeal);

        MessageManager.current.Send(new SkillHealthModMessage(parentSkill, emitter, receiver, healed, false, DamageType.OTHER));
    }
}