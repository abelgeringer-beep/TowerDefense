using System;
using UnityEngine;

public class MapVisualizer : MonoBehaviour
{
    private Transform parent;
    private object data;

    public Color startColor;
    public Color endColor;

    private void Awake()
    {
        parent = transform;
    }

    public void VisualizeMap(MapGrid grid, MapData data, bool visualizeUsingPrefabs)
    {
        if(visualizeUsingPrefabs)
        {
            VisualizeUsingPrefabs();
            return;
        }

        VisualizeUsingPremitives(grid, data);
    }

    private void VisualizeUsingPremitives(MapGrid grid, MapData data)
    {
        PlaceStartAndExitPoints(data);

        for(int i = 0; i < data.obsticles.Length; ++i)
        {
            if (data.obsticles[i])
            {
                Vector3 positionOnGrid = grid.CalculateCordinatesFromIndex(i);
                if(positionOnGrid == data.startPosition || positionOnGrid == data.endPosition)
                    continue;
                

                grid.SetCell(positionOnGrid.x, positionOnGrid.z, CellObjectType.Obsticle);

                if (PlaceKnightObsticle(data, positionOnGrid))
                    continue;
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
        element.transform.position = position + new Vector3(0.5f, 0.5f, 0.5f);
        element.transform.parent = this.parent;
        Renderer renderer = element.GetComponent<Renderer>();
        renderer.material.SetColor("_Color", color);
    }

    private void VisualizeUsingPrefabs()
    {
        throw new NotImplementedException();
    }
}
