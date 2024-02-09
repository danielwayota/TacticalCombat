using UnityEngine;

public class AdventureManager : MonoBehaviour
{
    public static AdventureManager current;

    public BattleEnemyGroup[] posibleEnemyGroups;
    public GameObject[] posibleMapPrfbs;

    public ItemStack[] posibleTreasures;

    [Header("Boss")]
    public BattleEnemyGroup bossGroup;
    public GameObject bossMapPrfb;

    [Header("Nodes")]
    public GameObject treasureNodePrfb;
    public GameObject battleNodePrfb;
    public GameObject bossNodePrfb;

    [Header("Node count")]
    public int minNodes = 0;
    public int maxNodes = 0;

    private int currentNodeIndex = 0;
    private int currentMapSize = 0;

    private AdventureMapNode[] mapNodes;

    private AdventureMapNode currentNode { get => this.mapNodes[this.currentNodeIndex]; }

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        this.GenerateMap();
    }

    protected void GenerateMap()
    {
        float padding = 1.4f;

        this.currentMapSize = Random.Range(this.minNodes, this.maxNodes);
        this.mapNodes = new AdventureMapNode[this.currentMapSize];

        Vector3 position = Vector3.left * padding * (this.currentMapSize / 2f);

        float treasureNodeChance = 0.05f;
        bool treasureNodeSpawned = false;

        for (int i = 0; i < this.currentMapSize - 1; i++)
        {
            if (treasureNodeSpawned == false && this.posibleTreasures.Length != 0)
            {
                float dice = Random.Range(0f, 1f);
                if (dice < treasureNodeChance)
                {
                    this.mapNodes[i] = this.PlaceTreasureNode(position);
                    treasureNodeSpawned = true;
                    position += Vector3.right * padding;
                    continue;
                }

                // No creamos nodo de item, incrementamos probabilidad.
                treasureNodeChance += 0.05f;
            }

            this.mapNodes[i] = this.PlaceBattleNode(position);
            position += Vector3.right * padding;
        }

        // El nodo del jefe
        int lastNodeIndex = this.currentMapSize - 1;
        this.mapNodes[lastNodeIndex] = this.PlaceBossNode(position);

        this.currentNodeIndex = 0;
        this.currentNode.AllowVisit();
    }

    private AdventureMapNode PlaceTreasureNode(Vector3 position)
    {
        GameObject nodeGO = Instantiate(this.treasureNodePrfb, position, Quaternion.identity);
        nodeGO.transform.parent = this.transform;

        AdventureTreasureNode node = nodeGO.GetComponent<AdventureTreasureNode>();

        int itemIndex = Random.Range(0, this.posibleTreasures.Length);
        node.Configure(this.posibleTreasures[itemIndex]);

        return node;
    }

    private AdventureMapNode PlaceBattleNode(Vector3 position)
    {
        GameObject nodeGO = Instantiate(this.battleNodePrfb, position, Quaternion.identity);
        nodeGO.transform.parent = this.transform;

        AdventureBattleNode node = nodeGO.GetComponent<AdventureBattleNode>();

        int enemyGroupIndex = Random.Range(0, this.posibleEnemyGroups.Length);
        BattleEnemyGroup group = this.posibleEnemyGroups[enemyGroupIndex];

        int mapIndex = Random.Range(0, this.posibleMapPrfbs.Length);
        GameObject map = this.posibleMapPrfbs[mapIndex];

        node.Configure(group, map);
        return node;
    }

    private AdventureMapNode PlaceBossNode(Vector3 position)
    {
        GameObject nodeGO = Instantiate(this.bossNodePrfb, position, Quaternion.identity);
        nodeGO.transform.parent = this.transform;

        AdventureBossNode node = nodeGO.GetComponent<AdventureBossNode>();

        node.Configure(this.bossGroup, this.bossMapPrfb);
        return node;
    }

    public void OnNodeVisited(AdventureMapNode node)
    {
        if (node == this.currentNode)
        {
            this.currentNodeIndex++;
        }

        if (this.currentNodeIndex < this.currentMapSize)
        {
            this.currentNode.AllowVisit();
        }
    }
}