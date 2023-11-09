using System.Collections.Generic;

public class BattleOverMessage : Message
{
    public override MessageTag tag => MessageTag.BATTLE_OVER;

    public Master winner { get; protected set; }

    public List<BattleOverCreatureData> creatureBattleOverData { get; protected set; }

    public bool hasWinner { get => this.winner != null; }

    private BattleOverMessage(Master winner, List<BattleOverCreatureData> creatureFinalData)
    {
        this.winner = winner;

        this.creatureBattleOverData = creatureFinalData;
    }

    public static BattleOverMessage CreateForWinner(Master winner, List<BattleOverCreatureData> creatureFinalData = null)
    {
        if (creatureFinalData == null)
        {
            return new BattleOverMessage(winner, new List<BattleOverCreatureData>());
        }

        return new BattleOverMessage(winner, creatureFinalData);
    }

    public static BattleOverMessage CreateForFlee(List<BattleOverCreatureData> creatureFinalData)
    {
        return new BattleOverMessage(null, creatureFinalData);
    }
}