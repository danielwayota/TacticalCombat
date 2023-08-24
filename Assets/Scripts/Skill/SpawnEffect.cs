using UnityEngine;

public class SpawnEffect : MonoBehaviour
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
        GameObject go = Instantiate(this.prfb, point, Quaternion.identity);

        Creature posibleNewCreature = go.GetComponent<Creature>();
        if (posibleNewCreature != null)
        {
            // Es una invocación.
            // FIXME: Con el cambio de los CreatureData viniendo del perfil, esto no funciona.
            emitter.master.AdoptCreature(posibleNewCreature);
            BattleManager.current.EmplaceCreature(posibleNewCreature);
        }
    }
}