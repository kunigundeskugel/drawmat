using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawMat.Models;

public abstract class ShapeBase
{
    public BoundingBox BBox { get; protected set; } = new();
    public static double BoundingBoxStrokeThickness { get; } = 4;
    public Point Origin { get; set; } = new();

    public virtual Control ToControl()
    {
        return new Canvas();
    }

    public virtual Rectangle CreateBoundingBoxVisual()
    {
        if (BBox.IsEmpty) return new Rectangle();
        var rect = new Rectangle
        {
            Stroke = Brushes.Blue,
            StrokeThickness = BoundingBoxStrokeThickness,
            Fill = Brushes.Transparent,
            Width = BBox.Width,
            Height = BBox.Height
        };
        Canvas.SetLeft(rect, BBox.MinX);
        Canvas.SetTop(rect, BBox.MinY);
        return rect;
    }
}

public class PolylineShape : ShapeBase
{
    public List<Point> Points { get; set; } = new();
    public double StrokeThickness { get; set; } = 2.0;
    private Color _strokeColor;

    public PolylineShape(List<Point> points, Color color)
    {
        BBox = new BoundingBox();
        _strokeColor = color;
        foreach (var point in points)
        {
            AddPoint(point);
        }
    }

    public override Rectangle CreateBoundingBoxVisual()
    {
        var rect = base.CreateBoundingBoxVisual();
        rect.Stroke = Brushes.Red;
        return rect;
    }

    public override Control ToControl()
    {
        var canvas = new Canvas();
        var polyline = new Polyline
        {
            Points = new AvaloniaList<Point>(Points),
            Stroke = new SolidColorBrush(_strokeColor),
            StrokeThickness = StrokeThickness
        };
        canvas.Children.Add(polyline);
        canvas.Children.Add(base.ToControl());
        return canvas;
    }

    public void AddPoint(Point next)
    {
        Points.Add(next);
        BBox.Include(new BoundingBox(next, StrokeThickness));
    }
}

public class GroupShape : ShapeBase
{
    public List<ShapeBase> Children { get; set; } = new();

    public void Add(ShapeBase shape)
    {
        Children.Add(shape);
        UpdateBoundingBox();
    }

    public void Add(List<ShapeBase> shapes)
    {
        foreach (var shape in shapes)
        {
            Children.Add(shape);
        }
        UpdateBoundingBox();
    }

    public override Rectangle CreateBoundingBoxVisual()
    {
        UpdateBoundingBox();
        var rect = base.CreateBoundingBoxVisual();
        rect.Stroke = Brushes.Blue;
        return rect;
    }

    public List<ShapeBase> SearchChildren(Point position)
    {
        var hits = new List<ShapeBase>();

        foreach (var child in Children)
        {
            if (child.BBox.Contains(position))
            {
                hits.Add(child);
            }
        }

        return hits;
    }

    public List<ShapeBase> SearchChildren(BoundingBox box)
    {
        var hits = new List<ShapeBase>();

        foreach (var child in Children)
        {
            if (box.Contains(child.BBox))
            {
                hits.Add(child);
            }
        }

        return hits;
    }

    public void UpdateBoundingBox()
    {
        BoundingBox groupBox = new BoundingBox();
        foreach (var child in Children)
        {
            groupBox.Include(child.BBox);
        }
        BBox = groupBox;
    }

    public override Control ToControl()
    {
        var canvas = new Canvas();
        foreach (var child in Children)
        {
            var control = child.ToControl();
            if (control != null)
            {
                canvas.Children.Add(control);
            }
        }
        UpdateBoundingBox();
        canvas.Children.Add(base.ToControl());
        return canvas;
    }
}
