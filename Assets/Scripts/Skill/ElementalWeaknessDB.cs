using System.Collections.Generic;

public static class ElementalWeaknessDB
{
    static Dictionary<(ElementalType, ElementalType), float> db;

    static ElementalWeaknessDB()
    {
        db = new Dictionary<(ElementalType, ElementalType), float>();

        db.Add((ElementalType.FIRE, ElementalType.GRASS), 1.5f);
        db.Add((ElementalType.FIRE, ElementalType.WATER), 0.5f);

        db.Add((ElementalType.WATER, ElementalType.FIRE), 1.5f);
        db.Add((ElementalType.WATER, ElementalType.GROUND), 1.5f);

        db.Add((ElementalType.GROUND, ElementalType.FIRE), 1.5f);
        db.Add((ElementalType.GROUND, ElementalType.WATER), 0.5f);

        db.Add((ElementalType.GRASS, ElementalType.WATER), 1.5f);
        db.Add((ElementalType.GRASS, ElementalType.GROUND), 1.5f);
        db.Add((ElementalType.GRASS, ElementalType.FIRE), 0.5f);
    }

    public static float GetWeaknessMultiplier(ElementalType skillType, ElementalType receiverType)
    {
        (ElementalType, ElementalType) pair = (skillType, receiverType);

        if (db.ContainsKey(pair))
        {
            return db[pair];
        }

        // Valor por defecto
        return 1f;
    }
}
