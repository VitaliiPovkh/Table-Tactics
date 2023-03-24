using UnityEngine;

public abstract class Selectable : MonoBehaviour
{
    private Color unitColor;
    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        unitColor = spriteRenderer.color;
    }

    public void Select()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f);
    }

    public void Deselect()
    {
        spriteRenderer.color = unitColor;
    }

    protected SpriteRenderer SpriteRenderer => spriteRenderer;
}
