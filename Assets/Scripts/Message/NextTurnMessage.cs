public class NextTurnMessage : Message
{
    public override MessageTag tag => MessageTag.NEXT_TURN;

    public Master currentTurnMaster { get; protected set; }

    public NextTurnMessage(Master currentTurnMaster)
    {
        this.currentTurnMaster = currentTurnMaster;
    }

    public override string ToString()
    {
        return $"{this.currentTurnMaster.masterName} has the turn!";
    }
}