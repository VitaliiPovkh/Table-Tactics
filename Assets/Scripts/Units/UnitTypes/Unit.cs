
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Unit : Selectable
{
    [SerializeField] private UnitInfo info;
    [SerializeField] private PolygonCollider2D attackArea;

    [Range(1f, 1.7f)]
    [SerializeField] private float squadeScale = 1f;

    private float currentHp;
    private float currentAmmo;

    private Color unitColor;
    private Vector2 movementDirection;

    private AStarPathFinder pathFinder;
    private List<Vector2> movementPath;
    private int pathIdx = 0;

    private SpriteRenderer spriteRenderer;
    private Vector2 initialSize = new Vector2(10, 10);
    private float currentAngularSpeed;

    private void Start()
    {           
        spriteRenderer = GetComponent<SpriteRenderer>();
        pathFinder = new AStarPathFinder();

        MovementDirection = transform.position;
        unitColor = spriteRenderer.color;

        
        transform.localScale = initialSize * squadeScale;
        attackArea.transform.localScale = new Vector2(1, info.AttackRange);
        attackArea.transform.position += (Vector3)Vector2.up * info.AttackRange / 2 * 10;
    }

    private void Update()
    {


        if (movementPath.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, movementPath[pathIdx], info.Speed * Time.deltaTime);


            Vector3 targetDirection = (Vector3)movementPath[pathIdx] - transform.position;
            if (targetDirection != Vector3.zero)
            {
                targetDirection.z = 0f;
                float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
                float currentAngle = transform.eulerAngles.z;

                float newAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref currentAngularSpeed, 1f);

                transform.eulerAngles = Vector3.forward * newAngle;
            }
            
        }
        if ((Vector2)transform.position == movementPath[pathIdx] && pathIdx < movementPath.Count - 1)
        {
            pathIdx++;
        }

        
    }

    private void OnValidate()
    {
        transform.localScale = initialSize * squadeScale;       
    }

    public abstract void Attack(Unit unit);

    public void Select()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f);   
    }

    public void Deselect()
    {
        spriteRenderer.color = unitColor;      
    }

    public Vector2 MovementDirection
    {
        get => movementDirection;
        set
        {
            movementDirection = value;
            movementPath = pathFinder?.GetPath(transform.position, MovementDirection, transform.localScale.y);
            pathIdx = 0;
        }
    }

}
