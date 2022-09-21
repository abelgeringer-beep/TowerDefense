using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CandidateMap
{
    public MapGrid grid;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public bool[] obstacles = null;

    private int numberOfPieces = 0;
    private List<KnightPiece> knightPiecesList;
    private List<Vector3> path;

    private List<Vector3> corners;
    private int cornersNearEachOther;
    private CandidateMap candidateMap;

    public CandidateMap(MapGrid grid, int numberOfPieces)
    {
        this.grid = grid;
        this.numberOfPieces = numberOfPieces;
        knightPiecesList = new List<KnightPiece>();
    }

    public CandidateMap(CandidateMap candidateMap)
    {
        grid = candidateMap.grid;
        startPoint = candidateMap.startPoint;
        endPoint = candidateMap.endPoint;
        obstacles = (bool[])candidateMap.obstacles.Clone();
        corners = new List<Vector3>(candidateMap.corners);
        cornersNearEachOther = candidateMap.cornersNearEachOther;
        path = new List<Vector3>(candidateMap.path);
    }

    public void CreateMap(Vector3 startPosition, Vector3 endPosition, bool autoRepair = false)
    {
        startPoint = startPosition;
        endPoint = endPosition;
        obstacles = new bool[grid.width * grid.length];
        RandomlyPlaceKnights(numberOfPieces);
        PlaceObstacles();
        FindPath();

        if(autoRepair)
            Repair();
    }

    public void FindPath()
    {
        path = AStar.GetPath(startPoint, endPoint, obstacles, grid);
        corners = GetCorners(path);
        cornersNearEachOther = CalculateConrersNearEachOther(corners);
    }

    private int CalculateConrersNearEachOther(List<Vector3> corners)
    {
        int cornersNearEachOther = 0;
        for(int i = 0; i < corners.Count - 1; i++)
            if (Vector3.Distance(corners[i], corners[i + 1]) <= 1)
                ++cornersNearEachOther;

        return cornersNearEachOther;
    }

    private List<Vector3> GetCorners(List<Vector3> path)
    {
        List<Vector3> cornerPositions = new List<Vector3>();
        List<Vector3> pathWithStart = new List<Vector3>(path);
       pathWithStart.Insert(0, startPoint);
        if (pathWithStart.Count <= 0)
            return cornerPositions;

        for(int i = 0; i < cornerPositions.Count - 2; i++)
        {
            if ((pathWithStart[i + 1].x != cornerPositions[i].x && pathWithStart[i + 2].z != pathWithStart[i + 1].z) 
                    || (pathWithStart[i + 1].z != cornerPositions[i].z && pathWithStart[i + 2].x != pathWithStart[i + 1].x))
            {
                cornerPositions.Add(pathWithStart[i + 1]);
            }
        }
        return cornerPositions;
    }

    private bool PositionCanBeObsitcle(Vector3 position)
    {
        if (position == startPoint || position == endPoint)
            return false;
        
        int index = grid.CalculateIndexFromCordinates(position.x, position.z);
        return obstacles[index] == false;
    }
    
    private void RandomlyPlaceKnights(int numOfPieces)
    {
        int count = numberOfPieces;
        int knightPlacementTryLimit = 100;
        while(count > 0 && knightPlacementTryLimit > 0)
        {
            int randomIndex = Random.Range(0, obstacles.Length);

            if (!obstacles[randomIndex])
            {
                Vector3 coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                if(coordinates == startPoint || coordinates == endPoint)
                    continue;

                obstacles[randomIndex] = true;
                knightPiecesList.Add(new KnightPiece(coordinates));
                --count;
            }

            --knightPlacementTryLimit;
        }
    }

    public void PlaceObstaclesForKnight(KnightPiece knight)
    {
        for(int i = 0; i < KnightPiece.possibleMoves.Count; ++i)
        {
            Vector3 newPosition = knight.position + KnightPiece.possibleMoves[i];
            if(grid.IsCellValid(newPosition.x, newPosition.z) && PositionCanBeObsitcle(newPosition))
            {
                obstacles[grid.CalculateIndexFromCordinates(newPosition.x, newPosition.z)] = true;
            }
        }
    }

    private void PlaceObstacles()
    {
        for(int i = 0; i < knightPiecesList.Count; ++i)
        {
            PlaceObstaclesForKnight(knightPiecesList[i]);
        }
    }

    public MapData ReturnMapData()
    {
        return new MapData
        {
            obsticles = obstacles,
            knightPieces = knightPiecesList,
            startPosition = startPoint,
            endPosition = endPoint,
            path = path,
            corners = corners,
            cornersNearEachOther = cornersNearEachOther
        };
    }

    public List<Vector3> Repair()
    {
        int numberOfObstacles = obstacles.Where(_ => _).Count();
        List<Vector3> obstaclesToRemove = new List<Vector3>();

        if(path.Count > 0)
            return obstaclesToRemove;

        do
        {
            int obstacleIndexToRemove = Random.Range(0, numberOfObstacles);

            for (int i = 0; i < obstacles.Length; ++i)
            {
                if (obstacles[i]) {
                    if (obstacleIndexToRemove == 0)
                    {
                        obstacles[i] = false;
                        obstaclesToRemove.Add(grid.CalculateCoordinatesFromIndex(i));
                        break;
                    }
                    --obstacleIndexToRemove;
                } 
            }

            FindPath();
        } while (this.path.Count <= 0);

        for(int i = 0; i < obstaclesToRemove.Count; ++i)
        {
            if (path.Contains(obstaclesToRemove[i]))
                continue;

            int index = grid.CalculateIndexFromCoordinates(obstaclesToRemove[i].x, obstaclesToRemove[i].z);
            obstacles[index] = true;
        }

        return obstaclesToRemove;
    }

    public void AddMutation(float mutationRate)
    {
        int numItems = (int)(obstacles.Length * mutationRate);

        while(numItems > 0)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            obstacles[randomIndex] = !obstacles[randomIndex];
            --numItems;
        }
    }

    public void PlaceObstacle(int i, bool isObstacle)
    {
        obstacles[i] = isObstacle;
    }
     
    public bool IsObstacleAt(int i)
    {
        return obstacles[i];
    }

    public CandidateMap DeepClone()
    {
        return new CandidateMap(this);
    }
}
