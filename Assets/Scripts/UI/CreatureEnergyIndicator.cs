using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CreatureEnergyIndicator : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer gfx;

    public void Display(int energy)
    {
        if (this.isActiveAndEnabled == false)
        {
            return;
        }

        if (this.gfx == null)
        {
            this.gfx = this.GetComponent<SpriteRenderer>();
        }

        this.gfx.sprite = this.sprites[energy];
    }
}