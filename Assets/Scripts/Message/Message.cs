public enum MessageTag
{
    NEXT_TURN,
    SKILL_MISS,
    SKILL_DAMAGE
}

public abstract class Message
{
    public abstract MessageTag tag { get; }
}