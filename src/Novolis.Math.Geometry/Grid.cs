namespace Novolis.Math.Geometry;

public class Grid<T>
{
    private readonly T[,,] _board;

    public Grid(int width, int height, int depth = 1) => _board = new T[width, height, depth];

    public T this[int x, int y, int z]
    {
        get => _board[x, y, z];
        set => _board[x, y, z] = value;
    }

    public T this[LatticePoint position]
    {
        get => _board[position.X, position.Y, position.Z];
        set => _board[position.X, position.Y, position.Z] = value;
    }

    public int Width => _board.GetLength(0);

    public int Height => _board.GetLength(1);

    public int Depth => _board.GetLength(2);

    public void Clear()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        for (var z = 0; z < Depth; z++)
            _board[x, y, z] = default!;
    }
}
