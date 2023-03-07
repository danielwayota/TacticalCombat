using UnityEngine;

public class PoisonStatusCondition : StatusCondition
{
    public float damagePercent = 0.2f;
    public GameObject onApplyVfx;

    protected override void ExecuteOnTurnStart(Stats targetStats)
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

    protected override void ExecuteStatsModifiers(Stats targetStats)
    {

    }
}