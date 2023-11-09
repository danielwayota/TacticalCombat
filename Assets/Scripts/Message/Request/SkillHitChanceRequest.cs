public class SkillHitChanceRequest : Message
{
    public override MessageTag tag => MessageTag.REQUEST_SKILL_HIT_CHANCE;

    public Skill skill { get; protected set; }
    public float chance { get; protected set; }

    public bool isAHideRequest { get => this.skill == null; }

    private SkillHitChanceRequest(Skill skill, float chance)
    {
        this.skill = skill;
        this.chance = chance;
    }

    public static SkillHitChanceRequest CreateForShow(Skill skill, float chance)
    {
        return new SkillHitChanceRequest(skill, chance);
    }

    private static SkillHitChanceRequest hideMsg = new SkillHitChanceRequest(null, 0);
    public static SkillHitChanceRequest CreateForHide()
    {
        return hideMsg;
    }
}