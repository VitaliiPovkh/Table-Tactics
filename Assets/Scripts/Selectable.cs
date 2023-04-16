using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Selectable : MonoBehaviour
{
    private Color unitColor;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Unit = GetComponent<Unit>();
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

    public Unit Unit { get; private set; }
}
