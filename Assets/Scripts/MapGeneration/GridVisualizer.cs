using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GameObject ground;

    public void VisualizerGrid(int width, int length)
    {
        Vector3 position = new Vector3(width / 2f, 0, length / 2f);
        Quaternion rotation = Quaternion.Euler(90f, 0, 0);
        GameObject board = Instantiate(ground, position - new Vector3(0, 0.5f, 0), rotation);
        board.transform.localScale = new Vector3(width, length, 1);
    }
}
