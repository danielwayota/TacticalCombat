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

    private Creature selectedCreature;

    // Marcadores de ruta.
    public GameObject pathMarkerPrfb;
    private List<MapPathMarker> pathMarkers = new List<MapPathMarker>();

    public Transform pathMarkerHolder;

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
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
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
                this.selectedCreature != null &&
                GameManager.current.IsOwnerOnTurn(this.selectedCreature)
            )
            {
                List<Vector3> path = GameManager.current.mapManager.PredictWorldPathFor(
                    this.selectedCreature.transform.position, world
                );

                this.HideAllPathMarkers();
                this.DisplayPredictedPath(path);
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            this.HideAllPathMarkers();

            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);

            if (this.selectedCreature != null)
            {
                this.selectedCreature.SetSelectionStatus(false);
            }

            this.selectedCreature = GameManager.current.GetCreatureAtPosition(world);
            if (this.selectedCreature != null)
            {
                this.selectedCreature.SetSelectionStatus(true);
            }
        }

        if (Input.GetButtonDown("Fire2") && this.selectedCreature != null)
        {
            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);

            this.HideAllPathMarkers();
            GameManager.current.MoveCreatureTo(this.selectedCreature, world);
        }
    }

    private void DisplayPredictedPath(List<Vector3> path)
    {
        int mathMaxSteps = Mathf.Min(this.selectedCreature.CurrentMaxDistance(), path.Count);

        for (int i = 0; i < mathMaxSteps; i++)
        {
            MapPathMarker marker = this.GetMarkerByIndex(i);

            int cost = this.selectedCreature.GetEnergyCostForPathLength(i + 1);
            marker.SetColourUsingPathCost(cost);

            marker.transform.position = path[i];
        }
    }

    private void HideAllPathMarkers()
    {
        foreach (var marker in this.pathMarkers)
        {
            marker.Hide();
        }
    }

    public MapPathMarker GetMarkerByIndex(int index)
    {
        if (this.pathMarkers.Count > index)
        {
            MapPathMarker marker = this.pathMarkers[index];

            return marker;
        }

        GameObject go = Instantiate(this.pathMarkerPrfb);
        MapPathMarker newMarker = go.GetComponent<MapPathMarker>();
        this.pathMarkers.Add(newMarker);

        newMarker.transform.SetParent(this.pathMarkerHolder);

        return newMarker;
    }
}