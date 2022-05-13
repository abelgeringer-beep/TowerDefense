using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Color;

public class MapGenerator : MonoBehaviour
{
    private const int MoveStraightCost = 10;
    private const int MoveDiagonalCost = 14;
    
    public GameObject node;
    public GameObject start;
    public GameObject end;

    [SerializeField] private int width;
    [SerializeField] private int height;

    private List<GameObject[,]> _mapSections;
    private GameObject[,] _nodes;
    private List<GameObject> _wayPoints;

    private List<PathNode> _openList;
    private List<PathNode> _closedList;
    private PathNode[,] _pathNodes;
    private static readonly int Color1 = Shader.PropertyToID("Color");

    private const float Scale = 2.5f;
    private void Start()
    {
        
        _pathNodes = new PathNode[width, height];
        _nodes = new GameObject[width, height];
        GenerateMap();
        // DeleteRandomNodes(width * height / 5);
        DrawPath();
    }
    
    private List<GameObject> FindPath(int startX, int startY, int endX, int endY)
    {
        PathNode startNode = new PathNode(_nodes[startX, startY]);
        PathNode endNode = new PathNode(_nodes[endX, endY]);
        
        Instantiate(start, new Vector3(startX * 6, Scale, startY * 6), Quaternion.identity);
        Instantiate(end, new Vector3(endX * 6, Scale, endY * 6), Quaternion.identity);
        
        _openList = new List<PathNode>{startNode};
        _closedList = new List<PathNode>();

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                PathNode tmpPathNode = new PathNode(_nodes[i,j])
                    {
                        GCost = int.MaxValue
                    };

                tmpPathNode.CalcFCost();

                tmpPathNode.Prev = null;
                _pathNodes[i,j] = tmpPathNode;
            }
        }

        startNode.GCost = 0;
        startNode.HCost = CalcDistance(startNode, endNode);
        startNode.CalcFCost();

        while (_openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(_openList);
            
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);
            
            foreach (PathNode neighbour in GetNeighbours(currentNode))
            {
                if (_closedList.Contains(neighbour))
                    continue;

                int tentativeGCost = currentNode.GCost + CalcDistance(currentNode, neighbour);

                if (tentativeGCost >= neighbour.GCost) continue;
                
                neighbour.Prev = currentNode;
                neighbour.GCost = tentativeGCost;
                neighbour.HCost = CalcDistance(neighbour, endNode);
                neighbour.CalcFCost();
                    
                if(!_openList.Contains(neighbour))
                    _openList.Add(neighbour);
            }
        }

        return null;
    }

    private List<PathNode> GetNeighbours(PathNode currentNode)
    {
        List<PathNode> neighbours = new List<PathNode>();
        
        // left
        if (currentNode.x - 1 >= 0)
            neighbours.Add(GetPathNode(currentNode.x - 1, currentNode.y));
        // right
        if (currentNode.x + 1 < width)
            neighbours.Add(GetPathNode(currentNode.x + 1, currentNode.y));
        // down
        if (currentNode.y - 1 >= 0)
            neighbours.Add(GetPathNode(currentNode.x, currentNode.y - 1));
        // up
        if (currentNode.y + 1 < height)
            neighbours.Add(GetPathNode(currentNode.x, currentNode.y + 1));

        return neighbours;
    }

    private PathNode GetPathNode(float x, float y)
    {
        Collider[] tmp = Physics.OverlapSphere(new Vector3(x, 0.5f, y), 0.01f);
        Transform t = tmp[0].transform;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (_nodes[i, j].transform.position == t.position)
                {
                    Debug.Log("_nodes[i, j].transform" + _nodes[i, j].transform.position);
                    Debug.Log("_pathNodes[i, j]" + _pathNodes[i, j]);
                    return _pathNodes[i, j];
                }
            }
        }
        
        Debug.Log("null returned");
        return null;
    }

    private List<GameObject> CalculatePath(PathNode endNode)
    {
        List<GameObject> path = new List<GameObject>();

        PathNode currentNode = endNode;

        while (currentNode.Prev != null)
        {
            Debug.Log("pos: " + currentNode);
            path.Add(currentNode.node);
            currentNode = currentNode.Prev;
        }

        path.Reverse();

        return path;
    }

    private PathNode GetLowestFCostNode(List<PathNode> list)
    {
        PathNode lowest = list[0];

        foreach (var t in list)
            if (t.FCost < lowest.FCost)
                lowest = t;

        return lowest;
    }

    private int CalcDistance(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);

        int remaining = Mathf.Abs(xDistance - yDistance);

        return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
    }
    
    private void DrawPath()
    {
        for (int i = 0; i < 4; ++i)
        {
            //SetEndpointsInSection(_mapSections[i], ref _endpointsInMapSections);
            
            var path = FindPath(
                Random.Range(0, width),
                Random.Range(0, height),
                Random.Range(0, width),
                Random.Range(0, height)
            );

            foreach (GameObject t in path)
            {
                t.GetComponent<Renderer>().material.SetColor(Color1, gray);
            }
        }
    }

    private void SetEndpointsInSection(GameObject[,] mapSection, ref List<GameObject> endpointsInMapSections)
    {
        endpointsInMapSections.Add(mapSection[Random.Range(0, width / 2), Random.Range(0, height / 2)]);
        endpointsInMapSections.Add(mapSection[Random.Range(0, width / 2), Random.Range(0, height / 2)]);
    }
    
    private void DeleteRandomNodes(int amountToDelete)
    {
        for (int i = 0; i < amountToDelete; ++i)
        { 
            _nodes[Random.Range(0, width), Random.Range(0, height)].SetActive(false);
        }
    }
    private void GenerateMap()
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                GameObject tmp = Instantiate(node, new Vector3(j * 6f, 0f, i * 6f), Quaternion.identity);
                _nodes[i, j] = tmp;
            }
        }
    }
}