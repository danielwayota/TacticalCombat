using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIMaster : Master
{
    public override void BeginTurn()
    {
        this.RechargeAllCreatures();
        StartCoroutine(this.TurnRutine());
    }

    private IEnumerator TurnRutine()
    {
        foreach (var creature in this.creatures)
        {
            int attempts = 0;

            while (attempts < 32)
            {
                attempts++;

                var offset = new Vector3(
                    Random.Range(-5, 5),
                    Random.Range(-5, 5)
                );

                Vector3 target = creature.transform.position + offset;

                if (GameManager.current.mapManager.IsAGroundTile(target) == false)
                {
                    continue;
                }

                GameManager.current.MoveCreatureTo(creature, target);
                break;
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1);
        GameManager.current.NextTurn();
    }
}