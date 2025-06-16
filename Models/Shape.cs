using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using DrawMat.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawMat.Models;

public abstract class ShapeBase
{
    public Rect BoundingBox { get; protected set; } = new();
    public Point Origin { get; set; } = new();

    public virtual Control ToControl()
    {
        var canvas = new Canvas();
        var bboxRect = CreateBoundingBoxVisual();
        if (bboxRect != null)
        {
            canvas.Children.Add(bboxRect);
        }
        return canvas;
    }

    protected virtual Rectangle CreateBoundingBoxVisual()
    {
        var rect = new Rectangle
        {
            Stroke = Brushes.Blue,
            StrokeThickness = 2,
            Fill = Brushes.Transparent,
            Width = BoundingBox.Width,
            Height = BoundingBox.Height
        };
        Canvas.SetLeft(rect, BoundingBox.X);
        Canvas.SetTop(rect, BoundingBox.Y);
        return rect;
    }
}

public class PolylineShape : ShapeBase
{
    public double StrokeThickness { get; set; } = 1.0;
    public List<Point> Points { get; set; } = new();

    public PolylineShape(Point start)
    {
        StrokeThickness = 2;
        Points = new List<Point> { start };
        BoundingBox = new Rect(start.X, start.Y, 1, 1);
    }

    protected override Rectangle CreateBoundingBoxVisual()
    {
        var rect = base.CreateBoundingBoxVisual();
        rect.Stroke = Brushes.Red;
        rect.StrokeThickness = 1;
        return rect;
    }

    public override Control ToControl()
    {
        var canvas = new Canvas();
        var polyline = new Polyline
        {
            Points = new AvaloniaList<Point>(Points),
            Stroke = Brushes.Black,
            StrokeThickness = StrokeThickness
        };
        canvas.Children.Add(polyline);
        canvas.Children.Add((Canvas)base.ToControl());
        return canvas;
    }

    public void AddPoint(Point next)
    {
        Points.Add(next);
        BoundingBox = BoundingBox.Union(next);
    }
}

public class GroupShape : ShapeBase
{
    public List<ShapeBase> Children { get; set; } = new();

    public void UpdateBoundingBox()
    {
        if (Children.Count == 0)
        {
            BoundingBox = new Rect();
            return;
        }
        Rect groupBox = Children[0].BoundingBox;
        foreach (var child in Children.Skip(1))
        {
                groupBox = groupBox.Union(child.BoundingBox);
        }
        BoundingBox = groupBox;
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
        canvas.Children.Add((Canvas)base.ToControl());
        return canvas;
    }
}
