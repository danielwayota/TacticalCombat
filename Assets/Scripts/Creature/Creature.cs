using System.Collections;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Vector2Int localPosition;

    public GameObject selectionIndicator;

    public float movementSpeed = 4f;

    private bool isSelected = false;

    // stats
    public int speed = 4;

    public int maxEnergy = 2;
    private int energy;

    void Start()
    {
        this.Recharge();

        this.SetSelectionStatus(false);
    }

    public void Recharge()
    {
        this.UpdateEnergy(this.maxEnergy);
    }

    public int CurrentMaxDistance()
    {
        return this.speed * this.energy;
    }

    private void UpdateEnergy(int e)
    {
        this.energy = e;
        CreatureUI.current.DisplayEnergy(e);
    }

    public void SetSelectionStatus(bool isSelected)
    {
        this.selectionIndicator.SetActive(isSelected);

        this.isSelected = isSelected;

        if (this.isSelected)
        {
            CreatureUI.current.Show();
            CreatureUI.current.DisplayEnergy(this.energy);
        }
        else
        {
            CreatureUI.current.Hide();
        }
    }

    public void FollowPath(Vector3[] worldPath)
    {
        StopAllCoroutines();
        StartCoroutine(this.FollowPathRutine(worldPath));
    }

    private IEnumerator FollowPathRutine(Vector3[] worldPath)
    {
        int pathLength = Mathf.Min(this.CurrentMaxDistance(), worldPath.Length);
        int cost = this.GetEnergyCostForPathLength(pathLength);

        this.UpdateEnergy(this.energy - cost);

        for (int i = 0; i < pathLength; i++)
        {
            Vector3 target = worldPath[i];

            float percent = 0;

            Vector3 start = this.transform.position;

            while (percent < 1f)
            {
                this.transform.position = Vector3.Lerp(start, target, percent);

                percent += Time.deltaTime * this.movementSpeed;
                yield return null;
            }

            this.transform.position = target;
        }
    }

    public int GetEnergyCostForPathLength(int length)
    {
        // speed = 4
        // 2 / 4 = 0.5 -> ceil 1
        // 5 / 4 = 1.25   -> ceil 2

        int cost = Mathf.CeilToInt(length / (float)this.speed);

        return cost;
    }
}
