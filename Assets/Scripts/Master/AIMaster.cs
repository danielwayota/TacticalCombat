using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMaster : Master
{
    protected Creature lastTarget;

    public override void BeginTurn()
    {
        this.lastTarget = null;

        this.BeginTurnToAllCreatures();
        StartCoroutine(this.TurnRutine());
    }

    private Vector3 GenerateCreatureTarget(Creature creature)
    {
        Stats stats = creature.GetCurrentStats();
        List<Vector3> reachArea = GameManager.current.mapManager.PredictAreaFor(
            creature.transform.position,
            stats.speed
        );
        List<Creature> enemies = GameManager.current.GetEnemyCreaturesInArea(
            reachArea,
            this
        );

        if (enemies.Count != 0)
        {
            // Obtenemos el enemigo más cercano
            Creature nearest = enemies[0];
            float lastDistance = 9999;

            foreach (var enemy in enemies)
            {
                float distance = Vector3.Distance(enemy.transform.position, creature.transform.position);
                if (distance < lastDistance)
                {
                    lastDistance = distance;
                    nearest = enemy;
                }
            }

            this.lastTarget = nearest;
        }

        if (this.lastTarget != null)
        {
            return this.GenerateRandomTargetInArea(creature, this.lastTarget.transform.position, 1f);
        }

        return this.GenerateRandomTargetInArea(creature, creature.transform.position, stats.speed);
    }

    private Vector3 GenerateRandomTargetInArea(Creature creature, Vector3 center, float distance)
    {
        int attempts = 0;

        while (attempts < 32)
        {
            attempts++;

            var offset = new Vector3(
                Random.Range(-distance, distance),
                Random.Range(-distance, distance)
            );

            Vector3 target = center + offset;

            if (GameManager.current.CanMoveCreatureTo(creature, target))
            {
                return target;
            }
        }

        // No nos movemos.
        return center;
    }

    private IEnumerator TurnRutine()
    {
        foreach (var creature in this.creatures)
        {
            Vector3 target = this.GenerateCreatureTarget(creature);

            GameManager.current.MoveCreatureTo(creature, target);

            while (creature.isMoving)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            if (this.lastTarget != null)
            {
                Skill[] skills = creature.GetSkills();
                int rndIndex = Random.Range(0, skills.Length);
                Skill selectedSkill = skills[rndIndex];

                GameManager.current.TryToPerformSkillAtPoint(creature, selectedSkill, this.lastTarget.transform.position);
            }

            yield return new WaitForSeconds(0.5f);
        }

        GameManager.current.NextTurn();
    }
}