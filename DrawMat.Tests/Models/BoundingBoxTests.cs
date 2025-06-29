using Avalonia;
using Avalonia.Collections;
using FluentAssertions;
using DrawMat.Models;
using Xunit;
namespace DrawMat.Tests.Models;

public class BoundingBoxTests
{
    [Fact]
    public void DefaultBoundingBox_ShouldBeEmpty()
    {
        BoundingBox bbox = new BoundingBox();
        bbox.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void BoundingBoxWithZero_ShouldNotBeEmpty()
    {
        BoundingBox bbox = new BoundingBox(new Point(0, 0));
        bbox.MinX.Should().Be(0);
        bbox.MinY.Should().Be(0);
        bbox.Width.Should().Be(0);
        bbox.Height.Should().Be(0);
        bbox.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void BoundingBoxWithPoints_ShouldNotBeEmpty()
    {
        BoundingBox bbox = new BoundingBox();
        bbox.Include(new Point(5, 3));
        bbox.MinX.Should().Be(5);
        bbox.MinY.Should().Be(3);
        bbox.Width.Should().Be(0);
        bbox.Height.Should().Be(0);
        bbox.IsEmpty.Should().BeFalse();

        bbox.Include(new Point(3, 7));
        bbox.MinX.Should().Be(3);
        bbox.MinY.Should().Be(3);
        bbox.Width.Should().Be(2);
        bbox.Height.Should().Be(4);
        bbox.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void BoundingBoxContains_WhenBoxIsStrictlyInside_ShouldReturnTrue()
    {
        var outer = new BoundingBox(0, 0, 10, 10);
        var inner = new BoundingBox(2, 2, 8, 8);

        outer.Contains(inner).Should().BeTrue();
    }

    [Fact]
    public void BoundingBoxContains_WhenBoxTouchesEdges_ShouldReturnTrue()
    {
        var outer = new BoundingBox(0, 0, 10, 10);
        var touching = new BoundingBox(0, 0, 10, 10);

        outer.Contains(touching).Should().BeTrue();
    }

    [Fact]
    public void BoundingBoxContains_WhenBoxExtendsBeyondRightEdge_ShouldReturnFalse()
    {
        var outer = new BoundingBox(0, 0, 10, 10);
        var outside = new BoundingBox(2, 2, 12, 8);

        outer.Contains(outside).Should().BeFalse();
    }

    [Fact]
    public void BoundingBoxContains_WhenBoxIsLargerThanOuter_ShouldReturnFalse()
    {
        var outer = new BoundingBox(0, 0, 10, 10);
        var tooBig = new BoundingBox(-5, -5, 15, 15);

        outer.Contains(tooBig).Should().BeFalse();
    }

    [Fact]
    public void BoundingBoxContains_WhenBoxIsPartiallyOutsideTopLeft_ShouldReturnFalse()
    {
        var outer = new BoundingBox(0, 0, 10, 10);
        var cornerOut = new BoundingBox(-1, -1, 5, 5);

        outer.Contains(cornerOut).Should().BeFalse();
    }
}
