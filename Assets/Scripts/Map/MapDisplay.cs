using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public struct MapTilePair
{
    public TileType type;
    public Tile visualTile;
}

public class MapDisplay : MonoBehaviour
{
    public MapTilePair[] mapTilePairs;
    public Tilemap targetTilemap;

    public Camera gameCamera;
    public Transform cursor;

    // Marcadores de ruta.
    public GameObject pathMarkerPrfb;
    private List<MapPathMarker> pathMarkers = new List<MapPathMarker>();

    public Transform pathMarkerHolder;

    private HumanMaster humanMaster;

    void Awake()
    {
        this.humanMaster = FindObjectOfType<HumanMaster>();
    }

    public void RenderMapData(Map mapdata)
    {
        for (int x = 0; x < mapdata.width; x++)
        {
            for (int y = 0; y < mapdata.height; y++)
            {
                TileType type = mapdata.GetTileType(x, y);

                Tile tile = this.GetTileForType(type);

                this.targetTilemap.SetTile(new Vector3Int(x, -y, 0), tile);
            }
        }
    }

    private Tile GetTileForType(TileType type)
    {
        foreach (var pair in this.mapTilePairs)
        {
            if (pair.type == type)
                return pair.visualTile;
        }

        Debug.LogError("No hay tile para: " + type);
        return null;
    }

    void Update()
    {
        if (InputManager.GetIfMouseHasMoved())
        {
            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);

            if (GameManager.current.mapManager.IsAGroundTile(world))
            {
                this.cursor.gameObject.SetActive(true);
                this.cursor.position = GameManager.current.mapManager.SnapToTile(world);
            }
            else
            {
                this.cursor.gameObject.SetActive(false);
            }

            if (
                this.humanMaster.hasCreatureSelected &&
                GameManager.current.IsOwnerOnTurn(this.humanMaster.selectedCreature)
            )
            {
                this.HideAllPathMarkers();

                switch (this.humanMaster.status)
                {
                    case HumanCombatStatus.MOVE:
                        List<Vector3> path = GameManager.current.mapManager.PredictWorldPathFor(
                            this.humanMaster.selectedCreature.transform.position, world
                        );

                        this.DisplayPredictedPath(path);
                        break;
                    case HumanCombatStatus.SKILL:
                        List<Vector3> reachArea = GameManager.current.mapManager.PredictAreaFor(
                            this.humanMaster.selectedCreature.transform.position,
                            this.humanMaster.selectedSkill.range
                        );

                        this.DisplayPredictedArea(reachArea);

                        if (GameManager.current.mapManager.IsInsideArea(reachArea, world))
                        {
                            List<Vector3> skillEffectArea = GameManager.current.mapManager.PredictAreaFor(
                                world,
                                this.humanMaster.selectedSkill.area
                            );

                            this.DisplayPredictedArea(skillEffectArea, 2);
                        }

                        break;
                }

            }
        }

        if (InputManager.GetLeftClickDown())
        {
            this.HideAllPathMarkers();

            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
            this.humanMaster.OnSelectionRequested(world);
        }

        if (InputManager.GetRightClickDown() && this.humanMaster.hasCreatureSelected)
        {
            this.HideAllPathMarkers();

            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
            this.humanMaster.OnMoveOrSkillRequested(world);
        }
    }

    private void DisplayPredictedPath(List<Vector3> path)
    {
        Creature selected = this.humanMaster.selectedCreature;

        int mathMaxSteps = Mathf.Min(selected.CurrentMaxDistance(), path.Count);

        for (int i = 0; i < mathMaxSteps; i++)
        {
            MapPathMarker marker = this.GetNextMarker();

            int cost = selected.GetEnergyCostForPathLength(i + 1);
            marker.SetColourUsingPathCost(cost);

            marker.transform.position = path[i];
        }
    }

    private void DisplayPredictedArea(List<Vector3> area, int pseudoCost = 1)
    {
        for (int i = 0; i < area.Count; i++)
        {
            MapPathMarker marker = this.GetNextMarker();

            marker.SetColourUsingPathCost(pseudoCost);

            marker.transform.position = area[i];
        }
    }

    private void HideAllPathMarkers()
    {
        foreach (var marker in this.pathMarkers)
        {
            marker.Hide();
        }
    }

    public MapPathMarker GetNextMarker()
    {
        foreach (var marker in this.pathMarkers)
        {
            if (marker.visible == false)
            {
                return marker;
            }
        }

        GameObject go = Instantiate(this.pathMarkerPrfb);
        MapPathMarker newMarker = go.GetComponent<MapPathMarker>();
        this.pathMarkers.Add(newMarker);

        newMarker.transform.SetParent(this.pathMarkerHolder);

        return newMarker;
    }
}