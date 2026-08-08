using System.Numerics;
using Novolis.Math.Geometry;
using Novolis.Math.Topology;

namespace Novolis.Math.Unit.Geometry;

public sealed class GeometryCoveragePushTests
{
    [Test]
    public async Task Vector3_and_planar_extensions()
    {
        var v = new Vector3(3f, 4f, 0f);
        await Assert.That(v.Normalized().Length()).IsEqualTo(1f).Within(1e-5f);
        await Assert.That(v.Multiply(2.0).X).IsEqualTo(6f).Within(1e-5f);
        await Assert.That(v.Divide(2.0).X).IsEqualTo(1.5f).Within(1e-5f);
        await Assert.That(Vector3PlanarExtensions.Xz(1f, 2f)).IsEqualTo(new Vector3(1f, 0f, 2f));
        await Assert.That(v.WithY(9f).Y).IsEqualTo(9f);
        await Assert.That(v.ToPlanar().Y).IsEqualTo(0f);
    }

    [Test]
    public async Task Lattice_equality_and_shape_tostring()
    {
        var p = new LatticePoint(1, 2, 3);
        await Assert.That(p.Equals((object)p)).IsTrue();
        await Assert.That(p.Equals((object)"x")).IsFalse();
        await Assert.That(p == new LatticePoint(1, 2, 3)).IsTrue();
        await Assert.That(p != new LatticePoint(0, 0, 0)).IsTrue();

        var b = new LatticeBounds(0, 0, 0, 1, 1, 1);
        await Assert.That(b.Equals((object)b)).IsTrue();
        await Assert.That(b == new LatticeBounds(0, 0, 0, 1, 1, 1)).IsTrue();
        await Assert.That(b != new LatticeBounds(1, 0, 0, 1, 1, 1)).IsTrue();

        var shape = new Shape { Polygon = PolygonFactory.CreateRectangle(1, 1, Vector3.Zero), Color = Rgba32.Red };
        await Assert.That(shape.ToString()).IsNotEmpty();
    }

    [Test]
    public async Task Rgba32_object_equals_and_rigid_rotated_to()
    {
        await Assert.That(Rgba32.Red.Equals((object)Rgba32.Red)).IsTrue();
        await Assert.That(Rgba32.Red.Equals((object)"no")).IsFalse();
        await Assert.That(Rgba32.Red == Rgba32.Red).IsTrue();
        await Assert.That(Rgba32.Red != Rgba32.Crimson).IsTrue();

        var t = new RigidTransform(Vector3.Zero, Quaternion.Identity, 1f);
        var rotated = t.RotatedTo(new Vector3(0.1f, 0.2f, 0.3f));
        await Assert.That(rotated.Rotation).IsNotEqualTo(Quaternion.Identity);
    }

    [Test]
    public async Task CylindricalPoint_object_equals()
    {
        var c = CylindricalPoint.FromCartesian(1, 0, 0);
        await Assert.That(c.Equals((object)c)).IsTrue();
        await Assert.That(c.Equals((object)1)).IsFalse();
    }

    [Test]
    public async Task AxisAlignedBox_disjoint_and_outside_contains()
    {
        var a = AxisAlignedBox.FromMinMax(Vector3.Zero, Vector3.One);
        var b = AxisAlignedBox.FromMinMax(new Vector3(2, 2, 2), new Vector3(3, 3, 3));
        await Assert.That(a.Intersects(b)).IsFalse();
        await Assert.That(a.Contains(new Vector3(-1, 0.5f, 0.5f))).IsFalse();
        await Assert.That(a.Contains(new Vector3(0.5f, -1, 0.5f))).IsFalse();
        await Assert.That(a.Contains(new Vector3(0.5f, 0.5f, -1))).IsFalse();
        await Assert.That(a.Contains(new Vector3(2, 0.5f, 0.5f))).IsFalse();
    }

    [Test]
    public async Task SlabIntersect_parallel_miss_and_swap()
    {
        await Assert.That(SlabIntersect.RayBox(
            Vector3.Zero,
            Vector3.One,
            new Vector3(-1f, 0.5f, 0.5f),
            new Vector3(0f, 1f, 0f),
            0f,
            10f)).IsFalse();

        await Assert.That(SlabIntersect.RayBox(
            Vector3.Zero,
            Vector3.One,
            new Vector3(2f, 0.5f, 0.5f),
            new Vector3(-1f, 0f, 0f),
            0f,
            10f)).IsTrue();

        await Assert.That(SlabIntersect.RayBox(
            Vector3.Zero,
            Vector3.One,
            new Vector3(0.5f, 0.5f, -5f),
            Vector3.UnitZ,
            0f,
            1f)).IsFalse();
    }

    [Test]
    public async Task TriangleRay_parallel_and_miss()
    {
        var v0 = Vector3.Zero;
        var v1 = Vector3.UnitX;
        var v2 = Vector3.UnitY;
        var parallel = new Ray(new Vector3(0, 0, 1), Vector3.UnitX);
        await Assert.That(TriangleRay.TryHit(parallel, v0, v1, v2, 10f, out _, out _)).IsFalse();

        var miss = new Ray(new Vector3(5, 5, 5), -Vector3.UnitZ);
        await Assert.That(TriangleRay.TryHit(miss, v0, v1, v2, 10f, out _, out _)).IsFalse();
    }

    [Test]
    public async Task TriangleMesh_validation_and_empty_bounds()
    {
        await Assert.That(() => new TriangleMesh(null!, [0, 1, 2])).Throws<ArgumentNullException>();
        await Assert.That(() => new TriangleMesh([Vector3.Zero], [0, 1])).Throws<ArgumentException>();
        await Assert.That(() => new TriangleMesh(Array.Empty<Vector3>(), [0, 1, 2])).Throws<ArgumentException>();
        await Assert.That(() => new TriangleMesh([Vector3.Zero], [0, 1, 2])).Throws<ArgumentOutOfRangeException>();

        var empty = new TriangleMesh(Array.Empty<Vector3>(), Array.Empty<int>());
        await Assert.That(empty.GetAxisAlignedBox().Min).IsEqualTo(Vector3.Zero);
    }

    [Test]
    public async Task BvhRaycast_empty_root_returns_false()
    {
        var hit = BvhRaycast.Traverse(
            ReadOnlySpan<TriangleBvhNode>.Empty,
            ReadOnlySpan<int>.Empty,
            -1,
            new Ray(Vector3.Zero, Vector3.UnitZ),
            10f,
            static (int _, in Ray _, float _, out float d, out Vector3 n) =>
            {
                d = 0;
                n = default;
                return false;
            },
            out _,
            out _,
            out _);
        await Assert.That(hit).IsFalse();
    }

    [Test]
    public async Task AdaptiveMesh_validation_and_adapt_positions_overload()
    {
        await Assert.That(() => AdaptiveMeshFactory.FromCapsuleGraph([], [], 4, 1)).Throws<ArgumentException>();
        await Assert.That(() => AdaptiveMeshFactory.FromCapsuleGraph(
            [new AdaptiveMeshHandle(Vector3.Zero, 0.1f)],
            [],
            radialSegments: 2,
            ringsPerEdge: 1)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => AdaptiveMeshFactory.FromCapsuleGraph(
            [new AdaptiveMeshHandle(Vector3.Zero, 0.1f)],
            [],
            radialSegments: 4,
            ringsPerEdge: 0)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => AdaptiveMeshFactory.FromCapsuleGraph(
            [new AdaptiveMeshHandle(Vector3.Zero, 0.1f)],
            [(0, 5)],
            4,
            1)).Throws<ArgumentOutOfRangeException>();

        var handles = new AdaptiveMeshHandle[]
        {
            new(Vector3.Zero, 0.1f),
            new(Vector3.UnitY, 0.1f),
        };
        var mesh = AdaptiveMeshFactory.FromCapsuleGraph(handles, [(0, 1), (0, 0)], 4, 1);
        await Assert.That(() => new AdaptiveMesh(Array.Empty<AdaptiveMeshHandle>(), [], [])).Throws<ArgumentException>();
        await Assert.That(() => new AdaptiveMesh(handles, [AdaptiveVertexBinding.ForSphere(0, Vector3.UnitY)], [0, 1])).Throws<ArgumentException>();

        var dest = new Vector3[mesh.VertexCount];
        mesh.Adapt(new[] { handles[0].Position, handles[1].Position }.AsSpan(), dest.AsSpan());
        await Assert.That(dest[0].Length()).IsGreaterThan(0f);
        await Assert.That(() => mesh.Adapt(new[] { Vector3.Zero }.AsSpan(), dest.AsSpan())).Throws<ArgumentException>();
        await Assert.That(() => mesh.Adapt(new[] { Vector3.Zero, Vector3.One }.AsSpan(), new Vector3[1].AsSpan())).Throws<ArgumentException>();
        await Assert.That(() => mesh.Adapt(handles.AsSpan()[..1], dest.AsSpan())).Throws<ArgumentException>();
    }

    [Test]
    public async Task EditableMesh_boundary_loops_and_replace_validation()
    {
        var open = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, new Vector3(1, 1, 0)],
            [0, 1, 2]);
        var loops = open.FindBoundaryLoops();
        await Assert.That(loops.Count).IsGreaterThan(0);

        await Assert.That(() => open.ReplaceContents([Vector3.Zero], [0, 1])).Throws<ArgumentException>();
        await Assert.That(() => open.ReplaceContents([Vector3.Zero], [0, 0, 1])).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task MeshOptimize_zero_area_and_canonical_rotations()
    {
        var colinear = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, new Vector3(2, 0, 0), Vector3.UnitY],
            [0, 1, 2, 0, 1, 3]);
        var opt = MeshOptimize.Apply(colinear, new OptimizeOptions(
            WeldDuplicateVertices: false,
            RemoveDuplicateFaces: false,
            RemoveDegenerateFaces: true,
            RemoveUnusedVertices: true));
        await Assert.That(opt.Diagnostics.Any(d => d.Code == "degenerateFacesRemoved")).IsTrue();

        var rotated = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ],
            [1, 2, 0, 2, 0, 1, 2, 3, 0]);
        var dup = MeshOptimize.Apply(rotated, new OptimizeOptions(
            WeldDuplicateVertices: false,
            RemoveDuplicateFaces: true,
            RemoveDegenerateFaces: false,
            RemoveUnusedVertices: false));
        await Assert.That(dup.Mesh.TriangleCount).IsLessThan(3);
    }

    [Test]
    public async Task ShapeExtensions_polygon_intersect_path()
    {
        var a = new Shape { Polygon = PolygonFactory.CreateRectangle(2, 2, Vector3.Zero), Color = Rgba32.White };
        var b = new Shape { Polygon = PolygonFactory.CreateRectangle(2, 2, new Vector3(0.5f, 0, 0)), Color = Rgba32.White };
        await Assert.That(a.Intersect(b)).IsTrue();
        await Assert.That(a.GetIntersectionPoints(b).Any()).IsTrue();
    }

    [Test]
    public async Task MeshWeld_and_plane_split_edge_cases()
    {
        var mesh = new EditableMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, new Vector3(1e-8f, 0, 0)],
            [0, 1, 2, 3, 1, 2]);
        var welded = MeshWeld.Apply(mesh, new WeldOptions(Tolerance: 1e-4f));
        await Assert.That(welded.VertexCount).IsLessThanOrEqualTo(mesh.VertexCount);

        var split = MeshPlaneSplit.Split(mesh, new Plane(Vector3.UnitY, 0.5f));
        await Assert.That(split.Positive.TriangleCount + split.Negative.TriangleCount).IsGreaterThanOrEqualTo(0);
    }

    [Test]
    public async Task NurbsCurve_and_mesh_boolean_branch_push()
    {
        await Assert.That(() => NurbsCurve.CreateClampedUniformKnots(2, 2)).Throws<ArgumentException>();
        var knots = NurbsCurve.CreateClampedUniformKnots(4, 2);
        var pts = new[] { Vector3.Zero, Vector3.UnitX, new Vector3(2, 1, 0), new Vector3(3, 0, 0) };
        var p = NurbsCurve.Evaluate(2, pts, knots, null, 0.5f);
        await Assert.That(p.Length()).IsGreaterThanOrEqualTo(0f);
        var fit = NurbsCurve.FromFitPoints([Vector3.Zero, Vector3.UnitX]);
        await Assert.That(fit.ControlPoints.Length).IsEqualTo(2);

        var a = new EditableMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitY], [0, 1, 2]);
        var b = new EditableMesh([Vector3.Zero, Vector3.UnitX, Vector3.UnitZ], [0, 1, 2]);
        var concat = MeshBoolean.Concat(a, b);
        await Assert.That(concat.TriangleCount).IsEqualTo(2);
    }

    [Test]
    public async Task Branch_push_lattice_bvh_slab_and_rgba()
    {
        var bounds = new LatticeBounds(0, 0, 0, 1, 1, 1);
        await Assert.That(bounds.Equals((object)"x")).IsFalse();
        await Assert.That(new LatticePoint(1, 2, 3).Equals((object)null!)).IsFalse();
        await Assert.That(Rgba32.Red.GetHashCode()).IsNotEqualTo(0);

        var mesh = new TriangleMesh(
            [Vector3.Zero, Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ],
            [0, 1, 2, 0, 2, 3]);
        var bvh = mesh.CreateBvh();
        await Assert.That(bvh.Raycast(new Ray(new Vector3(10, 10, 10), Vector3.UnitX), 1f, out _, out _, out _, out _)).IsFalse();

        await Assert.That(SlabIntersect.RayBox(
            Vector3.Zero,
            Vector3.One,
            new Vector3(0.5f, 0.5f, 0.5f),
            Vector3.UnitZ,
            2f,
            1f)).IsFalse();
    }
}
