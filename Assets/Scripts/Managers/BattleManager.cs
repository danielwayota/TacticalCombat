using System.Collections.Generic;
using UnityEngine;

public enum BattleCategory
{
    RANDOM_ENCOUNTER,
    VS_MASTER,
    BOSS
}

public class BattleManager : MonoBehaviour, IMessageListener
{
    public static BattleManager current;

    private Master[] masters;
    private int turnIndex;

    public MapManager mapManager { get; protected set; }

    protected List<Creature> gameCreatures;
    protected List<Creature> graveyard;

    protected bool isBattleOver;
    public BattleCategory battleCategory { get; protected set; }

    public bool CanFleeBattle
    {
        get
        {
            if (this.battleCategory == BattleCategory.VS_MASTER)
            {
                return false;
            }

            if (this.battleCategory == BattleCategory.BOSS)
            {
                return false;
            }

            return true;
        }
    }

    protected List<Creature> returnBuffer;

    protected List<BattleOverCreatureData> creaturesBattleOverData = null;

    protected BattleReward[] posibleRewards;
    protected List<ItemStack> battleOverRewards = null;

    void Awake()
    {
        current = this;
    }

    public void StartBattle(
        string mapData,
        CreatureData[] humanCreatures,
        CreatureData[] aiCreatures,
        BattleReward[] posibleRewards,
        BattleCategory battleCategory
    )
    {
        this.gameCreatures = new List<Creature>();
        this.graveyard = new List<Creature>();

        this.returnBuffer = new List<Creature>();

        this.mapManager = GetComponent<MapManager>();
        this.mapManager.Configure(mapData);

        Master human = this.GetComponentInChildren<HumanMaster>();
        Master ai = this.GetComponentInChildren<AIMaster>();

        this.masters = new Master[] { human, ai };

        human.SpawnCreatures(this.mapManager.humanSpawnPoints, humanCreatures);
        ai.SpawnCreatures(this.mapManager.aiSpawnPoints, aiCreatures);

        this.turnIndex = -1;
        this.isBattleOver = false;
        this.battleCategory = battleCategory;

        this.posibleRewards = posibleRewards;

        Invoke("NextTurn", .5f);

        // NOTE: Esto depende del orden en el que se reciban los mensajes.
        //       Ahora mismo, el ExperienceManager se registra primero,
        //       por lo tanto recibe el mensaje primero.
        //       Si esto no fuera así, no arreglamos nada.
        MessageManager.current.AddListener(MessageTag.CREATURE_DEFEATED, this);
        MessageManager.current.AddListener(MessageTag.CREATURE_CAPTURED, this);
    }

    // Se ejecuta desde el BattleOverUI
    public void EndBattle()
    {
        List<CreatureData> finalCreatureData = new List<CreatureData>();

        foreach (var battleOverCreatureData in this.GetCreaturesFinalData())
        {
            finalCreatureData.Add(battleOverCreatureData.final);
        }

        OverworldManager.current.StoreResultingCreatureData(finalCreatureData.ToArray());

        List<ItemStack> rewards = this.GetBattleOverRewards();
        OverworldManager.current.StoreItemRewards(rewards.ToArray());

        OverworldManager.current.EndBattle();
    }

    // Se ejecuta desde el TurnsUI
    public void FleeBattle()
    {
        if (this.CanFleeBattle == false)
        {
            Debug.LogWarning("Cannot flee from a battle against another master");
            return;
        }

        this.isBattleOver = true;

        MessageManager.current.Send(
            BattleOverMessage.CreateForFlee(this.GetCreaturesFinalData())
        );
    }

    public void EmplaceCreature(Creature creature)
    {
        Vector3 worldPosition = creature.transform.position;
        if (this.mapManager.IsAGroundTile(worldPosition) == false)
        {
            throw new System.Exception("Invalid Creature emplacement!");
        }

        creature.transform.position = this.mapManager.SnapToTile(worldPosition);
        this.gameCreatures.Add(creature);
    }

    public void Receive(Message msg)
    {
        if (msg is CreatureDefeatedMessage)
        {
            CreatureDefeatedMessage cdm = msg as CreatureDefeatedMessage;
            Creature creature = cdm.receiver;

            this.gameCreatures.Remove(creature);
            creature.gameObject.SetActive(false);
            this.graveyard.Add(creature);

            // Quitamos el obstáculo dinámico del path finding.
            this.mapManager.dynamicObstacles.Remove(creature.transform.position);
        }

        if (msg is CreatureCapturedMessage)
        {
            CreatureCapturedMessage ccm = msg as CreatureCapturedMessage;

            // Quitamos el obstáculo dinámico del path finding.
            this.mapManager.dynamicObstacles.Remove(ccm.receiver.transform.position);
        }

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
            MessageManager.current.Send(BattleOverMessage.CreateForWin(
                human,
                this.GetCreaturesFinalData(),
                this.GetBattleOverRewards()
            ));

        }

        if (ai.HasAliveCreatures() && human.HasAliveCreatures() == false)
        {
            this.isBattleOver = true;
            MessageManager.current.Send(BattleOverMessage.CreateForLoss(ai));
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

        if (BattleManager.current.mapManager.IsAGroundTile(targetPos) == false)
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

    protected void TryToPerformSkillInArea(Creature emitter, Skill skill, List<Vector3> area)
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

        if (skill.isSpawner)
        {
            emitter.ConsumeEnergyFor(skill);
            skill.ResolveAsSpawner(emitter, area);
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
                skill.ResolveForReceiver(emitter, receiver);
            }
        }
    }

    public bool IsOwnerOnTurn(Creature creature)
    {
        Master currentMaster = this.masters[this.turnIndex];
        return creature.master == currentMaster;
    }

    public bool IsMasterOnTurn(Master master)
    {
        Master currentMaster = this.masters[this.turnIndex];
        return currentMaster == master;
    }

    private List<BattleOverCreatureData> GetCreaturesFinalData()
    {
        if (this.creaturesBattleOverData != null)
            return this.creaturesBattleOverData;

        this.creaturesBattleOverData = new List<BattleOverCreatureData>();

        HumanMaster human = this.masters[0] as HumanMaster;

        foreach (var creature in human.creatures)
        {
            if (creature.innerData.isMinion)
            {
                continue;
            }

            CreatureData startData = creature.innerData.Clone();

            ShadowStats shadowExp = ExperienceManager.current.GetEffortExpFor(creature);
            creature.innerData.AddExperience(shadowExp);

            CreatureProfile profile = creature.innerData.profile;
            CreatureData finalData = profile.LevelUpIfItShould(creature.innerData);

            // Aumentamos la lealtad al terminar una batalla
            finalData.stats.ModifyLoyalty(0.02f);
            // Aumentamos la lealtad por cada nivel adquirido
            int levelDiff = finalData.level - startData.level;
            finalData.stats.ModifyLoyalty(0.05f * levelDiff);

            BattleOverCreatureData battleOverCreatureData = new BattleOverCreatureData(
                creature, startData, finalData
            );

            this.creaturesBattleOverData.Add(battleOverCreatureData);
        }

        foreach (var deadCreature in this.graveyard)
        {
            if (deadCreature.innerData.isMinion)
            {
                continue;
            }

            if (!deadCreature.belongToHuman)
            {
                continue;
            }

            deadCreature.innerData.stats.ModifyLoyalty(-0.01f);

            BattleOverCreatureData battleOverCreatureData = new BattleOverCreatureData(
                deadCreature, deadCreature.innerData, deadCreature.innerData
            );

            this.creaturesBattleOverData.Add(battleOverCreatureData);
        }

        return this.creaturesBattleOverData;
    }

    private List<ItemStack> GetBattleOverRewards()
    {
        if (this.battleOverRewards != null)
            return this.battleOverRewards;

        this.battleOverRewards = new List<ItemStack>();

        foreach (var posibleReward in this.posibleRewards)
        {
            float dice = Random.Range(0f, 1f);
            if (dice > posibleReward.chance)
            {
                continue;
            }

            int amount = Random.Range(
                posibleReward.minAmount,
                posibleReward.maxAmount
            );

            if (amount == 0)
                continue;

            this.battleOverRewards.Add(
                new ItemStack(posibleReward.item, amount)
            );
        }

        return this.battleOverRewards;
    }
}
