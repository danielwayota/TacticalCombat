public class ShadowStats
{
    public int level;

    public float maxhp;

    public float attack;
    public float defense;

    public float accuracy;
    public float evasion;

    public float elemAttack;
    public float elemDefense;

    public float speed;

    public int experience;

    public float loyalty;

    public ShadowStats()
    {
        this.level = 0;
        this.maxhp = 0;
        this.attack = 0;
        this.defense = 0;
        this.accuracy = 0;
        this.evasion = 0;
        this.elemAttack = 0;
        this.elemDefense = 0;
        this.speed = 0;
        this.experience = 0;
        this.loyalty = 0;
    }

    public ShadowStats(Stats stats)
    {
        this.level = stats.level;
        this.maxhp = stats.maxhp;
        this.attack = stats.attack;
        this.defense = stats.defense;
        this.accuracy = stats.accuracy;
        this.evasion = stats.evasion;
        this.elemAttack = stats.elemAttack;
        this.elemDefense = stats.elemDefense;
        this.speed = stats.speed;
        this.experience = 0;
        this.loyalty = 0;
    }

    public ShadowStats(ShadowStats shadow)
    {
        this.level = shadow.level;
        this.maxhp = shadow.maxhp;
        this.attack = shadow.attack;
        this.defense = shadow.defense;
        this.accuracy = shadow.accuracy;
        this.evasion = shadow.evasion;
        this.elemAttack = shadow.elemAttack;
        this.elemDefense = shadow.elemDefense;
        this.speed = shadow.speed;
        this.experience = shadow.experience;
        this.loyalty = shadow.loyalty;
    }

    public void Sum(ShadowStats shadowDelta)
    {
        // El nivel no se suma, solo estadísticas
        // this.level += shadowDelta.level;
        this.maxhp += shadowDelta.maxhp;
        this.attack += shadowDelta.attack;
        this.defense += shadowDelta.defense;
        this.accuracy += shadowDelta.accuracy;
        this.evasion += shadowDelta.evasion;
        this.elemAttack += shadowDelta.elemAttack;
        this.elemDefense += shadowDelta.elemDefense;
        this.speed += shadowDelta.speed;
        this.experience += shadowDelta.experience;

        // La lealtad no se suma
        // this.loyalty
    }

    public ShadowStats Clone()
    {
        return new ShadowStats(this);
    }
}
