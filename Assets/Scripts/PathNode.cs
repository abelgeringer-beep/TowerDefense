using UnityEngine;

public class PathNode
{
    public GameObject node;
    
    public int x;
    public int y;
    
    public int GCost;
    public int FCost;
    public int HCost;

    public PathNode Prev;

    public PathNode(GameObject node)
    {
        var position = node.transform.position;
        x = (int) position.x;
        y = (int) position.z;
        this.node = node;
    }

    public override string ToString()
    {
        return node.transform.position.x + "," + node.transform.position.y;
    }

    public void CalcFCost()
    {
        FCost = GCost + HCost;
    }
}