using System;
using System.Collections.Generic;
using UnityEngine;

public class VertexPosition : IEquatable<VertexPosition>, IComparable<VertexPosition>
{
    public static List<Vector2Int> possibleNeighbours = new List<Vector2Int>
    {
        new Vector2Int(0, -1),
        new Vector2Int(0, 1),
        new Vector2Int(-1, 0),
        new Vector2Int(1, 0),
    };

    public float totalCost;
    public float estimatedCost;
    public bool isTaken;
    public VertexPosition previousVertex = null;
    public Vector3 position;

    public int X { get => (int)position.x; }
    public int Z { get => (int)position.z; }

    public VertexPosition(Vector3 position, bool isTaken = false)
    {
        this.position = position;
        this.isTaken = isTaken;
        this.estimatedCost = 0;
        this.totalCost = 1;
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }

    public int GetHashCode(VertexPosition obj)
    {
        return obj.GetHashCode();
    }

    public int CompareTo(VertexPosition other)
    {
        return estimatedCost == other.estimatedCost ? 0 : estimatedCost > other.estimatedCost ? 1 : -1;
    }

    public bool Equals(VertexPosition other)
    {
        return position == other.position;
    }
}
