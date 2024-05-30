using System.Collections.Generic;
using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    public static AdventureManager current;

    public AdventureLevel[] levels;

    private int currentLevelIndex = 0;
    private AdventureLevel CurrentLevel { get => this.levels[this.currentLevelIndex]; }

    private List<AdventureMapNode> currentLevelMapNodes;

    [Header("Nodes")]
    public GameObject treasureNodePrfb;
    public GameObject battleNodePrfb;
    public GameObject bossNodePrfb;

    public void Awake()
    {
        current = this;
    }

    public void Start()
    {
        this.currentLevelMapNodes = new List<AdventureMapNode>();

        this.currentLevelIndex = 0;
        this.GenerateCurrentLevelMap();
    }

    protected void GenerateCurrentLevelMap()
    {
        float padding = 1.6f;

        int mapWidth = this.CurrentLevel.mapNodeWidth;
        int mapHeight = this.CurrentLevel.mapNodeHeight;

        AdventureMapNode[,] map = new AdventureMapNode[mapWidth, mapHeight];

        for (int i = 0; i < mapWidth; i++)
        {
            if (i % 2 != 0) continue;

            Vector3 position = Vector3.right * padding * i;
            AdventureMapNode newNode = this.PlaceBattleNode(position);

            newNode.AllowVisit();
            map[i, 0] = newNode;
        }

        // Interconexiones
        for (int y = 0; y < mapHeight - 1; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                var currentNode = map[x, y];
                if (currentNode == null) continue;

                int branchCount = 1;
                float dice = Random.Range(0f, 1f);

                if (dice < this.CurrentLevel.extraBranchChance)
                {
                    branchCount = 2;
                }

                for (int z = 0; z < branchCount; z++)
                {
                    int indexToConnect = Mathf.Clamp(Random.Range(x - 1, x + 2), 0, mapWidth - 1);

                    AdventureMapNode nodeToConnect = map[indexToConnect, y + 1];
                    if (nodeToConnect == null)
                    {
                        var position = new Vector3(padding * indexToConnect, padding * (y + 1), 0);

                        if (y != (mapHeight / 2) - 1)
                        {
                            nodeToConnect = this.PlaceBattleNode(position);
                        }
                        else
                        {
                            nodeToConnect = this.PlaceTreasureNode(position);
                        }

                        map[indexToConnect, y + 1] = nodeToConnect;
                    }

                    if (currentNode.forwardConnections.Contains(nodeToConnect) == false)
                    {
                        currentNode.forwardConnections.Add(nodeToConnect);
                    }
                }
            }
        }

        // Nodo del jefe
        var bossPos = new Vector3(mapWidth / 2f, (mapHeight) * padding, 0);
        AdventureMapNode bossNode = this.PlaceBossNode(bossPos);

        for (int x = 0; x < mapWidth; x++)
        {
            int y = mapHeight - 1;
            var currentNode = map[x, y];
            if (currentNode == null) continue;

            currentNode.forwardConnections.Add(bossNode);
        }
    }

    private AdventureMapNode MakeNode(GameObject prefab, Vector3 position)
    {
        float vibration = 0.25f;
        Vector3 offset = new Vector3(
            Random.Range(-vibration, vibration),
            Random.Range(-vibration, vibration),
            0
        );

        GameObject nodeGO = Instantiate(prefab, position + offset, Quaternion.identity);
        nodeGO.transform.parent = this.transform;

        AdventureMapNode node = nodeGO.GetComponent<AdventureMapNode>();

        this.currentLevelMapNodes.Add(node);

        return node;
    }

    private AdventureTreasureNode PlaceTreasureNode(Vector3 position)
    {
        AdventureTreasureNode node = this.MakeNode(this.treasureNodePrfb, position) as AdventureTreasureNode;

        int itemIndex = Random.Range(0, this.CurrentLevel.posibleTreasures.Length);
        node.Configure(this.CurrentLevel.posibleTreasures[itemIndex]);

        return node;
    }

    private AdventureBattleNode PlaceBattleNode(Vector3 position)
    {
        AdventureBattleNode node = this.MakeNode(this.battleNodePrfb, position) as AdventureBattleNode;

        int enemyGroupIndex = Random.Range(0, this.CurrentLevel.posibleEnemyGroups.Length);
        BattleEnemyGroup group = this.CurrentLevel.posibleEnemyGroups[enemyGroupIndex];

        int mapIndex = Random.Range(0, this.CurrentLevel.posibleMapPrfbs.Length);
        GameObject map = this.CurrentLevel.posibleMapPrfbs[mapIndex];

        node.Configure(group, map);
        return node;
    }

    private AdventureBossNode PlaceBossNode(Vector3 position)
    {
        AdventureBossNode node = this.MakeNode(this.bossNodePrfb, position) as AdventureBossNode;

        node.Configure(this.CurrentLevel.bossGroup, this.CurrentLevel.bossMapPrfb);
        return node;
    }

    public void OnNodeVisited(AdventureMapNode node)
    {
        foreach (var cnode in this.currentLevelMapNodes)
        {
            if (cnode.canBeVisited && !cnode.hasBeenVisited)
            {
                cnode.DenyVisit();
            }
        }

        foreach (var next in node.forwardConnections)
        {
            next.AllowVisit();
        }

    }

    public void OnLevelFinished()
    {
        foreach (var node in this.currentLevelMapNodes)
        {
            Destroy(node.gameObject);
        }
        this.currentLevelMapNodes.Clear();

        this.currentLevelIndex++;
        if (this.currentLevelIndex >= this.levels.Length)
        {
            this.currentLevelIndex = 0;
        }

        this.GenerateCurrentLevelMap();
    }
}