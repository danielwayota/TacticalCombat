public enum MessageTag
{
    NEXT_TURN,
    SKILL_MISS,
    SKILL_HEALTH_MOD,

    CREATURE_SELECTED,
    CREATURE_UPDATED,
    CREATURE_MOVED,
    CREATURE_DEFEATED,
    CREATURE_CAPTURED,

    ACTION_CREATURE_MOVE,
    ACTION_CREATURE_SKILL,

    BATTLE_OVER
}

public abstract class Message
{
    public abstract MessageTag tag { get; }
}