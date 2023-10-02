public class SkillHitChanceRequest : Message
{
    public override MessageTag tag => MessageTag.REQUEST_SKILL_HIT_CHANCE;

    public Skill skill { get; protected set; }
    public float chance { get; protected set; }

    // NOTE: Si la 'skill' es nula, la idea es ocultar el cuadro.
    public SkillHitChanceRequest(Skill skill, float chance)
    {
        this.skill = skill;
        this.chance = chance;
    }
}