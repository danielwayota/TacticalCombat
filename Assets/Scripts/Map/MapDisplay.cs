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

    private Map map;
    private MapPathFinder pathFinder;

    private Creature selectedCreature;
    private List<Creature> creatures;

    // Marcadores de ruta
    public GameObject pathMarkerPrfb;
    private List<GameObject> pathMarkers;

    public Transform pathMarkerHolder;

    void Awake()
    {
        this.pathMarkers = new List<GameObject>();
    }

    public void RenderMapData(Map mapdata)
    {
        this.map = mapdata;

        this.pathFinder = new MapPathFinder();
        this.pathFinder.ConfigureForMap(this.map);

        for (int x = 0; x < this.map.width; x++)
        {
            for (int y = 0; y < this.map.height; y++)
            {
                TileType type = this.map.GetTileType(x, y);

                Tile tile = this.GetTileForType(type);

                this.targetTilemap.SetTile(new Vector3Int(x, -y, 0), tile);
            }
        }
    }

    public void EmplaceCreature(Creature creature, Vector2Int localPosition)
    {
        if (this.creatures == null)
        {
            this.creatures = new List<Creature>();
        }

        Vector3 worldPosition = this.LocalToWorld(localPosition);

        creature.localPosition = localPosition;
        creature.transform.position = worldPosition;

        this.creatures.Add(creature);
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
            var local = this.WorldToLocal(world);

            if (this.map.GetTileType(local.x, local.y) == TileType.GROUND)
            {
                this.cursor.gameObject.SetActive(true);
                this.cursor.position = this.LocalToWorld(local);
            }
            else
            {
                this.cursor.gameObject.SetActive(false);
            }

            if (this.selectedCreature != null)
            {
                Vector2Int start = this.selectedCreature.localPosition;
                List<Vector2Int> path = this.pathFinder.GetPath(start.x, start.y, local.x, local.y);

                this.HideAllPathMarkers();

                for (int i = 0; i < path.Count; i++)
                {
                    Vector3 worldPoint = this.LocalToWorld(path[i]);
                    GameObject marker = this.GetMarkerByIndex(i);

                    marker.transform.position = worldPoint;
                }
            }
        }

        if (Input.GetButtonDown("Fire1"))
        {
            this.HideAllPathMarkers();

            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int local = this.WorldToLocal(world);

            if (this.selectedCreature != null)
            {
                this.selectedCreature.SetSelectionStatus(false);
            }

            this.selectedCreature = this.GetCreatureAtPosition(local);
            if (this.selectedCreature != null)
            {
                this.selectedCreature.SetSelectionStatus(true);
            }
        }

        if (Input.GetButtonDown("Fire2") && this.selectedCreature != null)
        {
            Vector3 world = this.gameCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2Int localTarget = this.WorldToLocal(world);

            Vector2Int localStart = this.selectedCreature.localPosition;
            List<Vector2Int> path = this.pathFinder.GetPath(localStart.x, localStart.y, localTarget.x, localTarget.y);

            if (path.Count != 0)
            {
                this.selectedCreature.localPosition = localTarget;

                Vector3[] worldPath = new Vector3[path.Count];
                for (int i = 0; i < path.Count; i++)
                {
                    worldPath[i] = this.LocalToWorld(path[i]);
                }

                this.selectedCreature.FollowPath(worldPath);
            }

        }
    }

    private void HideAllPathMarkers()
    {
        foreach (var marker in this.pathMarkers)
        {
            marker.SetActive(false);
        }
    }

    public GameObject GetMarkerByIndex(int index)
    {
        if (this.pathMarkers.Count > index)
        {
            GameObject marker = this.pathMarkers[index];

            marker.SetActive(true);
            return marker;
        }

        GameObject newMarker = Instantiate(this.pathMarkerPrfb);
        this.pathMarkers.Add(newMarker);

        newMarker.transform.SetParent(this.pathMarkerHolder);

        return newMarker;
    }

    private Creature GetCreatureAtPosition(Vector2Int localPosition)
    {
        foreach (var creature in this.creatures)
        {
            Vector2Int pos = creature.localPosition;
            if (pos.x == localPosition.x && pos.y == localPosition.y)
            {
                return creature;
            }
        }

        return null;
    }

    private Vector2Int WorldToLocal(Vector3 world)
    {
        Vector3 local = world - this.transform.position;

        int mapX = Mathf.FloorToInt(local.x);
        int mapY = Mathf.FloorToInt(local.y);

        return new Vector2Int(mapX, -mapY);
    }

    private Vector3 LocalToWorld(Vector2Int local)
    {
        Vector3 localF = new Vector3(local.x, -local.y, 0);

        return this.transform.position + localF + (Vector3.one * 0.5f);
    }
}