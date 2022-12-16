using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private Master[] masters;
    private int turnIndex;

    public MapManager mapManager { get; protected set; }

    protected List<Creature> gameCreatures;

    void Start()
    {
        current = this;

        this.gameCreatures = new List<Creature>();

        this.mapManager = GetComponent<MapManager>();
        this.mapManager.Configure();

        Master human = this.GetComponentInChildren<HumanMaster>();
        Master ai = this.GetComponentInChildren<AIMaster>();

        this.masters = new Master[] { human, ai };

        human.SpawnCreatures(this.mapManager.humanSpawnPoints);
        ai.SpawnCreatures(this.mapManager.aiSpawnPoints);

        this.turnIndex = -1;

        Invoke("NextTurn", .5f);
    }

    public void EmplaceCreature(Creature creature, Vector3 worldPosition)
    {
        if (this.mapManager.IsAGroundTile(worldPosition) == false)
        {
            throw new System.Exception("Invalid Creature emplacement!");
        }

        creature.transform.position = this.mapManager.SnapToTile(worldPosition);
        this.gameCreatures.Add(creature);
    }

    public void NextTurn()
    {
        this.turnIndex = (this.turnIndex + 1) % this.masters.Length;

        Master currentMaster = this.masters[this.turnIndex];

        this.mapManager.dynamicObstacles.Clear();

        foreach (var creature in this.gameCreatures)
        {
            if (creature.master != currentMaster)
            {
                this.mapManager.dynamicObstacles.Add(creature.transform.position);
            }
        }

        MessageManager.current.Send(new NextTurnMessage(currentMaster));
        currentMaster.BeginTurn();
    }

    public Creature GetCreatureAtPosition(Vector3 worldPosition)
    {
        foreach (var creature in this.gameCreatures)
        {
            float distance = Vector2.Distance(worldPosition, creature.transform.position);

            if (distance < 1f)
            {
                return creature;
            }
        }

        return null;
    }

    public void MoveCreatureTo(Creature creature, Vector3 worldTarget)
    {
        if (this.IsOwnerOnTurn(creature) == false)
        {
            Debug.LogError("Cannot move this creature.");
            return;
        }

        Creature creatureAtLocation = this.GetCreatureAtPosition(worldTarget);
        if (creatureAtLocation != null)
        {
            Debug.Log("Tile already occupied.");
            return;
        }

        List<Vector3> path = this.mapManager.PredictWorldPathFor(creature.transform.position, worldTarget);
        creature.FollowPath(path.ToArray());
    }

    public bool ThereIsTargetInArea(List<Vector3> area)
    {
        foreach (var point in area)
        {
            Creature creature = this.GetCreatureAtPosition(point);
            if (creature != null)
                return true;
        }

        return false;
    }

    public void TryToPerformSkillInArea(Creature emitter, Skill skill, List<Vector3> area)
    {
        if (this.IsOwnerOnTurn(emitter) == false)
        {
            Debug.LogError("It's not your turn!");
            return;
        }

        if (emitter.CanExecuteSkill(skill) == false)
        {
            Debug.LogError("Can't execute skill. No energy.");
            return;
        }

        if (this.ThereIsTargetInArea(area) == false)
        {
            Debug.Log("There is no target to execute Skill");
            return;
        }

        emitter.ConsumeEnergyFor(skill);

        foreach (var point in area)
        {
            Creature receiver = this.GetCreatureAtPosition(point);
            if (receiver != null)
            {
                skill.Resolve(emitter, receiver);
            }
        }
    }

    public bool IsOwnerOnTurn(Creature creature)
    {
        Master currentMaster = this.masters[this.turnIndex];
        return creature.master == currentMaster;
    }
}
