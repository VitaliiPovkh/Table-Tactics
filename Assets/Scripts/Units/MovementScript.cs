using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class MovementScript : MonoBehaviour
{
    private Seeker seeker;
    private AILerp aiLerp;

    private float currentAngularSpeed = 15f;
    private Vector2 movementDirection;

    private Rigidbody2D unitsBody;
    private Unit unit;

    private bool finalStop = false;
    private Path path;

    private void Start()
    {
        MovementDirection = transform.position;
        seeker = GetComponent<Seeker>();
        unitsBody = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
        aiLerp = GetComponent<AILerp>();

        aiLerp.speed = unit.Info.Speed;
        aiLerp.rotationSpeed = currentAngularSpeed;
    }

    private void FixedUpdate()
    {
        AIUnit target = unit.Target;
        if (unitsBody.position == movementDirection && target != null && !finalStop)
        {
            Vector2 direction = (unitsBody.position - (Vector2)target.transform.position).normalized;
            seeker.StartPath(unitsBody.position, unitsBody.position - direction / 2);
            finalStop = true;
        }
    }


    public Vector2 MovementDirection
    {
        get => movementDirection;
        set
        {
            movementDirection = value;
            path = seeker?.StartPath(unitsBody.position, movementDirection);
            finalStop = false;
        }
    }

    public bool IsPathFinished => (path != null && path.path.Count != 0) ? transform.position == (Vector3)path.path[path.path.Count - 1].position : true;

}