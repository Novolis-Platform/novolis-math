namespace Novolis.Math.Geometry;

/// <summary>Obsolete integer 3D grid; use <see cref="Novolis.Math.Arrays.DenseGrid{T}"/>.</summary>
[Obsolete("Use Novolis.Math.Arrays.DenseGrid<T> and GridIndex for volumetric storage. This type will be removed in a future release.")]
public class Grid<T>
{
    private readonly T[,,] _board;

    /// <summary>Allocates a grid with the given dimensions.</summary>
    /// <param name="width">X extent.</param>
    /// <param name="height">Y extent.</param>
    /// <param name="depth">Z extent; defaults to <c>1</c>.</param>
    public Grid(int width, int height, int depth = 1) => _board = new T[width, height, depth];

    /// <summary>Gets or sets a cell by integer coordinates.</summary>
    /// <param name="x">X index.</param>
    /// <param name="y">Y index.</param>
    /// <param name="z">Z index.</param>
    public T this[int x, int y, int z]
    {
        get => _board[x, y, z];
        set => _board[x, y, z] = value;
    }

    /// <summary>Gets or sets a cell by <see cref="LatticePoint"/>.</summary>
    /// <param name="position">Lattice coordinates.</param>
    public T this[LatticePoint position]
    {
        get => _board[position.X, position.Y, position.Z];
        set => _board[position.X, position.Y, position.Z] = value;
    }

    /// <summary>X extent of the backing array.</summary>
    public int Width => _board.GetLength(0);

    /// <summary>Y extent of the backing array.</summary>
    public int Height => _board.GetLength(1);

    /// <summary>Z extent of the backing array.</summary>
    public int Depth => _board.GetLength(2);

    /// <summary>Sets every cell to <see langword="default"/>.</summary>
    public void Clear()
    {
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        for (var z = 0; z < Depth; z++)
            _board[x, y, z] = default!;
    }
}
