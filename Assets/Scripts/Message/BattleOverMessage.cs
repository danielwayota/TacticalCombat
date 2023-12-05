using System.Collections.Generic;

public class BattleOverMessage : Message
{
    public override MessageTag tag => MessageTag.BATTLE_OVER;

    public Master winner { get; protected set; }

    public List<BattleOverCreatureData> creatureBattleOverData { get; protected set; }

    public List<ItemStack> itemRewards { get; protected set; }

    public bool hasWinner { get => this.winner != null; }

    private BattleOverMessage(Master winner, List<BattleOverCreatureData> creatureFinalData, List<ItemStack> itemRewards)
    {
        this.winner = winner;

        this.creatureBattleOverData = creatureFinalData;
        this.itemRewards = itemRewards;
    }

    public static BattleOverMessage CreateForWin(
        Master winner, List<BattleOverCreatureData> creatureFinalData, List<ItemStack> itemRewards
    )
    {
        return new BattleOverMessage(winner, creatureFinalData, itemRewards);
    }

    public static BattleOverMessage CreateForLoss(Master winner)
    {
        return new BattleOverMessage(winner, new List<BattleOverCreatureData>(), new List<ItemStack>());
    }

    public static BattleOverMessage CreateForFlee(List<BattleOverCreatureData> creatureFinalData)
    {
        return new BattleOverMessage(null, creatureFinalData, new List<ItemStack>());
    }
}