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
