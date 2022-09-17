public class Cell
{
    public int x;
    public int z;

    public bool isTaken;
    public CellObjectType objectType;

    public Cell(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.isTaken = false;
        this.objectType = CellObjectType.Empty;
    }
}

public enum CellObjectType
{
    Empty,
    Road,
    Obsticle,
    Start,
    Exit
}