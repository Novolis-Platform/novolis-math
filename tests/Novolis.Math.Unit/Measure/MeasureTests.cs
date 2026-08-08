using TUnit.Core;

namespace Novolis.Math.Measure.Tests;

public sealed class MeasureTests
{
    [Test]
    public async Task LengthUnits_round_trip_inches_and_mm()
    {
        var sixIn = LengthUnits.FromInches(6f);
        await Assert.That(sixIn.Points).IsEqualTo(432f);
        await Assert.That(sixIn.Inches).IsEqualTo(6f);

        var a5Width = LengthUnits.FromMillimeters(148f);
        await Assert.That(a5Width.Millimeters).IsEqualTo(148f).Within(0.01f);
    }

    [Test]
    public async Task Size_and_Thickness_helpers()
    {
        var size = new Size(LengthUnits.FromInches(6f), LengthUnits.FromInches(9f));
        await Assert.That(size.Width.Points).IsEqualTo(432f);
        await Assert.That(size.Height.Points).IsEqualTo(648f);

        var t = Thickness.Uniform(LengthUnits.FromInches(0.5f));
        await Assert.That(t.Horizontal.Points).IsEqualTo(72f);
        await Assert.That(t.Vertical.Points).IsEqualTo(72f);
    }

    [Test]
    public async Task Rect_Inset_shrinks_content_box()
    {
        var page = Rect.FromSize(
            LengthUnits.FromPoints(0),
            LengthUnits.FromPoints(0),
            new Size(LengthUnits.FromInches(6f), LengthUnits.FromInches(9f)));
        var margin = Thickness.Symmetric(LengthUnits.FromInches(1f), LengthUnits.FromInches(1f));
        var content = page.Inset(margin);

        await Assert.That(content.X.Points).IsEqualTo(72f);
        await Assert.That(content.Y.Points).IsEqualTo(72f);
        await Assert.That(content.Width.Points).IsEqualTo(432f - 144f);
        await Assert.That(content.Height.Points).IsEqualTo(648f - 144f);
        await Assert.That(content.Right.Points).IsEqualTo(content.X.Points + content.Width.Points);
    }
}
