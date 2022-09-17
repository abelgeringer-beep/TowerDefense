using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightPiece
{
    public static List<Vector3> possibleMoves = new List<Vector3>
    {
        new Vector3(-1, 0, 2),
        new Vector3( 1, 0, 2),
        new Vector3( 1, 0,-2),
        new Vector3(-1, 0,-2),
        new Vector3(-2, 0,-1),
        new Vector3( 2, 0,-1),
        new Vector3(-2, 0, 1),
        new Vector3( 2, 0, 1)
    };

    public Vector3 position;

    public KnightPiece(Vector3 position)
    {
        this.position = position;
    }
}
