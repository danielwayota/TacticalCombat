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
    private List<MapActionMarker> pathMarkers = new List<MapActionMarker>();

    public Transform pathMarkerHolder;

    private HumanMaster humanMaster;

    private Creature creatureUnderCursor = null;

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

        this.gameCamera.transform.position = new Vector3(
            this.transform.position.x + (mapdata.width / 2f),
            this.transform.position.y - (mapdata.height / 2f - 1),
            this.gameCamera.transform.position.z
        );

        this.gameCamera.GetComponent<CameraMove>().SetZoom(mapdata.width / 3f);
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
            world = BattleManager.current.mapManager.SnapToTile(world);

            if (BattleManager.current.mapManager.IsAGroundTile(world))
            {
                this.cursor.gameObject.SetActive(true);
                this.cursor.position = world;
            }
            else
            {
                this.cursor.gameObject.SetActive(false);
            }

            if (
                this.humanMaster.hasCreatureSelected &&
                BattleManager.current.IsOwnerOnTurn(this.humanMaster.selectedCreature)
            )
            {
                this.HideAllPathMarkers();

                switch (this.humanMaster.status)
                {
                    case HumanCombatStatus.MOVE:
                        List<Vector3> path = BattleManager.current.mapManager.PredictWorldPathFor(
                            this.humanMaster.selectedCreature.transform.position, world
                        );

                        this.DisplayPredictedPath(path);
                        break;
                    case HumanCombatStatus.SKILL:
                        List<Vector3> reachArea = BattleManager.current.mapManager.PredictAreaFor(
                            this.humanMaster.selectedCreature.transform.position,
                            this.humanMaster.selectedSkill.range
                        );

                        this.DisplayPredictedArea(reachArea);

                        bool targetInReachArea = BattleManager.current.mapManager.IsInsideArea(reachArea, world);
                        if (targetInReachArea)
                        {
                            List<Vector3> skillEffectArea = BattleManager.current.mapManager.PredictAreaFor(
                                world,
                                this.humanMaster.selectedSkill.area
                            );

                            this.DisplayPredictedArea(skillEffectArea, true);
                        }

                        Creature posibleTargetCreature = BattleManager.current.GetCreatureAtPosition(world);

                        // Si está fuera de alcance, no mostramos la probabilidad de acierto.
                        if (!targetInReachArea)
                        {
                            posibleTargetCreature = null;
                        }

                        if (this.creatureUnderCursor != posibleTargetCreature)
                        {
                            this.creatureUnderCursor = posibleTargetCreature;
                            this.humanMaster.RequestSkillHitChance(this.creatureUnderCursor);
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
            MapActionMarker marker = this.GetNextMarker();

            int cost = selected.GetEnergyCostForPathLength(i + 1);
            marker.ShowForPathUsingCost(cost);

            marker.transform.position = path[i];
        }
    }

    private void DisplayPredictedArea(List<Vector3> area, bool isAction = false)
    {
        for (int i = 0; i < area.Count; i++)
        {
            MapActionMarker marker = this.GetNextMarker();

            if (isAction)
            {
                marker.ShowForSkillAction();
            }
            else
            {
                marker.ShowForSkillReach();
            }

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

    public MapActionMarker GetNextMarker()
    {
        foreach (var marker in this.pathMarkers)
        {
            if (marker.visible == false)
            {
                return marker;
            }
        }

        GameObject go = Instantiate(this.pathMarkerPrfb);
        MapActionMarker newMarker = go.GetComponent<MapActionMarker>();
        this.pathMarkers.Add(newMarker);

        newMarker.transform.SetParent(this.pathMarkerHolder);

        return newMarker;
    }
}