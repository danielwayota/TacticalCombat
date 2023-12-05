using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public static OverworldManager current;

    public CreatureProfile[] humanCreatureProfiles;
    private CreatureData[] humanCreatures;

    // Esto es temporal
    public int[] humanCreatureLevels;
    public List<Item> startUpItems = new List<Item>();

    [Header("Scene names")]
    public string overworldSceneName = "Overworld";
    public string battleSceneName = "Battle";

    private TeamUI teamUI;
    private InventoryUI inventoryUI;

    private List<ItemStack> inventory;

    private EventSystem overworldEventSystem;

    void Awake()
    {
        current = this;

        this.humanCreatures = new CreatureData[this.humanCreatureProfiles.Length];
        for (int i = 0; i < this.humanCreatureProfiles.Length; i++)
        {
            int targetLevel = this.humanCreatureLevels[i];
            this.humanCreatures[i] = this.humanCreatureProfiles[i].GenerateDataForLevel(targetLevel);
        }

        this.teamUI = GameObject.FindObjectOfType<TeamUI>(true);

        this.inventoryUI = GameObject.FindObjectOfType<InventoryUI>(true);

        this.inventory = new List<ItemStack>();
        foreach (var item in this.startUpItems)
        {
            this.AddItemToInventory(item);
        }

        // NOTE: Esto es para el warning de que hay dos EventSystem activos.
        this.overworldEventSystem = EventSystem.current;
    }

    public void ToggleTeamView()
    {
        if (this.inventoryUI.isVisible)
        {
            this.inventoryUI.Hide();
        }

        this.teamUI.ToggleDisplay(this.humanCreatures);
    }

    public void ToggleInventoryView()
    {
        if (this.teamUI.isVisible)
        {
            this.teamUI.Hide();
        }

        this.inventoryUI.ToggleDisplay(this.inventory, this.humanCreatures);
    }

    private IEnumerator LoadBattle(string mapData, CreatureData[] aiCreatures, BattleReward[] posibleRewards)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(this.battleSceneName, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            yield return null;
        }

        Scene battleScene = SceneManager.GetSceneByName(this.battleSceneName);
        SceneManager.SetActiveScene(battleScene);

        BattleManager.current.StartBattle(mapData, this.humanCreatures, aiCreatures, posibleRewards);
        this.gameObject.SetActive(false);
    }

    public void StartBattle(string mapData, CreatureData[] aiCreatures, BattleReward[] posibleRewards)
    {
        this.overworldEventSystem.enabled = false;

        StartCoroutine(this.LoadBattle(mapData, aiCreatures, posibleRewards));
    }

    public void EndBattle()
    {
        Scene overworldScene = SceneManager.GetSceneByName(this.overworldSceneName);
        SceneManager.SetActiveScene(overworldScene);

        SceneManager.UnloadSceneAsync(this.battleSceneName);
        this.gameObject.SetActive(true);

        this.overworldEventSystem.enabled = true;
    }

    public void StoreResultingCreatureData(CreatureData[] afterBattleData)
    {
        // NOTE: Las criaturas pueden venir en otro órden.
        foreach (var data in afterBattleData)
        {
            data.stats.Restore();
        }

        this.humanCreatures = afterBattleData;
    }

    public void StoreItemRewards(ItemStack[] itemRewards)
    {
        foreach (var stack in itemRewards)
        {
            this.AddItemToInventory(stack.item, stack.amount);
        }
    }

    public void AddItemToInventory(Item item, int amount = 1)
    {
        bool shouldAddNew = true;

        foreach (var itemStack in this.inventory)
        {
            if (itemStack.item == item && itemStack.hasSpace)
            {
                itemStack.amount += amount;
                shouldAddNew = false;
            }
        }

        if (shouldAddNew)
        {
            this.inventory.Add(new ItemStack(item, amount));
        }
    }
}
