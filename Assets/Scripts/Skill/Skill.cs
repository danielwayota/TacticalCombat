using UnityEngine;

public class Skill : MonoBehaviour
{
    public float range = 1.5f;
    public int cost = 1;

    public string skillName;

    public GameObject vfx;

    protected IEffect[] effects;

    void Awake()
    {
        this.effects = this.GetComponents<IEffect>();
    }

    public void Resolve(Creature emitter, Creature receiver)
    {
        if (this.effects.Length == 0)
        {
            Debug.LogError($"This skill ({this.skillName}) has no effects!");
            return;
        }

        foreach (var effect in this.effects)
        {
            effect.Resolve(emitter, receiver);
        }

        if (this.vfx != null)
        {
            GameObject go = Instantiate(this.vfx, receiver.transform.position, Quaternion.identity);
            Destroy(go, 2f);
        }
    }
}
