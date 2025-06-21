using Xunit;
using FluentAssertions;
using Avalonia;
using DrawMat.Models;
using DrawMat.Utilities;
namespace DrawMat.Tests.Models;

public class GroupShapeTests
{
    [Fact]
    public void UpdateBoundingBox_ShouldNotBeEmpty()
    {
        var a = new Rect();

        a.X.Should().BeLessThanOrEqualTo(0);
        a.Y.Should().BeLessThanOrEqualTo(0);
        a.Right.Should().BeLessThanOrEqualTo(0);
        a.Bottom.Should().BeLessThanOrEqualTo(0);
        a.IsEmpty().Should().BeTrue();
        a.Contains(new Point(8, 8)).Should().BeFalse();

        var b = new Rect(8, 8, 0, 0);

        b.X.Should().Be(8);
        b.Y.Should().Be(8);
        b.Width.Should().Be(0);
        b.Height.Should().Be(0);
        b.IsEmpty().Should().BeFalse();
        b.Contains(new Point(8, 8)).Should().BeTrue();

        var c = a.Union(b);

        c.Should().Be(b);

        var d = new Point(3, 6);

        c.Union(d).Should().Be(new Rect(3, 6, 5, 2));
    }
}
