using Avalonia;
using Avalonia.Collections;
using FluentAssertions;
using DrawMat.Models;
using DrawMat.Utilities;
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
        BoundingBox bbox_0 = new BoundingBox(new Point(0, 0));
        bbox_0.IsEmpty.Should().BeFalse();
        bbox_0.MinX.Should().Be(0);
        bbox_0.MinY.Should().Be(0);
        bbox_0.Width.Should().Be(0);
        bbox_0.Height.Should().Be(0);
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
}
