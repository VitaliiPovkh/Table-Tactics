using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class MovementScript : MonoBehaviour
{
    private AStarPathFinder pathFinder;
    private List<Vector2> movementPath;
    private int pathIdx = 0;

    private float currentAngularSpeed;
    private Vector2 movementDirection;

    private void Start()
    {
        pathFinder = new AStarPathFinder();
        MovementDirection = transform.position;      
    }

    private void Update()
    {
        if (movementPath.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, movementPath[pathIdx], Info.Speed * Time.deltaTime);
            Rotate();
        }
        if ((Vector2)transform.position == movementPath[pathIdx] && pathIdx < movementPath.Count - 1)
        {
            pathIdx++;
        }
    }

    private void Rotate()
    {
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

    public UnitInfo Info { private get; set; }

}
