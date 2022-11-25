using UnityEngine;

public class StatusConditionEffect : MonoBehaviour, IEffect
{
    protected StatusCondition[] conditions;

    void Awake()
    {
        this.conditions = this.GetComponentsInChildren<StatusCondition>();

        foreach (var cond in this.conditions)
        {
            if (cond.gameObject == this.gameObject)
            {
                Debug.LogError("Las condiciones de estado deben estar en un GameObject diferente al de la habilidad/efecto");
            }
        }
    }

    public void Resolve(Creature emitter, Creature receiver)
    {
        foreach (var cond in this.conditions)
        {
            // Clonamos el objeto con la condicion de estado
            GameObject parasiteObj = Instantiate(cond.gameObject);

            StatusCondition condition = parasiteObj.GetComponent<StatusCondition>();
            condition.Configure(receiver);

            receiver.AddStatusCondition(condition);
        }
    }
}