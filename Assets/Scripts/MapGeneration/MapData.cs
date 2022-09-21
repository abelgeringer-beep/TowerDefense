using System.Collections.Generic;
using UnityEngine;

public struct MapData
{
    public bool[] obsticles;
    public int cornersNearEachOther;
    public List<KnightPiece> knightPieces;
    public Vector3 startPosition;
    public Vector3 endPosition;
    public List<Vector3> path;
    public List<Vector3> corners;
}
