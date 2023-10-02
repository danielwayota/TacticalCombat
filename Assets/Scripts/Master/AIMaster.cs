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
        List<Vector3> reachArea = BattleManager.current.mapManager.PredictAreaFor(
            creature.transform.position,
            stats.speed
        );
        List<Creature> enemies = BattleManager.current.GetEnemyCreaturesInArea(
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
            return this.GenerateNearestPointAroundTarget(creature, this.lastTarget.transform.position);
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

            if (BattleManager.current.CanMoveCreatureTo(creature, target))
            {
                return target;
            }
        }

        // No nos movemos.
        return center;
    }

    private Vector3 GenerateNearestPointAroundTarget(Creature creature, Vector3 targetCenter)
    {
        Vector3[] posibleTargets = new Vector3[] {
            targetCenter + Vector3.up,
            targetCenter + Vector3.left,
            targetCenter + Vector3.down,
            targetCenter + Vector3.right,
        };

        bool sorted = false;
        while (!sorted)
        {
            sorted = true;

            for (int i = 0; i < posibleTargets.Length - 1; i++)
            {
                float a = Vector3.SqrMagnitude(posibleTargets[i + 0] - creature.transform.position);
                float b = Vector3.SqrMagnitude(posibleTargets[i + 1] - creature.transform.position);

                // Ya estamos en uno de los destinos posibles.
                if (a < 1f)
                {
                    // No moverse.
                    return posibleTargets[i];
                }

                if (b < a)
                {
                    sorted = false;

                    Vector3 tmp = posibleTargets[i + 0];
                    posibleTargets[i + 0] = posibleTargets[i + 1];
                    posibleTargets[i + 1] = tmp;
                }
            }
        }

        foreach (var targetPoint in posibleTargets)
        {
            if (BattleManager.current.CanMoveCreatureTo(creature, targetPoint))
            {
                return targetPoint;
            }
        }

        // No nos movemos.
        return creature.transform.position;
    }

    private IEnumerator TurnRutine()
    {
        // PRUEBA:
        // Cambiamos el órden de las criaturas enemigas para generar comportamientos inesperados.
        for (int i = 0; i < this.creatures.Count - 1; i++)
        {
            int index = Random.Range(i, this.creatures.Count);

            Creature tmp = this.creatures[i];
            this.creatures[i] = this.creatures[index];
            this.creatures[index] = tmp;
        }

        foreach (var creature in this.creatures)
        {
            Vector3 target = this.GenerateCreatureTarget(creature);

            BattleManager.current.MoveCreatureTo(creature, target);

            while (creature.isMoving)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            Skill[] skills = creature.GetSkills();
            int rndIndex = Random.Range(0, skills.Length);
            Skill selectedSkill = skills[rndIndex];

            // Si tiene rango 0, es una habilidad que me puedo lanzar a mí mismo.
            if (selectedSkill.range == 0)
            {
                BattleManager.current.TryToPerformSkillAtPoint(creature, selectedSkill, creature.transform.position);
            }
            else if (this.lastTarget != null)
            {
                BattleManager.current.TryToPerformSkillAtPoint(creature, selectedSkill, this.lastTarget.transform.position);
            }

            yield return new WaitForSeconds(0.5f);
        }

        BattleManager.current.NextTurn();
    }
}