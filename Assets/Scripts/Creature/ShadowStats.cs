public class ShadowStats
{
    public int level = 1;

    public float maxhp = 100;

    public float attack = 1;
    public float defense = 1;

    public float accuracy = 1;
    public float evasion = 1;

    public float elemAttack = 1;
    public float elemDefense = 1;

    public float speed = 4;

    public ShadowStats Clone()
    {
        ShadowStats clone = new ShadowStats();

        clone.level = this.level;
        clone.maxhp = this.maxhp;
        clone.attack = this.attack;
        clone.defense = this.defense;
        clone.accuracy = this.accuracy;
        clone.evasion = this.evasion;
        clone.elemAttack = this.elemAttack;
        clone.elemDefense = this.elemDefense;
        clone.speed = this.speed;

        return clone;
    }
}
