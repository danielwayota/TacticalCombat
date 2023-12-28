using UnityEngine;

public class CaptureEffect : MonoBehaviour, IEffect
{
    public void Resolve(Creature emitter, Creature receiver)
    {
        if (emitter.master == receiver.master)
        {
            Debug.LogError("Can't capture. Their master is the same.");
            return;
        }

        receiver.ResetLoyalty();
        receiver.master.RemoveCreature(receiver);
        emitter.master.AdoptCreature(receiver);

        MessageManager.current.Send(new CreatureCapturedMessage(emitter, receiver));
    }
}