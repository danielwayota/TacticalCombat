using UnityEngine;

public class SpawnEffect : MonoBehaviour, ISpawner
{
    public GameObject prfb;

    [Header("Necesita un tile sin criaturas")]
    public bool requiresEmptySpace = false;

    public void ResolveAtPoint(Creature emitter, Vector3 point)
    {
        if (this.requiresEmptySpace)
        {
            Creature posibleCreature = BattleManager.current.GetCreatureAtPosition(point);
            if (posibleCreature != null)
            {
                return;
            }
        }

        Instantiate(this.prfb, point, Quaternion.identity);
    }
}