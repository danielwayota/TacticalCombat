public class SkillDamageMessage : Message
{
    public override MessageTag tag => MessageTag.SKILL_DAMAGE;

    public Skill skill { get; protected set; }
    public Creature receiver { get; protected set; }

    public int damage { get; protected set; }
    public bool critical { get; protected set; }

    public SkillDamageMessage(Skill skill, Creature receiver, int damage, bool crit)
    {
        this.skill = skill;
        this.receiver = receiver;
        this.damage = damage;
        this.critical = crit;
    }

    public override string ToString()
    {
        string critText = this.critical ? "CRIT" : "";
        return $"{this.skill.skillName} to {this.receiver.name} with {this.damage} {critText}";
    }
}