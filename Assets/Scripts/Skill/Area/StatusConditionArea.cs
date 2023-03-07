using UnityEngine;

public class StatusConditionArea : StatusConditionEffect, IMessageListener
{
    public bool isDepleted
    {
        get => this.remainingTurns <= 0 || this.remainingUses <= 0;
    }

    public int uses = 99;
    protected int remainingUses;

    public int turnCount = 1;
    protected int remainingTurns;

    void Start()
    {
        this.remainingUses = this.uses;
        this.remainingTurns = this.turnCount;
        this.TryToResolveArea();

        MessageManager.current.AddListener(MessageTag.CREATURE_MOVED, this);
        StatusConditionAreaManager.current.AddArea(this);
    }

    public void ConsumeOneTurn()
    {
        this.remainingTurns--;

        if (this.isDepleted)
        {
            MessageManager.current.RemoveListener(MessageTag.CREATURE_MOVED, this);
            Destroy(this.gameObject);
        }
    }

    public void TryToResolveArea()
    {
        Creature posibleCreature = GameManager.current.GetCreatureAtPosition(this.transform.position);
        if (posibleCreature != null)
        {
            this.ResolveArea(posibleCreature);
        }
    }

    protected void ResolveArea(Creature target)
    {
        this.remainingUses--;
        this.Resolve(null, target);
    }

    public void Receive(Message msg)
    {
        CreatureMovedMessage cmm = msg as CreatureMovedMessage;

        bool intersectPosition = GameManager.current.mapManager.AreSameTile(
            cmm.creature.transform.position,
            this.transform.position
        );

        if (intersectPosition)
        {
            this.ResolveArea(cmm.creature);
        }
    }
}