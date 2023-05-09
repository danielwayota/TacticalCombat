using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class OverworldManager : MonoBehaviour
{
    public static OverworldManager current;

    public GameObject[] humanCreaturesPrfbs;

    public string overworldSceneName = "Overworld";
    public string battleSceneName = "Battle";

    void Awake()
    {
        current = this;
    }

    private IEnumerator LoadBattle(string mapData, GameObject[] aiCreaturePrfbs)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(this.battleSceneName, LoadSceneMode.Additive);

        while (operation.isDone == false)
        {
            yield return null;
        }

        Scene battleScene = SceneManager.GetSceneByName(this.battleSceneName);
        SceneManager.SetActiveScene(battleScene);

        BattleManager.current.StartBattle(mapData, this.humanCreaturesPrfbs, aiCreaturePrfbs);
        this.gameObject.SetActive(false);
    }

    public void StartBattle(string mapData, GameObject[] aiCreatures)
    {
        StartCoroutine(this.LoadBattle(mapData, aiCreatures));
    }

    public void EndBattle()
    {
        Scene overworldScene = SceneManager.GetSceneByName(this.overworldSceneName);
        SceneManager.SetActiveScene(overworldScene);

        SceneManager.UnloadSceneAsync(this.battleSceneName);
        this.gameObject.SetActive(true);
    }
}
