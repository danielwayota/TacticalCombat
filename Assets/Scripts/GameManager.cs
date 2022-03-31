using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextAsset mapData;

    void Start()
    {
        Map map = Map.CreateWithStringData(this.mapData.text);

        MapDisplay display = GameObject.FindObjectOfType<MapDisplay>();
        display.RenderMapData(map);
    }
}
