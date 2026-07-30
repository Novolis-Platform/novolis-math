namespace Novolis.Math.Arrays;

/// <summary>Integer chunk coordinate in a 3D voxel lattice.</summary>
public readonly record struct ChunkCoord3(int X, int Y, int Z);

/// <summary>
/// Fixed 16³ voxel chunk with packed <see cref="ushort"/> block ids (0 = air).
/// Local indices are in [0, 16).
/// </summary>
public sealed class VoxelChunk
{
    /// <summary>Edge length in blocks.</summary>
    public const int Size = 16;

    /// <summary>Total cells in a chunk.</summary>
    public const int Volume = Size * Size * Size;

    readonly ushort[] _blocks = new ushort[Volume];

    /// <summary>World chunk coordinate this buffer belongs to.</summary>
    public ChunkCoord3 Coord { get; set; }

    /// <summary>Whether any non-air block is present (lazy flag; call <see cref="Recount"/> after bulk writes).</summary>
    public bool IsEmpty { get; private set; } = true;

    /// <summary>Number of non-air blocks (maintained by set/fill/clear).</summary>
    public int SolidCount { get; private set; }

    /// <summary>Linear index for local (lx, ly, lz).</summary>
    public static int Index(int lx, int ly, int lz) =>
        lx + Size * (lz + Size * ly);

    /// <summary>Gets a block id at local coordinates.</summary>
    public ushort Get(int lx, int ly, int lz)
    {
        ValidateLocal(lx, ly, lz);
        return _blocks[Index(lx, ly, lz)];
    }

    /// <summary>Sets a block id; returns previous value.</summary>
    public ushort Set(int lx, int ly, int lz, ushort id)
    {
        ValidateLocal(lx, ly, lz);
        var i = Index(lx, ly, lz);
        var prev = _blocks[i];
        if (prev == id)
            return prev;
        _blocks[i] = id;
        if (prev == 0 && id != 0)
        {
            SolidCount++;
            IsEmpty = false;
        }
        else if (prev != 0 && id == 0)
        {
            SolidCount--;
            if (SolidCount <= 0)
            {
                SolidCount = 0;
                IsEmpty = true;
            }
        }

        return prev;
    }

    /// <summary>Fills the entire chunk with <paramref name="id"/>.</summary>
    public void Fill(ushort id)
    {
        Array.Fill(_blocks, id);
        if (id == 0)
        {
            SolidCount = 0;
            IsEmpty = true;
        }
        else
        {
            SolidCount = Volume;
            IsEmpty = false;
        }
    }

    /// <summary>Clears to air.</summary>
    public void Clear() => Fill(0);

    /// <summary>Recomputes <see cref="SolidCount"/> / <see cref="IsEmpty"/> from raw data.</summary>
    public void Recount()
    {
        var n = 0;
        for (var i = 0; i < _blocks.Length; i++)
            if (_blocks[i] != 0)
                n++;
        SolidCount = n;
        IsEmpty = n == 0;
    }

    /// <summary>Direct buffer for bulk meshing (length <see cref="Volume"/>).</summary>
    public ReadOnlySpan<ushort> Blocks => _blocks;

    /// <summary>Mutable buffer for bulk fills (caller must <see cref="Recount"/>).</summary>
    public Span<ushort> BlocksMutable => _blocks;

    /// <summary>True when local coords are inside the chunk.</summary>
    public static bool ContainsLocal(int lx, int ly, int lz) =>
        (uint)lx < Size && (uint)ly < Size && (uint)lz < Size;

    static void ValidateLocal(int lx, int ly, int lz)
    {
        if (!ContainsLocal(lx, ly, lz))
            throw new ArgumentOutOfRangeException($"Local ({lx},{ly},{lz}) outside 0..{Size - 1}.");
    }
}
