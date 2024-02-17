using UnityEngine;

using System.Collections.Generic;

public class Skill : MonoBehaviour
{
    public ElementalType elementalType;

    public float range = 1.5f;
    public float area = 0;

    public int cost = 1;

    public float distancePenalizationMultiplier = 0.1f;

    public string skillName;

    public GameObject vfx;

    protected IEffect[] effects;

    public float currentDistancePenalization { get; protected set; }

    protected ISpawner spawnEffect;
    public bool isSpawner = false;

    void Awake()
    {
        this.effects = this.GetComponents<IEffect>();
        this.spawnEffect = this.GetComponent<ISpawner>();
    }

    public void ResolveForReceiver(Creature emitter, Creature receiver)
    {
        float tileDistance = Vector3.Distance(emitter.transform.position, receiver.transform.position);
        this.currentDistancePenalization = (-tileDistance + 2) * this.distancePenalizationMultiplier;

        if (this.effects.Length == 0)
        {
            Debug.LogError($"This skill ({this.skillName}) has no effects!");
            return;
        }

        bool canHit = this.CalculateIfCanHit(emitter, receiver);
        if (canHit)
        {
            foreach (var effect in this.effects)
            {
                effect.Resolve(emitter, receiver);
            }
        }
        else
        {
            MessageManager.current.Send(new SkillMissMessage(this, receiver));
        }

        if (this.vfx != null)
        {
            GameObject go = Instantiate(this.vfx, receiver.transform.position, Quaternion.identity);
            go.SetActive(true);
            Destroy(go, 2f);
        }
    }

    public void ResolveAsSpawner(Creature emitter, List<Vector3> area)
    {
        foreach (var point in area)
        {
            this.spawnEffect.ResolveAtPoint(emitter, point);

            if (this.vfx != null)
            {
                GameObject go = Instantiate(this.vfx, point, Quaternion.identity);
                go.SetActive(true);
                Destroy(go, 2f);
            }
        }
    }

    public virtual float CalculateHitChance(Creature emitter, Creature receiver)
    {
        Stats eStats = emitter.GetCurrentStats();
        Stats rStats = receiver.GetCurrentStats();

        float hitChance = 1f - Mathf.Max(rStats.evasion - eStats.accuracy, 0) / (float)rStats.evasion;
        hitChance += this.currentDistancePenalization;

        return hitChance;
    }

    protected bool CalculateIfCanHit(Creature emitter, Creature receiver)
    {
        float hitChance = this.CalculateHitChance(emitter, receiver);

        float dice = Random.Range(0f, 1f);
        return dice < hitChance;
    }
}
