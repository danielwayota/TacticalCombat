using UnityEngine;

public class BattleArea : MonoBehaviour
{
    public TextAsset mapData;

    public GameObject[] aiCreaturesPfbs;

    private float coolDownTime = 0;

    void Update()
    {
        if (this.coolDownTime > 0)
        {
            this.coolDownTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (this.coolDownTime > 0)
        {
            // Está desactivado.
            return;
        }

        // Desactivamos durante 1 segundo.
        this.coolDownTime = 1f;
        OverworldManager.current.StartBattle(this.mapData.text, this.aiCreaturesPfbs);
    }
}