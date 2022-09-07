using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    PHYSICAL,
    ELEMENTAL,
    MIXED
}

public class DamageEffect : MonoBehaviour, IEffect
{
    public DamageType damageType;
    public ElementalType elementalType;

    public int power = 20;

    public void Resolve(Creature emitter, Creature receiver)
    {
        int damage = this.CalculateDamage(emitter.GetCurrentStats(), receiver.GetCurrentStats());

        receiver.ModifyHealth(-damage);
    }

    protected int CalculateDamage(Stats emitterStats, Stats receiverStats)
    {
        // Fórmula: https://bulbapedia.bulbagarden.net/wiki/Damage
        float AD = this.CalculateAD(emitterStats, receiverStats);
        float rawDamage = (((2 * emitterStats.level) / 5) + 2) * this.power * AD;
        rawDamage = (rawDamage / 50) + 2;

        rawDamage *= this.GetElementalMultiplier(emitterStats.elementalType, this.elementalType, receiverStats.elementalType);

        return Mathf.RoundToInt(rawDamage);
    }

    protected float CalculateAD(Stats emitterStats, Stats receiverStats)
    {
        if (this.damageType == DamageType.PHYSICAL)
        {
            return emitterStats.attack / receiverStats.defense;
        }

        if (this.damageType == DamageType.ELEMENTAL)
        {
            return emitterStats.elemAttack / receiverStats.elemDefense;
        }

        if (this.damageType == DamageType.MIXED)
        {
            float physical = (emitterStats.attack / receiverStats.defense) / 2f;
            float elemental = (emitterStats.elemAttack / receiverStats.elemDefense) / 2f;

            return physical + elemental;
        }

        return 0;
    }

    protected float GetElementalMultiplier(ElementalType emitterType, ElementalType skillType, ElementalType receiverType)
    {
        float multiplier = 1f;

        // Bonus del mismo tipo
        if (emitterType == skillType)
        {
            multiplier = 1.5f;
        }

        multiplier *= ElementalWeaknessDB.GetWeaknessMultiplier(skillType, receiverType);

        return multiplier;
    }
}
