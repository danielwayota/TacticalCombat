using System.Collections.Generic;

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager current;

    private Master[] masters;
    private int turnIndex;

    public MapManager mapManager { get; protected set; }

    protected List<Creature> gameCreatures;
    protected List<Creature> graveyard;

    protected bool isBattleOver;

    protected List<Creature> returnBuffer;

    void Start()
    {
        current = this;

        this.gameCreatures = new List<Creature>();
        this.graveyard = new List<Creature>();

        this.returnBuffer = new List<Creature>();

        this.mapManager = GetComponent<MapManager>();
        this.mapManager.Configure();

        Master human = this.GetComponentInChildren<HumanMaster>();
        Master ai = this.GetComponentInChildren<AIMaster>();

        this.masters = new Master[] { human, ai };

        human.SpawnCreatures(this.mapManager.humanSpawnPoints);
        ai.SpawnCreatures(this.mapManager.aiSpawnPoints);

        this.turnIndex = -1;
        this.isBattleOver = false;

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

    public void OnCreatureDeath(Creature creature)
    {
        this.gameCreatures.Remove(creature);
        creature.gameObject.SetActive(false);

        this.graveyard.Add(creature);

        this.CheckForBattleOver();
    }

    public void NextTurn()
    {
        if (this.isBattleOver)
        {
            return;
        }

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

    protected void CheckForBattleOver()
    {
        int creatureCount = this.gameCreatures.Count;
        if (creatureCount == 0)
        {
            Debug.LogWarning("Empate!!  ??");
            this.isBattleOver = true;
        }

        Master human = this.masters[0];
        Master ai = this.masters[1];

        if (human.HasAliveCreatures() && ai.HasAliveCreatures() == false)
        {
            this.isBattleOver = true;
            MessageManager.current.Send(new BattleOverMessage(human, ai));
        }

        if (ai.HasAliveCreatures() && human.HasAliveCreatures() == false)
        {
            this.isBattleOver = true;
            MessageManager.current.Send(new BattleOverMessage(ai, human));
        }
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

    public List<Creature> GetEnemyCreaturesInArea(List<Vector3> area, Master currentMaster)
    {
        this.returnBuffer.Clear();

        foreach (var point in area)
        {
            Creature posibleCreature = this.GetCreatureAtPosition(point);
            if (posibleCreature == null)
            {
                continue;
            }

            if (posibleCreature.master == currentMaster)
            {
                continue;
            }

            this.returnBuffer.Add(posibleCreature);
        }

        return this.returnBuffer;
    }

    public bool CanMoveCreatureTo(Creature creature, Vector3 worldTarget)
    {
        Vector3 targetPos = this.mapManager.SnapToTile(worldTarget);

        if (this.IsOwnerOnTurn(creature) == false)
        {
            Debug.LogWarning("Cannot move this creature.");
            return false;
        }

        if (GameManager.current.mapManager.IsAGroundTile(targetPos) == false)
        {
            Debug.LogWarning("Not a ground tile.");
            return false;
        }

        Creature creatureAtLocation = this.GetCreatureAtPosition(targetPos);
        if (creatureAtLocation != null)
        {
            Debug.Log("Tile already occupied.");
            return false;
        }

        return true;
    }

    public void MoveCreatureTo(Creature creature, Vector3 worldTarget)
    {
        if (this.isBattleOver)
        {
            return;
        }

        Vector3 targetPos = this.mapManager.SnapToTile(worldTarget);

        if (this.CanMoveCreatureTo(creature, worldTarget) == false)
        {
            return;
        }

        List<Vector3> path = this.mapManager.PredictWorldPathFor(creature.transform.position, targetPos);
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

    public void TryToPerformSkillAtPoint(Creature emitter, Skill skill, Vector3 point)
    {
        Vector3 targetPos = this.mapManager.SnapToTile(point);

        List<Vector3> reachArea = this.mapManager.PredictAreaFor(
            emitter.transform.position,
            skill.range
        );

        bool isInArea = false;
        foreach (var pos in reachArea)
        {
            if (pos == targetPos)
            {
                isInArea = true;
                break;
            }
        }

        if (isInArea == false)
        {
            Debug.Log("Can't attack. Target is not in range.");
            return;
        }

        List<Vector3> effectArea = this.mapManager.PredictAreaFor(
            targetPos,
            skill.area
        );

        this.TryToPerformSkillInArea(emitter, skill, effectArea);
    }

    public void TryToPerformSkillInArea(Creature emitter, Skill skill, List<Vector3> area)
    {
        if (this.isBattleOver)
        {
            return;
        }

        if (this.IsOwnerOnTurn(emitter) == false)
        {
            Debug.LogWarning("It's not your turn!");
            return;
        }

        if (emitter.CanExecuteSkill(skill) == false)
        {
            Debug.LogWarning("Can't execute skill. No energy.");
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
