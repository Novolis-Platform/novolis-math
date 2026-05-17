namespace Novolis.Math.Arrays;

public readonly record struct GridIndex
{
    public uint X { get; }
    public uint Y { get; }
    public uint Z { get; }

    public GridIndex(uint x, uint y, uint z = 0)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public GridIndex(string x, string y, uint z = 0)
    {
        X = ConvertToZeroBasedIndex(x);
        Y = ConvertToZeroBasedIndex(y);
        Z = z;
    }

    public GridIndex(uint x, string y, uint z = 0)
    {
        X = x;
        Y = ConvertToZeroBasedIndex(y);
        Z = z;
    }

    public GridIndex(string x, uint y, uint z = 0)
    {
        X = ConvertToZeroBasedIndex(x);
        Y = y;
        Z = z;
    }

    private static uint ConvertToZeroBasedIndex(string index)
    {
        var result = 0u;
        foreach (var c in index)
            if (char.IsLetter(c))
                result = result * 26u + (uint)(c - 'a');
            else
                throw new ArgumentException("Invalid character in index");
        return result;
    }
}
