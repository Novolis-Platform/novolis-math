using System.Numerics;
using Novolis.Math.Geometry;

namespace Novolis.Math.Unit.Geometry;

public class SlabIntersectTests
{
    [Test]
    public async Task Ray_hits_unit_cube_center()
    {
        var hit = SlabIntersect.RayBox(
            Vector3.Zero,
            Vector3.One,
            new Vector3(0.5f, 0.5f, -1f),
            Vector3.UnitZ,
            0f,
            float.MaxValue);
        await Assert.That(hit).IsTrue();
    }
}
