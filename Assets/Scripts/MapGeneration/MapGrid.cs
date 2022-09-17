using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MapGrid
{
    public int width { get; }
    public int length { get; }

    public Cell[,] cellGrid;
    
    public MapGrid(int w, int l)
    {
        width = w;
        length = l;

        CreateGrid(width, length);
    }

    private void CreateGrid(int width, int length)
    {
        this.cellGrid = new Cell[width, length];
        for(int row = 0; row < width; ++row)
        {
            for (int col = 0; col < width; ++col)
            {
                cellGrid[row, col] = new Cell(col, row);
            }
        }
    }

    public void SetCell(int x, int z, CellObjectType cellObjectType, bool isTaken = false)
    {
        cellGrid[x, z].objectType = cellObjectType;
        cellGrid[x, z].isTaken = isTaken;
    }

    public void SetCell(float x, float z, CellObjectType cellObjectType, bool isTaken = false)
    {
        SetCell((int)x, (int)z, cellObjectType, isTaken);
    }

    public bool IsCellTaken(int x, int z)
    {
        return cellGrid[x, z].isTaken;
    }

    public Vector3 CalculateCoordinatesFromIndex(int randomIndex)
    {
        int x = randomIndex % width;
        int z = randomIndex / length;

        return new Vector3(x, 0, z);
    }

    public bool IsCellValid(float x, float z)
    {
        return x < width && x >= 0 && z < length && z>= 0;
    }

    public int CalculateIndexFromCoordinates(int x, int z)
    {
        return x + width * z;
    }

    public int CalculateIndexFromCordinates(float x, float z)
    {
        return CalculateIndexFromCoordinates((int) x, (int) z);
    }

    public void CheckCoordinates()
    {
        for(int x = 0; x < cellGrid.GetLength(0); ++x)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for(int z = 0; z < cellGrid.GetLength(1); ++z)
            {
                stringBuilder.Append(z + ";" + x + " ");
            }
            Debug.Log(stringBuilder.ToString());
        }
    }
}
