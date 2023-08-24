using UnityEngine;

using System.Collections.Generic;

public class ExperienceManager : MonoBehaviour, IMessageListener
{
    public static ExperienceManager current;

    public float boost = 0.015f;

    private Dictionary<Creature, ShadowStats> creatureEffortExpDeltas;

    void Awake()
    {
        current = this;

        this.creatureEffortExpDeltas = new Dictionary<Creature, ShadowStats>();

        MessageManager.current.AddListener(MessageTag.CREATURE_MOVED, this);
        MessageManager.current.AddListener(MessageTag.SKILL_HEALTH_MOD, this);
        MessageManager.current.AddListener(MessageTag.CREATURE_DEFEATED, this);
        MessageManager.current.AddListener(MessageTag.CREATURE_CAPTURED, this);
    }

    public void Receive(Message msg)
    {
        if (msg is CreatureMovedMessage)
        {
            var cmm = msg as CreatureMovedMessage;
            if (cmm.creature.belongToHuman == false)
            {
                // Ignoramos a los enemigos
                return;
            }

            this.IncreaseSpeedEffortExp(cmm.creature, cmm.pathLength);
        }

        if (msg is SkillHealthModMessage)
        {
            var shm = msg as SkillHealthModMessage;

            if (shm.healthModAmount >= 0)
            {
                // Ignoramos curas o daños a 0
                return;
            }

            if (shm.emitter == null)
            {
                // Es una condicion de estado, ignoramos
                return;
            }

            if (shm.emitter.belongToHuman)
            {
                // Hemos golpeado
                this.IncreaseAttackEffortExp(shm.emitter, shm.damageType, shm.critical);
            }

            if (shm.receiver.belongToHuman)
            {
                // Nos han golpeado
                this.IncreaseDefenseEffortExp(shm.receiver, shm.damageType, shm.critical);
            }
        }

        if (msg is CreatureDefeatedMessage)
        {
            var cdm = msg as CreatureDefeatedMessage;

            if (cdm.emitter.belongToHuman == false)
            {
                // Ignoramos a los enemigos
                return;
            }

            this.IncreaseExperience(cdm.emitter, cdm.receiver);
        }

        if (msg is CreatureCapturedMessage)
        {
            var cdm = msg as CreatureCapturedMessage;

            this.IncreaseExperience(cdm.emitter, cdm.receiver);
        }
    }

    private void IncreaseAttackEffortExp(Creature creature, DamageType damageType, bool isCritical)
    {
        ShadowStats shadow = this.GetEffortExpFor(creature);

        if (damageType == DamageType.PHYSICAL)
        {
            shadow.attack += this.boost;
        }
        else if (damageType == DamageType.ELEMENTAL)
        {
            shadow.elemAttack += this.boost;
        }
        else if (damageType == DamageType.MIXED)
        {
            shadow.attack += this.boost / 2f;
            shadow.elemAttack += this.boost / 2f;
        }

        if (isCritical)
        {
            shadow.accuracy += this.boost;
        }

        this.creatureEffortExpDeltas[creature] = shadow;
    }

    private void IncreaseDefenseEffortExp(Creature creature, DamageType damageType, bool isCritical)
    {
        ShadowStats shadow = this.GetEffortExpFor(creature);

        if (damageType == DamageType.PHYSICAL)
        {
            shadow.defense += this.boost;
        }
        else if (damageType == DamageType.ELEMENTAL)
        {
            shadow.elemDefense += this.boost;
        }
        else if (damageType == DamageType.MIXED)
        {
            shadow.defense += this.boost / 2f;
            shadow.elemDefense += this.boost / 2f;
        }

        if (isCritical)
        {
            // En el lore mental, al recibir un golpe crítico, aprendes algo para defenderte para la próxima :D
            shadow.evasion += this.boost;
        }

        this.creatureEffortExpDeltas[creature] = shadow;
    }

    private void IncreaseSpeedEffortExp(Creature creature, int pathLength)
    {
        ShadowStats shadow = this.GetEffortExpFor(creature);

        // Dividimos entre la velocidad actual para limitar el crecimiento de ésta.
        //    Cuanta más velocidad tenga, menos crecimiento.
        int currentSpeed = creature.GetBaseStats().speed;
        shadow.speed += (this.boost * pathLength) / currentSpeed;

        this.creatureEffortExpDeltas[creature] = shadow;
    }

    private void IncreaseExperience(Creature emitter, Creature receiver)
    {
        int levelDiff = receiver.innerData.level - emitter.innerData.level;
        // 3 - 5 = -2  50 - 16 = +34
        // 5 - 5 =  0            +50
        // 7 - 5 =  2            +66

        int baseExpGain = 50;
        int exp = Mathf.Clamp(baseExpGain + levelDiff * 8, 10, 9999);

        ShadowStats shadow = this.GetEffortExpFor(emitter);
        shadow.experience += exp;

        this.creatureEffortExpDeltas[emitter] = shadow;
    }

    public ShadowStats GetEffortExpFor(Creature creature)
    {
        if (this.creatureEffortExpDeltas.ContainsKey(creature) == false)
        {
            ShadowStats shadow = new ShadowStats();
            this.creatureEffortExpDeltas.Add(creature, shadow);
        }

        return this.creatureEffortExpDeltas[creature];
    }
}