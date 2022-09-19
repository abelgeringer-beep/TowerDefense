using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private MapGrid grid;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private CandidateMap candidateMap;

    public GridVisualizer gridVisualizer;
    public Direction startEdge;
    public Direction exitEdge;
    public bool randomPlacement;
    public bool VisualizeUsingPrefabs = false;
    public bool autoRepair = true;
    public MapVisualizer mapVisualizer;
    [Range(1, 100)]
    public int numberOfPieces;
    [Range(5, 30)]
    public int width = 10;
    [Range(5, 30)]
    public int length = 10;


    private void Start()
    {
        gridVisualizer.VisualizerGrid(width, length);
        GenerateNewMap();
    }

    public void GenerateNewMap()
    {
        mapVisualizer.ClearMap();

        grid = new MapGrid(width, length);

        MapHelper.RandomlyChoseAndSetStartAndExitPoints(grid, ref startPosition, ref endPosition, randomPlacement, startEdge, exitEdge);

        candidateMap = new CandidateMap(grid, numberOfPieces);
        candidateMap.CreateMap(startPosition, endPosition, autoRepair);
        mapVisualizer.VisualizeMap(grid, candidateMap.ReturnMapData(), VisualizeUsingPrefabs);
    }

    public void TryRepair()
    {
        if (candidateMap == null)
            return;

        List<Vector3> listOfObstaclesToRemove = candidateMap.Repair();

        if (listOfObstaclesToRemove.Count <= 0)
            return;
        
        mapVisualizer.ClearMap();
        mapVisualizer.VisualizeMap(grid, candidateMap.ReturnMapData(), VisualizeUsingPrefabs);
    }
}
