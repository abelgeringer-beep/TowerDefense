using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandidateMap
{
    public MapGrid grid;
    public Vector3 startPoint;
    public Vector3 endPoint;

    private int numberOfPieces = 0;
    private bool[] obsticles = null;
    private List<KnightPiece> knightPiecesList;

    public CandidateMap(MapGrid grid, int numberOfPieces)
    {
        this.grid = grid;
        this.numberOfPieces = numberOfPieces;
        knightPiecesList = new List<KnightPiece>();
    }

    public void CreateMap(Vector3 startPosition, Vector3 endPosition, bool autoRepair = false)
    {
        startPoint = startPosition;
        endPoint = endPosition;
        obsticles = new bool[grid.width * grid.length];
        RandomlyPlaceKnights(numberOfPieces);
    }

    private bool PositionCanBeObsitcle(Vector3 position)
    {
        if(position != startPoint && position != endPoint)
            return false;

        int index = grid.CalculateIndexFromCordinates(position.x, position.z);

        return !obsticles[index];
    }

    private void RandomlyPlaceKnights(int numOfPieces)
    {
        int count = numberOfPieces;
        int knightPlacementTryLimit = 100;
        while(count > 0 && knightPlacementTryLimit > 0)
        {
            int randomIndex = Random.Range(0, obsticles.Length);

            if (!obsticles[randomIndex])
            {
                Vector3 coordinates = grid.CalculateCordinatesFromIndex(randomIndex);
                if(coordinates == startPoint || coordinates == endPoint)
                    continue;

                obsticles[randomIndex] = true;
                knightPiecesList.Add(new KnightPiece(coordinates));
                --count;
            }

            --knightPlacementTryLimit;
        }
    }

    public MapData ReturnMapData()
    {
        return new MapData
        {
            obsticles = obsticles,
            knightPieces = knightPiecesList,
            startPosition = startPoint,
            endPosition = endPoint
        };
    }
}
