using UnityEngine;

using System.Collections.Generic;

public enum HumanCombatStatus
{
    MOVE, SKILL
}

public class HumanMaster : Master, IMessageListener
{
    public HumanCombatStatus status { get; protected set; }

    public Creature selectedCreature { get; protected set; }
    public bool hasCreatureSelected
    {
        get => this.selectedCreature != null;
    }

    public Skill selectedSkill { get; protected set; }

    void Start()
    {
        MessageManager.current.AddListener(MessageTag.ACTION_CREATURE_MOVE, this);
        MessageManager.current.AddListener(MessageTag.ACTION_CREATURE_SKILL, this);
    }

    public override void BeginTurn()
    {
        this.GoToMoveMode();
        this.BeginTurnToAllCreatures();
    }

    public void OnSelectionRequested(Vector3 worldPos)
    {
        this.GoToMoveMode();

        Vector3 targetPos = GameManager.current.mapManager.SnapToTile(worldPos);

        if (this.hasCreatureSelected)
        {
            this.selectedCreature.SetSelectionStatus(false);
        }

        this.selectedCreature = GameManager.current.GetCreatureAtPosition(targetPos);
        if (this.hasCreatureSelected)
        {
            this.selectedCreature.SetSelectionStatus(true);
        }

        MessageManager.current.Send(new CreatureSelectedMessage(this.selectedCreature));
    }

    public void OnMoveOrSkillRequested(Vector3 worldPos)
    {
        Vector3 targetPos = GameManager.current.mapManager.SnapToTile(worldPos);

        switch (this.status)
        {
            case HumanCombatStatus.MOVE:
                GameManager.current.MoveCreatureTo(this.selectedCreature, targetPos);
                break;
            case HumanCombatStatus.SKILL:
                List<Vector3> area = GameManager.current.mapManager.PredictAreaFor(
                    this.selectedCreature.transform.position,
                    this.selectedSkill.range
                );

                bool isInArea = false;
                foreach (var point in area)
                {
                    if (point == targetPos)
                    {
                        isInArea = true;
                        break;
                    }
                }

                if (isInArea == false)
                {
                    Debug.LogError("Can't attack. Target is not in range.");
                    return;
                }

                Creature posibleTarget = GameManager.current.GetCreatureAtPosition(targetPos);

                if (posibleTarget == null)
                {
                    Debug.LogError("Can't attack. There is no target.");
                    return;
                }

                GameManager.current.TryToPerformSkill(this.selectedCreature, posibleTarget, this.selectedSkill);
                this.GoToMoveMode();
                break;
        }
    }

    public void Receive(Message msg)
    {
        if (msg is CreatureActionMoveMessage)
        {
            this.GoToMoveMode();
        }

        if (msg is CreatureActionSkillMessage)
        {
            CreatureActionSkillMessage casm = msg as CreatureActionSkillMessage;
            this.GoToSkillMode(casm.skill);
        }
    }

    public void GoToMoveMode()
    {
        this.selectedSkill = null;
        this.status = HumanCombatStatus.MOVE;
    }

    public void GoToSkillMode(Skill skill)
    {
        this.selectedSkill = skill;

        this.status = HumanCombatStatus.SKILL;
    }
}