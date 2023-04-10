using UnityEngine;

[RequireComponent(typeof(Unit))]
public class Selectable : MonoBehaviour
{
    private Color unitColor;

    private void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Unit = GetComponent<Unit>();
        unitColor = SpriteRenderer.color;
    }

    public void Select()
    {
        SpriteRenderer.color = new Color(1f, 1f, 1f);
    }

    public void Deselect()
    {
        SpriteRenderer.color = unitColor;
    }

    protected SpriteRenderer SpriteRenderer { get; private set; }
    public Unit Unit { get; private set; }
}
