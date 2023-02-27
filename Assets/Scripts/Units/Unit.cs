
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class Unit : Selectable
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float speed;

    private AStarPathFinder pathFinder;
    private Vector2 currentVelocity;

    private List<Vector2> movementPath;
    private int pathIdx = 0;

    private Vector2 movementDirection;
    public Vector2 MovementDirection 
    { 
        get { return movementDirection; }
        set
        {
            movementDirection = value;
            movementPath = pathFinder?.GetPath(transform.position, MovementDirection, transform.localScale.y);
            pathIdx = 0;
        }
    }
    

    private void Start()
    {           
        pathFinder = new AStarPathFinder();
        MovementDirection = transform.position;
    }

    private void Update()
    {
        if (movementPath.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, movementPath[pathIdx], speed * Time.deltaTime);
        }
        if ((Vector2)transform.position == movementPath[pathIdx] && pathIdx < movementPath.Count - 1)
        {
            pathIdx++;
        }
        transform.up = Vector2.SmoothDamp(transform.up, MovementDirection, ref currentVelocity, 0.1f, speed / 4);
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
