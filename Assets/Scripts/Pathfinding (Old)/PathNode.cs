
using UnityEngine;

public class PathNode
{
    private float distance;
    private float heuristicDistance;

    public Vector2 WorldCoordinates { get; set; }
    public float Distance 
    {
        get { return distance; }
        set 
        {
            distance = value;
            Weight = distance + heuristicDistance;
        }
    }
    public float HeuristicDistance 
    {
        get { return heuristicDistance; }
        set
        {
            heuristicDistance = value;
            Weight = distance + heuristicDistance;
        }
    }
    public float Weight { get; private set; }
    public PathNode PreviousNode { get; set; }
}
