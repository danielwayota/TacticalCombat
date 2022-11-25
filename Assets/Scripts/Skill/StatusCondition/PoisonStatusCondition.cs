using UnityEngine;

public class PoisonStatusCondition : StatusCondition
{
    public float damagePercent = 0.2f;
    public GameObject onApplyVfx;

    public override void ApplyOnTurnStart(Stats targetStats)
    {
        int damage = Mathf.RoundToInt(this.damagePercent * (float)targetStats.maxhp);

        int targetHp = Mathf.Clamp(targetStats.hp - damage, 1, targetStats.maxhp);

        int damageTaken = targetStats.hp - targetHp;
        if (damageTaken != 0)
        {
            MessageManager.current.Send(new SkillDamageMessage(null, this.targetCreature, damageTaken, false));
        }

        targetStats.hp = targetHp;

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