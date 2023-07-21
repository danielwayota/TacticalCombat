using UnityEngine;

[CreateAssetMenu(fileName = "Creature profile", menuName = "Creatures/Profile", order = 0)]
public class CreatureProfile : ScriptableObject
{
    public CreatureData baseData;

    [Header("Growth profile")]
    public Vector2 maxhp = Vector2.zero;
    public Vector2 attack = Vector2.zero;
    public Vector2 defense = Vector2.zero;
    public Vector2 accuracy = Vector2.zero;
    public Vector2 evasion = Vector2.zero;
    public Vector2 elemAttack = Vector2.zero;
    public Vector2 elemDefense = Vector2.zero;
    public Vector2 speed = Vector2.zero;

    public CreatureData GenerateDataForLevel(int targetLevel)
    {
        CreatureData data = this.baseData.Clone();

        while (data.stats.level < targetLevel)
        {
            this.LevelUp(data);
        }

        data.SetParentProfile(this);

        return data;
    }

    protected void LevelUp(CreatureData data)
    {
        ShadowStats shadow = data.stats.GetShadow();

        shadow.maxhp += Random.Range(this.maxhp.x, this.maxhp.y);
        shadow.attack += Random.Range(this.attack.x, this.attack.y);
        shadow.defense += Random.Range(this.defense.x, this.defense.y);
        shadow.accuracy += Random.Range(this.accuracy.x, this.accuracy.y);
        shadow.evasion += Random.Range(this.evasion.x, this.evasion.y);
        shadow.elemAttack += Random.Range(this.elemAttack.x, this.elemAttack.y);
        shadow.elemDefense += Random.Range(this.elemDefense.x, this.elemDefense.y);
        shadow.speed += Random.Range(this.speed.x, this.speed.y);

        if (shadow.experience >= 100)
        {
            shadow.experience -= 100;
        }

        shadow.level++;

        data.stats.ApplyShadow(shadow);
    }

    public CreatureData LevelUpIfItShould(CreatureData data)
    {
        CreatureData result = data.Clone();

        while (result.experience > 100)
        {
            this.LevelUp(result);
        }

        return result;
    }
}