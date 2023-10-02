using System.Collections;

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

    [Header("Scene names")]
    public string overworldSceneName = "Overworld";
    public string battleSceneName = "Battle";

    private TeamUI teamUI;

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

        // NOTE: Esto es para el warning de que hay dos EventSystem activos.
        this.overworldEventSystem = EventSystem.current;
    }

    public void ToggleTeamView()
    {
        this.teamUI.ToggleDisplay(this.humanCreatures);
    }

    private IEnumerator LoadBattle(string mapData, CreatureData[] aiCreatures)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(this.battleSceneName, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            yield return null;
        }

        Scene battleScene = SceneManager.GetSceneByName(this.battleSceneName);
        SceneManager.SetActiveScene(battleScene);

        BattleManager.current.StartBattle(mapData, this.humanCreatures, aiCreatures);
        this.gameObject.SetActive(false);
    }

    public void StartBattle(string mapData, CreatureData[] aiCreatures)
    {
        this.overworldEventSystem.enabled = false;

        StartCoroutine(this.LoadBattle(mapData, aiCreatures));
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
}
