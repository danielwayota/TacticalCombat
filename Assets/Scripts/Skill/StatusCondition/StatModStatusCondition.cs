

public class StatModStatusCondition : StatusCondition
{
    public int attack = 0;
    public int defense = 0;
    public int accuracy = 0;
    public int evasion = 0;
    public int elemAttack = 0;
    public int elemDefense = 0;
    public int speed = 0;

    protected override void ExecuteOnTurnStart(Stats targetStats)
    { }

    protected override void ExecuteStatsModifiers(Stats targetStats)
    {
        targetStats.attack += this.attack;
        targetStats.defense += this.defense;
        targetStats.accuracy += this.accuracy;
        targetStats.evasion += this.evasion;
        targetStats.elemAttack += this.elemAttack;
        targetStats.elemDefense += this.elemDefense;
        targetStats.speed += this.speed;
    }
}