using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit), typeof(Seeker))]
public class MovementScript : MonoBehaviour
{
    private Seeker seeker;
    private Path movementPath;
    private int pathIdx = 0;

    private float currentAngularSpeed;
    private Vector2 movementDirection;

    private void Start()
    {
        MovementDirection = transform.position;      
        seeker = GetComponent<Seeker>();
    }

    private void Update()
    {
        if (movementPath == null) return;

        Vector3 nextPosition = (Vector3)movementPath.path[pathIdx].position;
        if (movementPath.path.Count != 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, nextPosition, Info.Speed * Time.deltaTime);
            Rotate(nextPosition);
        }
        if (transform.position == nextPosition && pathIdx < movementPath.path.Count - 1)
        {
            pathIdx++;
        }

    }

    private void Rotate(Vector3 position)
    {
        Vector3 targetDirection = position - transform.position;
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
            movementPath = seeker?.StartPath(transform.position, movementDirection);
            pathIdx = 0;
        }
    }

    public UnitInfo Info { private get; set; }

}
