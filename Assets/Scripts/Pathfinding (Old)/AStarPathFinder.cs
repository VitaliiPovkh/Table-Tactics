using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder
{
    private float step;
    private Vector2 targetCoordinates;
    private List<PathNode> openNodes;
    private List<PathNode> closedNodes;

    public AStarPathFinder()
    {
        openNodes = new List<PathNode>();
        closedNodes = new List<PathNode>();
    }

    public List<Vector2> GetPath(Vector2 currentCoordinates, Vector2 targetCoordinates, float minUnitSize)
    {
        step = minUnitSize;
        this.targetCoordinates = targetCoordinates;

        List<Vector2> path = new List<Vector2>();

        if (IsReachable(currentCoordinates))
        {
            path.Add(currentCoordinates);
            return path;
        }

        Bounds targetArea = new Bounds(targetCoordinates, Vector2.one * 2 * step);

        PathNode currentNode = new PathNode()
        {
            WorldCoordinates = currentCoordinates,
            Distance = 0,
            HeuristicDistance = ChebyshevHeuristic(currentCoordinates, targetCoordinates),
            PreviousNode = null
        };
        openNodes.Add(currentNode);
    
        while (!targetArea.Contains(currentNode.WorldCoordinates))
        {
            OpenMooreNeighbors(currentNode);

            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);
            
            currentNode = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                currentNode = openNodes[i].Weight < currentNode.Weight ? openNodes[i] : currentNode;
            }
        }

        while (currentNode != null)
        {           
            path.Add(currentNode.WorldCoordinates);
            currentNode = currentNode.PreviousNode;
        }
        path.Reverse();

        //DEBUG
        openNodes.ForEach(n =>
        {
            List<Vector2> tPath = new List<Vector2>();
            while (n != null)
            {
                tPath.Add(n.WorldCoordinates);
                n = n.PreviousNode;
            }
            tPath.Reverse();
            DrawDebugPath(tPath, Color.yellow, 5f);
        });
        DrawDebugPath(path, Color.red, 5f);
        //DEBUG

        openNodes.Clear();
        closedNodes.Clear();
  
        return path;
    }
    
    private float ChebyshevHeuristic(Vector2 pointA, Vector2 pointB)
    {
        float diffX = Mathf.Abs(pointA.x - pointB.x);
        float diffY = Mathf.Abs(pointA.y - pointB.y);

        return Mathf.Max(diffX, diffY);
    }

    private void OpenMooreNeighbors(PathNode currentNode)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                
                PathNode newNode = new PathNode();

                Vector2 newNodeCoords = currentNode.WorldCoordinates;
                newNodeCoords.x = Mathf.Floor(newNodeCoords.x + step * i);
                newNodeCoords.y = Mathf.Floor(newNodeCoords.y + step * j);
                newNode.WorldCoordinates = newNodeCoords;

                newNode.PreviousNode = currentNode;

                bool isCurrentNode = i == 0 && j == 0;
                bool isWalkable = IsWalkable(newNode);
                bool isClosed = closedNodes.Exists(n => n.WorldCoordinates == newNode.WorldCoordinates);
                if (isCurrentNode || !isWalkable || isClosed) continue;

                newNode.Distance = currentNode.Distance + ((i == j || i == -j) ? step * Mathf.Sqrt(2) : step);
                newNode.HeuristicDistance = ChebyshevHeuristic(newNode.WorldCoordinates, targetCoordinates);      

                PathNode existedNode = openNodes.Find(n => n.WorldCoordinates == newNodeCoords);
                if (existedNode != null)
                {
                    if (existedNode.Distance > newNode.Distance)
                    {
                        existedNode.Distance = newNode.Distance;
                        existedNode.PreviousNode = currentNode;
                    }
                    continue;
                }

                openNodes.Add(newNode);
            }
        }
    }

    private bool IsWalkable(PathNode node)
    {
        return Physics2D.Linecast(node.PreviousNode.WorldCoordinates, node.WorldCoordinates, (int)Layers.MAP_OBJECTS).collider == null;
    }
    private bool IsReachable(Vector2 position)
    {
        return Physics2D.Linecast(position, targetCoordinates, (int)Layers.MAP_BORDERS).collider != null;
    }

    private void DrawDebugPath(List<Vector2> points, Color c, float time)
    {
        for (int i = 1; i < points.Count; i++)
        {
            if (points[i] != null)
            {
                Debug.DrawLine(points[i - 1], points[i], c, time);
            }
        }
    }


}
