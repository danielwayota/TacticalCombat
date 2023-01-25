public class BattleOverMessage : Message
{
    public override MessageTag tag => MessageTag.BATTLE_OVER;

    public Master winner { get; protected set; }
    public Master losser { get; protected set; }

    public BattleOverMessage(Master winner, Master losser)
    {
        this.winner = winner;
        this.losser = losser;
    }
}