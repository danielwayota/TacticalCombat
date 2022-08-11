using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour, IEffect
{
    public int power = 20;

    public void Resolve(Creature emitter, Creature receiver)
    {
        int damage = this.CalculateDamage(emitter.GetCurrentStats(), receiver.GetCurrentStats());

        receiver.ModifyHealth(-damage);
    }

    protected int CalculateDamage(Stats emitterStats, Stats receiverStats)
    {
        // Fórmula: https://bulbapedia.bulbagarden.net/wiki/Damage
        float AD = emitterStats.attack / receiverStats.defense;
        float rawDamage = (((2 * emitterStats.level) / 5) + 2) * this.power * AD;

        return Mathf.RoundToInt((rawDamage / 50) + 2);
    }
}
