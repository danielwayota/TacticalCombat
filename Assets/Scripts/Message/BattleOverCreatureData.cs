public class BattleOverCreatureData
{
    public Creature creature { get; protected set; }

    public CreatureData start { get; protected set; }
    public CreatureData final { get; protected set; }

    public int levelGain { get => this.final.level - this.start.level; }
    public int experienceGain { get => (this.final.experience - this.start.experience) + 100 * this.levelGain; }

    public BattleOverCreatureData(Creature creature, CreatureData start, CreatureData final)
    {
        this.creature = creature;
        this.start = start;
        this.final = final;
    }
}