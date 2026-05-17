using System.Text;

namespace Novolis.Math.Arrays;

public partial class DenseGrid<T>
{
    private readonly T?[,,] _array;

    public uint Width { get; private set; }
    public uint Height { get; private set; }
    public uint Depth { get; private set; }

    public DenseGrid(uint width, uint height, uint depth = 1)
    {
        Width = width;
        Height = height;
        Depth = depth;
        _array = new T?[height, width, depth];
    }

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
