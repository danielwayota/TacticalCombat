using UnityEngine;

public interface ISpawner
{
    public void ResolveAtPoint(Creature emitter, Vector3 point);
}
