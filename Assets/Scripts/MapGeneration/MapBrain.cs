using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapBrain : MonoBehaviour
{
    [Header("Genetic Algoriytm parameters")] 
    [SerializeField, Range(20, 100)]
    private int populationSize = 20;
    [SerializeField, Range(0, 100)]
    private int crossoverRate = 100;
    private double crossoverRatePercent;
    [SerializeField, Range(0, 100)]
    private int mutationRate = 0;
    private float mutationRatePercent;
    [SerializeField, Range(1, 100)]
    private int generationLimit = 10;

    // Algorytm variables
    private List<CandidateMap> currentGeneration = new List<CandidateMap>();
    private int totalFitnessThisGeneration;
    private int bestFitnessScoreAllTime = 0;
    private CandidateMap bestMap = null;
    private int bestMapGenerationNumber = 0;
    private int generationNumber = 1;

    [Header("fitness parameters")]
    [SerializeField]
    private int fitnessCornerMin = 6;
    [SerializeField]
    private int fitnessCornerMax = 9;
    [SerializeField, Range(1, 3)]
    private int fitnessCornerWeight = 1;
    [SerializeField, Range(1, 3)]
    private int fitnessNearCornerWeight = 1;
    [SerializeField, Range(1, 5)]
    private int fitnessPathWeight = 1;
    [SerializeField, Range(0.3f, 1f)]
    private float fitnessObstacleCornerWeight = 1;

    [Header("Map start parameters")]
    [SerializeField, Range(3, 30)]
    private int mapWidth = 11;
    [SerializeField, Range(3, 30)]
    private int mapLength = 11;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private MapGrid grid;
    [SerializeField]
    private Direction startPositionEdge = Direction.Left;
    [SerializeField]
    private Direction exitPositionEdge = Direction.Right;
    [SerializeField]
    private bool randomStartAndEnd = false;
    [SerializeField]
    private int NumberOfKnightPieces = 7;

    [Header("GridVisualizer grid")]
    public MapVisualizer mapVisualizer;
    private DateTime endDate;
    private DateTime startDate;
    private bool isAlgorythmRunning = false;

    public bool IsAlgorythmRunning { get => isAlgorythmRunning; }

    private void Start()
    {
        mutationRatePercent = mutationRate / 100f;
        crossoverRatePercent = crossoverRate / 100f;

        RunAlgorythm();
    }

    public void RunAlgorythm()
    {
        UIController.Instance.ResetScreen();
        ResetAlgorythmVariables();
        mapVisualizer.ClearMap();

        grid = new MapGrid(mapWidth, mapLength);

        MapHelper.RandomlyChoseAndSetStartAndExitPoints(grid, ref startPosition, ref endPosition, randomStartAndEnd, startPositionEdge, exitPositionEdge);

        isAlgorythmRunning = true;
        startDate = DateTime.Now;

        FindOptimalSolution(grid);

    }

    private void ResetAlgorythmVariables()
    {
        totalFitnessThisGeneration = 0;
        bestFitnessScoreAllTime = 0;
        bestMap = null;
        bestMapGenerationNumber = 0;
        generationNumber = 0;
    }

    private void FindOptimalSolution(MapGrid grid)
    {
        currentGeneration = new List<CandidateMap>(populationSize);
        for(int i = 0; i < populationSize; ++i)
        {
            CandidateMap candidateMap = new CandidateMap(grid, NumberOfKnightPieces);
            candidateMap.CreateMap(startPosition, endPosition, true);
            currentGeneration.Add(candidateMap);
        }

        StartCoroutine(GeneticAlgorythm());
    }

    private IEnumerator GeneticAlgorythm()
    {
        totalFitnessThisGeneration = 0;
        int bestFitnessScoreThisGeneration = 0;
        CandidateMap bestMapThisGeneration = null;

        for(int i = 0; i < currentGeneration.Count; ++i)
        {
            currentGeneration[i].FindPath();
            currentGeneration[i].Repair();
            int fitness = CalculateFitness(currentGeneration[i].ReturnMapData());

            totalFitnessThisGeneration += fitness;

            if(fitness > bestFitnessScoreThisGeneration)
            {
                bestFitnessScoreThisGeneration = fitness;
                bestMapThisGeneration = currentGeneration[i];
            }
        }

        if(bestFitnessScoreThisGeneration > bestFitnessScoreAllTime)
        {
            bestFitnessScoreAllTime = bestFitnessScoreThisGeneration;
            bestMap = bestMapThisGeneration.DeepClone();
            bestMapGenerationNumber = generationNumber;    
        }

        ++generationNumber;
        UIController.Instance.SetLoadingValue(generationNumber / (float)generationLimit);
        yield return new WaitForEndOfFrame();

        Debug.Log("best generation number: " + generationNumber + " best fitness score current generation: " + bestMapThisGeneration);

        if(generationNumber < generationLimit)
        {
            List<CandidateMap> nextGeneration = new List<CandidateMap>();

            while (nextGeneration.Count < populationSize)
            {
                CandidateMap parent1 = currentGeneration[RouletteWheelSelection()];
                CandidateMap parent2 = currentGeneration[RouletteWheelSelection()];

                CandidateMap child1, child2;

                CrossOverParents(parent1, parent2, out child1, out child2);

                child1.AddMutation(mutationRatePercent);
                child2.AddMutation(mutationRatePercent);

                nextGeneration.Add(child1);
                nextGeneration.Add(child2);
            }

            currentGeneration = nextGeneration;

            StartCoroutine(GeneticAlgorythm());
        }
        else 
            ShowResults();
    }

    private void ShowResults()
    {
        isAlgorythmRunning = false;
        Debug.Log("best solution: " + bestMapGenerationNumber + " with score: " + bestFitnessScoreAllTime);

        MapData data = bestMap.ReturnMapData();
        mapVisualizer.VisualizeMap(grid, data, true);

        UIController.Instance.HideLoadingScreen();

        Debug.Log("path length: " + data.path);
        Debug.Log("corners count: " + data.corners.Count);
        
        endDate = DateTime.Now;
        long elapsedTime = endDate.Ticks - startDate.Ticks;
        TimeSpan elapsedSpan = new TimeSpan(elapsedTime);
        Debug.Log("Time to run: " + elapsedSpan.TotalSeconds);
    }

    private void CrossOverParents(CandidateMap parent1, CandidateMap parent2, out CandidateMap child1, out CandidateMap child2)
    {
        child1 = parent1.DeepClone();
        child2 = parent2.DeepClone();

        if(Random.value < crossoverRatePercent)
        {
            int numBIts = parent1.obstacles.Length;
            int crossoverIndex = Random.Range(0, numBIts);

            for(int i = 0; i < crossoverIndex; i++)
            {
                child1.PlaceObstacle(i, parent2.IsObstacleAt(i));
                child2.PlaceObstacle(i, parent1.IsObstacleAt(i));
            }
        }
    }

    private int RouletteWheelSelection()
    {
        int randomValue = Random.Range(0, totalFitnessThisGeneration);

        for(int i = 0; i < populationSize; ++i)
        {
            randomValue -= CalculateFitness(currentGeneration[i].ReturnMapData());
            if (randomValue <= 0)
                return i;
        }

        return populationSize-1;
    }

    private int CalculateFitness(MapData mapData)
    {
        int numberOfObstacles = mapData.obsticles.Where(_ => _).Count();
        int score = mapData.path.Count * fitnessPathWeight + (int)(numberOfObstacles * fitnessObstacleCornerWeight);
        int cornersCount = mapData.corners.Count;

        if (cornersCount >= fitnessCornerMin && cornersCount <= fitnessCornerMax)
            score += cornersCount * fitnessCornerWeight;
        else if (cornersCount > fitnessCornerMax)
            score -= fitnessCornerWeight * (cornersCount - fitnessCornerMax);
        else if (cornersCount > fitnessCornerMin)
            score -= fitnessCornerWeight * fitnessCornerMin;

        score -= mapData.cornersNearEachOther * fitnessNearCornerWeight;

        return score;
    }
}
