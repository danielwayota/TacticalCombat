using UnityEngine;

public class PoisonStatusCondition : StatusCondition
{
    public float damagePercent = 0.2f;
    public GameObject onApplyVfx;

    public override void ApplyOnTurnStart(Stats targetStats)
    {
        int damage = Mathf.RoundToInt(this.damagePercent * (float)targetStats.maxhp);

        int damageTaken = this.targetCreature.DamageWithClamp(damage);
        if (damageTaken != 0)
        {
            MessageManager.current.Send(new SkillDamageMessage(null, this.targetCreature, damageTaken, false));
        }

        if (this.onApplyVfx != null)
        {
            GameObject effect = Instantiate(this.onApplyVfx, this.targetCreature.transform.position, Quaternion.identity);
            Destroy(effect, 2.0f);
        }
    }

    public override void ApplyStatsModifiers(Stats targetStats)
    {

    }
}