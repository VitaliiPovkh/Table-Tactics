
using UnityEngine;


public class Unit : Selectable
{
    [SerializeField] private InputScript inputScript;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;

    private Vector2 currentVelocity;

    public Vector2 MovementDirection { get; set; }
    

    private void Start()
    {
        MovementDirection = transform.position;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, MovementDirection, speed * Time.deltaTime);
        transform.up = Vector2.SmoothDamp(transform.up, MovementDirection, ref currentVelocity, 0.1f, speed / 4);
        Debug.Log($"Current unti pos {transform.position}");
    }

    public void Select()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f);   
    }

    public void Deselect()
    {
        spriteRenderer.color = new Color(59f / 255f, 200f / 255f, 82f / 255f);      
    }
}
