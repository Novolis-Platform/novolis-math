using System.Text;

namespace Novolis.Math.Arrays;

/// <summary>
/// Dense three-dimensional grid backed by a nullable rank-3 array, with numeric and letter indexers.
/// </summary>
/// <typeparam name="T">Cell value type.</typeparam>
public partial class DenseGrid<T>
{
    private readonly T?[,,] _array;

    /// <summary>Number of columns (X extent).</summary>
    public uint Width { get; private set; }

    /// <summary>Number of rows (Y extent).</summary>
    public uint Height { get; private set; }

    /// <summary>Number of depth slices (Z extent).</summary>
    public uint Depth { get; private set; }

    /// <summary>Allocates a grid of the given width, height, and optional depth.</summary>
    /// <param name="width">Column count.</param>
    /// <param name="height">Row count.</param>
    /// <param name="depth">Depth slice count; defaults to <c>1</c>.</param>
    public DenseGrid(uint width, uint height, uint depth = 1)
    {
        Width = width;
        Height = height;
        Depth = depth;
        _array = new T?[height, width, depth];
    }

    /// <summary>
    /// Renders a single depth slice as ASCII: <c>X</c> where a cell is non-null, space otherwise.
    /// </summary>
    /// <param name="z">Depth slice index; defaults to <c>0</c>.</param>
    /// <returns>Multi-line text map.</returns>
    public string GetMap(uint z = 0)
    {
        var map = new bool[Width, Height];
        for (var i = 0u; i < Width; i++)
        for (var j = 0u; j < Height; j++)
            if (_array.TryGetValue(new GridIndex(i, j, z), out var value) && value != null)
                map[i, j] = true;
            else
                map[i, j] = false;

        var stringBuilder = new StringBuilder();
        for (var i = 0u; i < Width; i++)
        {
            for (var j = 0u; j < Height; j++)
                stringBuilder.Append(map[i, j] ? "X" : " ");
            stringBuilder.Append(Environment.NewLine);
        }

        return stringBuilder.ToString();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        for (var z = 0u; z < Depth; z++)
        {
            if (Depth > 1)
                stringBuilder.AppendLine($"z={z}:");
            for (var i = 0u; i < Width; i++)
            {
                for (var j = 0u; j < Height; j++)
                    stringBuilder.Append($"[{this[i, j, z]}] ");
                stringBuilder.Append(Environment.NewLine);
            }
        }

        return stringBuilder.ToString();
    }
}
