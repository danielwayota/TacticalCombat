using UnityEngine;

[System.Serializable]
public class Stats
{
    public ElementalType elementalType;

    public int level = 1;

    [Header("HP/Energy")]
    public int hp = 100;
    public int maxhp = 100;

    public int energy = 2;
    public int maxEnergy = 2;

    [Header("Combat")]
    public int attack = 1;
    public int defense = 1;

    public int accuracy = 1;
    public int evasion = 1;

    public int elemAttack = 1;
    public int elemDefense = 1;

    [Header("Movement")]
    public int speed = 4;

    private ShadowStats shadow;

    public void Restore()
    {
        if (this.hp <= 0)
        {
            this.hp = 1;
        }
    }

    public ShadowStats GetShadow()
    {
        if (this.shadow == null)
        {
            ShadowStats shadow = new ShadowStats();

            shadow.level = this.level;
            shadow.maxhp = this.maxhp;
            shadow.attack = this.attack;
            shadow.defense = this.defense;
            shadow.accuracy = this.accuracy;
            shadow.evasion = this.evasion;
            shadow.elemAttack = this.elemAttack;
            shadow.elemDefense = this.elemDefense;
            shadow.speed = this.speed;

            this.shadow = shadow;
        }

        return this.shadow;
    }

    public void SetShadow(ShadowStats newShadow)
    {
        this.level = newShadow.level;

        int newMaxHP = Mathf.FloorToInt(newShadow.maxhp);

        int healthDiff = newMaxHP - this.maxhp;
        this.maxhp = newMaxHP;
        this.hp = Mathf.Clamp(this.hp + healthDiff, 0, this.maxhp);

        this.attack = Mathf.FloorToInt(newShadow.attack);
        this.defense = Mathf.FloorToInt(newShadow.defense);
        this.accuracy = Mathf.FloorToInt(newShadow.accuracy);
        this.evasion = Mathf.FloorToInt(newShadow.evasion);
        this.elemAttack = Mathf.FloorToInt(newShadow.elemAttack);
        this.elemDefense = Mathf.FloorToInt(newShadow.elemDefense);
        this.speed = Mathf.FloorToInt(newShadow.speed);

        this.shadow = newShadow;
    }

    public Stats Clone()
    {
        Stats clone = new Stats();

        clone.elementalType = this.elementalType;
        clone.level = this.level;
        clone.hp = this.hp;
        clone.maxhp = this.maxhp;
        clone.energy = this.energy;
        clone.maxEnergy = this.maxEnergy;
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
