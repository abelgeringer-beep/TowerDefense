using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapVisualizer : MonoBehaviour
{
    private Transform parent;
    private object data;

    public Color startColor;
    public Color endColor;
    public Dictionary<Vector3, GameObject> dictionaryOfObstacles;

    public GameObject roadStraight, roadTileCorner, tileEmpty, startTile, exitTile;
    public GameObject[] environmentTiles;

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
        for(int i = 0; i < data.path.Count; ++i)
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

                switch(cell.objectType)
                {
                    case CellObjectType.Empty:
                        CreateIndicator(position, tileEmpty);
                        break;
                    case CellObjectType.Road:
                        CreateIndicator(position, roadStraight);
                        break;
                    case CellObjectType.Exit:
                        CreateIndicator(position, exitTile);
                        break;
                    case CellObjectType.Start:
                        CreateIndicator(position, startTile);
                        break;
                    case CellObjectType.Obstacle:
                        CreateIndicator(position, environmentTiles[Random.Range(0, environmentTiles.Length)]);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void CreateIndicator(Vector3 position, GameObject prefab, Quaternion rotation = new Quaternion())
    {
        Vector3 placementPosition = position + new Vector3(0.5f, 0.5f, 0.5f);
        
        GameObject element = Instantiate(prefab, placementPosition, rotation);
        element.transform.parent = parent;
        dictionaryOfObstacles.Add(position, element);
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
    }

    public void ClearMap() {
        foreach(GameObject obstacle in dictionaryOfObstacles.Values)
            Destroy(obstacle);
        dictionaryOfObstacles.Clear();
    }
}
