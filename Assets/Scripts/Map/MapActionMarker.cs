using UnityEngine;

public class MapActionMarker : MonoBehaviour
{
    public GameObject skillReachMarkerSprite;
    public GameObject skillAttackMarkerSprite;

    public GameObject[] pathMarkerSprites;

    public bool visible { get; protected set; }

    void Awake()
    {
        this.Hide();
    }

    public void ShowForSkillReach()
    {
        this.Hide();

        this.skillReachMarkerSprite.SetActive(true);

        this.visible = true;
    }

    public void ShowForSkillAction()
    {
        this.Hide();

        this.skillAttackMarkerSprite.SetActive(true);

        this.visible = true;
    }

    public void ShowForPathUsingCost(int cost)
    {
        this.Hide();

        this.pathMarkerSprites[cost % this.pathMarkerSprites.Length].SetActive(true);

        this.visible = true;
    }

    public void Hide()
    {
        this.skillReachMarkerSprite.SetActive(false);
        this.skillAttackMarkerSprite.SetActive(false);

        foreach (var spr in this.pathMarkerSprites)
        {
            spr.SetActive(false);
        }

        this.visible = false;
    }
}