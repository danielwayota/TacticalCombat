using UnityEngine;

public class InvokeEffect : MonoBehaviour, ISpawner
{
    public CreatureProfile profile;

    public void ResolveAtPoint(Creature emitter, Vector3 point)
    {
        int targetLevel = emitter.innerData.level;

        CreatureData data = this.profile.GenerateDataForLevel(targetLevel);
        if (data.isMinion == false)
        {
            Debug.LogWarning("This creature cannot be invoked!");
            return;
        }

        emitter.master.CreateCreature(data, point);
    }
}