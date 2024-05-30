using UnityEngine;

[CreateAssetMenu(fileName = "Adventure Level", menuName = "Adventure/Level", order = 0)]
public class AdventureLevel : ScriptableObject
{
    public int mapNodeWidth = 5;
    public int mapNodeHeight = 5;

    public float extraBranchChance = 0.1f;

    [Header("Enemies")]
    public BattleEnemyGroup[] posibleEnemyGroups;
    public GameObject[] posibleMapPrfbs;

    [Header("Treasures")]
    public ItemStack[] posibleTreasures;

    [Header("Boss")]
    public BattleEnemyGroup bossGroup;
    public GameObject bossMapPrfb;
}