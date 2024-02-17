using UnityEngine;

[System.Serializable]
public struct BattleReward
{
    public Item item;
    public int minAmount;
    public int maxAmount;

    [Range(0f, 1f)]
    public float chance;
}