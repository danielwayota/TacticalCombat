using UnityEngine;

using System.Collections.Generic;

public abstract class AdventureMapNode : MonoBehaviour
{
    public Sprite notVisitedGfx;
    public Sprite visitedGfx;

    public bool canBeVisited { get; protected set; } = false;
    public bool hasBeenVisited { get; protected set; } = false;

    private SpriteRenderer spriteRenderer;

    public List<AdventureMapNode> forwardConnections = new();

    public void Start()
    {
        this.OnStatusChanged();
        this.DisplayConnections();
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

    public void DenyVisit()
    {
        this.hasBeenVisited = false;
        this.canBeVisited = false;

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

    public void DisplayConnections()
    {
        foreach (var node in this.forwardConnections)
        {
            GameObject obj = new GameObject("node-line");
            LineRenderer line = obj.AddComponent<LineRenderer>();

            Vector3 offset = (node.transform.position - this.transform.position).normalized * 0.5f;

            line.SetPositions(new Vector3[] {
                this.transform.position + offset,
                node.transform.position - offset
            });

            line.widthMultiplier = 0.1f;
            line.startColor = Color.gray;
            line.endColor = Color.gray;
            line.material = new Material(Shader.Find("Sprites/Default"));

            obj.transform.parent = this.transform;
        }
    }

    public abstract void Visit();
}