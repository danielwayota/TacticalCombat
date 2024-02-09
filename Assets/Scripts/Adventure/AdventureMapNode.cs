using UnityEngine;

public abstract class AdventureMapNode : MonoBehaviour
{
    public Sprite notVisitedGfx;
    public Sprite visitedGfx;

    public bool canBeVisited { get; protected set; } = false;
    public bool hasBeenVisited { get; protected set; } = false;

    private SpriteRenderer spriteRenderer;

    public void Start()
    {
        this.OnStatusChanged();
    }

    // Require un collider.
    public void OnMouseEnter()
    {
        this.transform.localScale = Vector3.one * 1.2f;
    }

    // Require un collider.
    public void OnMouseExit()
    {
        this.transform.localScale = Vector3.one;
    }

    // Require un collider.
    public void OnMouseDown()
    {
        if (this.canBeVisited == false)
        {
            Debug.Log("El nodo no puede ser visitado!");
            return;
        }

        if (this.hasBeenVisited)
        {
            Debug.Log("Nodo ya visitado!");
            return;
        }

        this.Visit();
    }

    public void AllowVisit()
    {
        this.hasBeenVisited = false;
        this.canBeVisited = true;

        this.OnStatusChanged();
    }

    public void MarkAsVisited()
    {
        this.hasBeenVisited = true;
        AdventureManager.current.OnNodeVisited(this);

        this.OnStatusChanged();
    }

    protected void OnStatusChanged()
    {
        if (this.spriteRenderer == null)
        {
            this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        if (this.canBeVisited)
        {
            this.spriteRenderer.color = new Color(1, 1, 1, 1);
        }
        else
        {
            this.spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }

        if (this.hasBeenVisited)
        {
            this.spriteRenderer.sprite = this.visitedGfx;
        }
        else
        {
            this.spriteRenderer.sprite = this.notVisitedGfx;
        }
    }

    public abstract void Visit();
}