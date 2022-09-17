using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private MapGrid grid;
    private Vector3 startPosition;
    private Vector3 endPosition;

    public GridVisualizer gridVisualizer;
    public Direction startEdge;
    public Direction exitEdge;
    public bool randomPlacement;
    public MapVisualizer mapVisualizer;
    [Range(1, 100)]
    public int numberOfPieces;
    [Range(5, 30)]
    public int width = 10;
    [Range(5, 30)]
    public int length = 10;


    private void Start()
    {
        grid = new MapGrid(width, length);
        gridVisualizer.VisualizerGrid(width, length);
        MapHelper.RandomlyChoseAndSetStartAndExitPoints(grid, ref startPosition, ref endPosition, randomPlacement, startEdge, exitEdge);

        CandidateMap candidateMap = new CandidateMap(grid, numberOfPieces);
        candidateMap.CreateMap(startPosition, endPosition, false);
        mapVisualizer.VisualizeMap(grid, candidateMap.ReturnMapData(), false);
    }
}
