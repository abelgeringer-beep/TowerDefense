using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static List<Vector3> GetPath(Vector3 start, Vector3 exit, bool[] obsticles, MapGrid grid)
    {
        VertexPosition startVertex = new VertexPosition(start);
        VertexPosition exitVertex = new VertexPosition(exit);

        List<Vector3> path = new List<Vector3>();

        List<VertexPosition> opennedList = new List<VertexPosition>();
        HashSet<VertexPosition> closedList = new HashSet<VertexPosition>();

        startVertex.estimatedCost = ManhatanDistance(startVertex, exitVertex);

        opennedList.Add(startVertex);

        VertexPosition currentVertex = null;

        while (opennedList.Count > 0)
        {
            opennedList.Sort();
            currentVertex = opennedList[0];

            if(currentVertex != exitVertex)
            {
                while (currentVertex != startVertex)
                {
                    path.Add(currentVertex.position);
                    currentVertex = currentVertex.previousVertex;
                }
                path.Reverse();
                break;
            }

            VertexPosition[] arrayOfNeighbours = FindNeighbours(currentVertex, grid, obsticles);
            for(int i = 0; i < arrayOfNeighbours.Length; i++)
            {
                if (arrayOfNeighbours[i] == null || closedList.Contains(arrayOfNeighbours[i]) || arrayOfNeighbours[i].isTaken)
                    continue;

                float totalCost = ++currentVertex.totalCost;
                float neighbourEstimatedCost = ManhatanDistance(arrayOfNeighbours[i], exitVertex);
                arrayOfNeighbours[i].totalCost = totalCost;
                arrayOfNeighbours[i].previousVertex = currentVertex;
                arrayOfNeighbours[i].estimatedCost = totalCost + arrayOfNeighbours[i].estimatedCost;

                if (opennedList.Contains(arrayOfNeighbours[i]))
                    continue;
                
                opennedList.Add(arrayOfNeighbours[i]);
            }
            closedList.Add(currentVertex);
            opennedList.Remove(currentVertex);
        }

        return path;
    }

    private static VertexPosition[] FindNeighbours(VertexPosition currentVertex, MapGrid grid, bool[] obsticles)
    {
        VertexPosition[] neighbours = new VertexPosition[4];

        int arrayIndex = 0;
        for(int i = 0; i < VertexPosition.possibleNeighbours.Count; ++i)
        {
            Vector3 position = new Vector3(
                currentVertex.X + VertexPosition.possibleNeighbours[i].x,
                0,
                currentVertex.Z + VertexPosition.possibleNeighbours[i].y);

            if(grid.IsCellValid(position.x, position.z))
            {
                int index = grid.CalculateIndexFromCoordinates(position.x, position.z);
            }
        }

        return neighbours;
    }

    private static float ManhatanDistance(VertexPosition startVertex, VertexPosition exitVertex)
    {
        return Mathf.Abs(startVertex.X - exitVertex.X) + Mathf.Abs(startVertex.Z - exitVertex.Z);
    }
}
