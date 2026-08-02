using Novolis.Math.Arrays;

namespace Novolis.Math.Unit.Arrays;

public sealed class VoxelChunkTests
{
    [Test]
    public async Task Set_Get_RoundTrip()
    {
        var chunk = new VoxelChunk { Coord = new ChunkCoord3(1, 0, -2) };
        await Assert.That(chunk.IsEmpty).IsTrue();
        chunk.Set(3, 4, 5, 42);
        await Assert.That(chunk.Get(3, 4, 5)).IsEqualTo((ushort)42);
        await Assert.That(chunk.SolidCount).IsEqualTo(1);
        await Assert.That(chunk.IsEmpty).IsFalse();
        chunk.Set(3, 4, 5, 0);
        await Assert.That(chunk.IsEmpty).IsTrue();
    }

    [Test]
    public async Task Fill_And_Clear()
    {
        var chunk = new VoxelChunk();
        chunk.Fill(7);
        await Assert.That(chunk.SolidCount).IsEqualTo(VoxelChunk.Volume);
        chunk.Clear();
        await Assert.That(chunk.IsEmpty).IsTrue();
    }

    [Test]
    public async Task Index_Is_Stable()
    {
        await Assert.That(VoxelChunk.Index(0, 0, 0)).IsEqualTo(0);
        await Assert.That(VoxelChunk.Index(1, 0, 0)).IsEqualTo(1);
        await Assert.That(VoxelChunk.ContainsLocal(15, 15, 15)).IsTrue();
        await Assert.That(VoxelChunk.ContainsLocal(16, 0, 0)).IsFalse();
    }

    [Test]
    public async Task Set_NoOp_WhenSameId()
    {
        var chunk = new VoxelChunk();
        chunk.Set(1, 1, 1, 5);
        var prev = chunk.Set(1, 1, 1, 5);
        await Assert.That(prev).IsEqualTo((ushort)5);
        await Assert.That(chunk.SolidCount).IsEqualTo(1);
    }

    [Test]
    public async Task Recount_AfterBulkWrite()
    {
        var chunk = new VoxelChunk();
        chunk.BlocksMutable[0] = 3;
        chunk.BlocksMutable[10] = 4;
        chunk.Recount();
        await Assert.That(chunk.SolidCount).IsEqualTo(2);
        await Assert.That(chunk.IsEmpty).IsFalse();
    }

    [Test]
    public async Task Get_OutOfRange_Throws()
    {
        var chunk = new VoxelChunk();
        var act = () => chunk.Get(16, 0, 0);
        await Assert.That(act).Throws<ArgumentOutOfRangeException>();
    }
}
