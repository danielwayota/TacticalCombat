public enum MessageTag
{
    NEXT_TURN,
    SKILL_MISS,
    SKILL_DAMAGE,

    CREATURE_SELECTED,
    CREATURE_UPDATED,
    CREATURE_MOVED,

    ACTION_CREATURE_MOVE,
    ACTION_CREATURE_SKILL,

    BATTLE_OVER
}

public abstract class Message
{
    public abstract MessageTag tag { get; }
}