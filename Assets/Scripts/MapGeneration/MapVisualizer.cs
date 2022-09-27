using Enemy;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapVisualizer : MonoBehaviour
{
    private Transform parent;
    private object data;
    private List<GameObject> sortedWayPoints = new List<GameObject>();

    public Color startColor;
    public Color endColor;
    public Dictionary<Vector3, GameObject> dictionaryOfObstacles;

    public GameObject roadStraight, roadTileCorner, tileEmpty, startTile, exitTile, wayPoint;
    public GameObject[] environmentTiles;
    public bool animate;

    private void Awake()
    {
        parent = transform;
        dictionaryOfObstacles = new Dictionary<Vector3, GameObject>();
    }

    public void VisualizeMap(MapGrid grid, MapData data, bool visualizeUsingPrefabs)
    {
        if(visualizeUsingPrefabs)
        {
            VisualizeUsingPrefabs(grid, data);
            return;
        }

        VisualizeUsingPremitives(grid, data);
    }

    private void VisualizeUsingPrefabs(MapGrid grid, MapData data)
    {
        for (int i = 0; i < data.path.Count; ++i)
        {
            Vector3 position = data.path[i];
            if (position == data.endPosition)
                continue;

            grid.SetCell(position.x, position.z, CellObjectType.Road);
        }

        for(int col = 0; col < grid.width; ++col)
        {
            for(int row = 0; row < grid.length; ++row)
            {
                Cell cell = grid.GetCell(col, row);
                Vector3 position = new Vector3(cell.x, 0, cell.z);
                int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

                if (data.obsticles[index] && !cell.isTaken)
                {
                    cell.objectType = CellObjectType.Obstacle;
                }

                // Direction previousDirection = Direction.None;
                Direction nextDirection = Direction.None;
                switch(cell.objectType)
                {
                    case CellObjectType.Empty:
                        CreateIndicator(position, tileEmpty, new Quaternion(), GetOffset(row, col));
                        break;
                    case CellObjectType.Road:
                        Instantiate(wayPoint, position + GetOffset(row, col) + new Vector3(0, 0.5f, 0), new Quaternion());
                        CreateIndicator(position, roadStraight, new Quaternion(), GetOffset(row, col));
                        break;
                    case CellObjectType.Exit:
                        Instantiate(wayPoint, position + GetOffset(row, col) + new Vector3(0, 0.5f, 0), new Quaternion());
                        CreateIndicator(position, exitTile, new Quaternion(), GetOffset(row, col));
                        break;
                    case CellObjectType.Start:
                        CreateIndicator(position, startTile, new Quaternion(), GetOffset(row, col));
                        Instantiate(wayPoint, position + GetOffset(row, col) + new Vector3(0, 0.5f, 0), new Quaternion());
                        startTile.transform.position = position + GetOffset(row, col) + new Vector3(0, 0.5f, 0);
                        sortedWayPoints.Add(startTile);
                        break;
                    case CellObjectType.Obstacle:
                        if(data.path.Count > 0)
                        {
                            nextDirection = GetDirectionFromVectors(data.path[0], position);
                        }
                        GameObject environmentTile = environmentTiles[Random.Range(0, environmentTiles.Length)];
                        CreateIndicator(position, environmentTile, new Quaternion(), GetOffset(row, col));
                        break;
                    default:
                        break;
                }
            }
        }
        GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("WayPoint");
        GameObject[] sorted = new GameObject[wayPoints.Length + 1];
        sorted[0] = startTile;

        for(int i = 1; i < sorted.Length; ++i)
        {
            sorted[i] = FindClosest(sorted[i - 1], wayPoints);
            sortedWayPoints.Add(sorted[i]);
        }
    }

    private GameObject FindClosest(GameObject closestTo, GameObject[] NearGameobjects)
    {
        float oldDistance = 9999999f;
        GameObject closestObject = new GameObject();
        foreach (GameObject g in NearGameobjects)
        {
            float dist = Vector3.Distance(closestTo.transform.position, g.transform.position);
            if (dist < oldDistance && !sortedWayPoints.Contains(g))
            {
                closestObject = g;
                oldDistance = dist;
            }
        }

        return closestObject;
    }

    private Vector3 GetOffset(int row, int col)
    {
        Vector3 offsetPosition = new Vector3(0f, 0f, 0f);
     
        offsetPosition.x = col * 3f;
        offsetPosition.z = row * 3f;
        
        return offsetPosition;
    }

    private Direction GetDirectionFromVectors(Vector3 positionToGoTo, Vector3 position)
    {
        if(positionToGoTo.x > position.x)
        {
            return Direction.Right;
        } 
        else if (positionToGoTo.x < position.x) 
        { 
            return Direction.Left;
        }
        else if (positionToGoTo.z < position.z) 
        {
            return Direction.Down;
        }
        return Direction.Up;
    }

    private void CreateIndicator(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion(), Vector3 offset = new Vector3())
    {
        Vector3 placementPosition = position + offset;
        
        GameObject element = Instantiate(prefab, placementPosition, rotation);
        element.transform.parent = parent;
        dictionaryOfObstacles.Add(position, element);

        if (animate)
        {
            element.AddComponent<DropTween>();
            DropTween.IncreaseDropTime();
        }
    }

    private void VisualizeUsingPremitives(MapGrid grid, MapData data)
    {
        PlaceStartAndExitPoints(data);

        for(int i = 0; i < data.obsticles.Length; ++i)
        {
            if (data.obsticles[i])
            {
                Vector3 positionOnGrid = grid.CalculateCoordinatesFromIndex(i);
                if(positionOnGrid == data.startPosition || positionOnGrid == data.endPosition)
                    continue;
                

                grid.SetCell(positionOnGrid.x, positionOnGrid.z, CellObjectType.Obstacle);

                if (PlaceKnightObsticle(data, positionOnGrid))
                    continue;

                if(dictionaryOfObstacles.ContainsKey(positionOnGrid) == false)
                    CreateIndicator(positionOnGrid, Color.white, PrimitiveType.Cube);
            }
        }
    }

    private bool PlaceKnightObsticle(MapData data, Vector3 positionOnGrid)
    {
        for(int i = 0; i < data.knightPieces.Count; ++i)
        {
            if (data.knightPieces[i].position == positionOnGrid)
            {
                CreateIndicator(positionOnGrid, Color.red, PrimitiveType.Cube);
                return true;
            }
        }

        return false;
    }


    private void PlaceStartAndExitPoints(MapData data)
    {
        CreateIndicator(data.startPosition, startColor, PrimitiveType.Sphere);
        CreateIndicator(data.endPosition, endColor, PrimitiveType.Sphere);
    }

    private void CreateIndicator(Vector3 position, Color color, PrimitiveType sphere)
    {
        GameObject element = GameObject.CreatePrimitive(sphere);
        dictionaryOfObstacles.Add(position, element);
        element.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
        element.transform.parent = this.parent;
        Renderer renderer = element.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);

        if(animate)
        {
            element.AddComponent<DropTween>();
            DropTween.IncreaseDropTime();
        }
    }

    public void ClearMap() {
        foreach(GameObject obstacle in dictionaryOfObstacles.Values)
            Destroy(obstacle);
        dictionaryOfObstacles.Clear();

        if(animate)
            DropTween.ResetTime();
        
    }
}
