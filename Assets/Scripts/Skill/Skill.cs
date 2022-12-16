using UnityEngine;

public class Skill : MonoBehaviour
{
    public float range = 1.5f;
    public float area = 0;

    public int cost = 1;

    public float distancePenalizationMultiplier = 0.1f;

    public string skillName;

    public GameObject vfx;

    protected IEffect[] effects;

    public float currentDistancePenalization { get; protected set; }

    void Awake()
    {
        this.effects = this.GetComponents<IEffect>();
    }

    public void Resolve(Creature emitter, Creature receiver)
    {
        float tileDistance = Vector3.Distance(emitter.transform.position, receiver.transform.position);
        this.currentDistancePenalization = (-tileDistance + 2) * this.distancePenalizationMultiplier;

        if (this.effects.Length == 0)
        {
            Debug.LogError($"This skill ({this.skillName}) has no effects!");
            return;
        }

        bool canHit = this.CalculateIfCanHit(emitter.GetCurrentStats(), receiver.GetCurrentStats());
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
            Destroy(go, 2f);
        }
    }

    private bool CalculateIfCanHit(Stats eStats, Stats rStats)
    {
        float hitChance = 1f - Mathf.Max(rStats.evasion - eStats.accuracy, 0) / (float)rStats.evasion;
        hitChance += this.currentDistancePenalization;

        float dice = Random.Range(0f, 1f);

        return dice < hitChance;
    }
}
