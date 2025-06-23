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
    public double StrokeThickness { get; set; } = 0.0;
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
        if (BBox.IsEmpty) return new Rectangle();
        var rect = new Rectangle
        {
            Stroke = Brushes.Blue,
            StrokeThickness = BoundingBoxStrokeThickness,
            Fill = Brushes.Transparent,
            Width = BBox.Width + StrokeThickness,
            Height = BBox.Height + StrokeThickness
        };
        Canvas.SetLeft(rect, BBox.MinX - StrokeThickness/2);
        Canvas.SetTop(rect, BBox.MinY - StrokeThickness/2);
        return rect;
    }
}

public class PolylineShape : ShapeBase
{
    public List<Point> Points { get; set; } = new();

    public PolylineShape(List<Point> points)
    {
        Points = points;
        BBox = new BoundingBox(points);
        StrokeThickness = 2.0;
    }

    protected override Rectangle CreateBoundingBoxVisual()
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
            Stroke = Brushes.Black,
            StrokeThickness = StrokeThickness
        };
        canvas.Children.Add(polyline);
        canvas.Children.Add(base.ToControl());
        return canvas;
    }

    public void AddPoint(Point next)
    {
        Points.Add(next);
        BBox.Include(next);
    }
}

public class GroupShape : ShapeBase
{
    public List<ShapeBase> Children { get; set; } = new();

    public void UpdateBoundingBox()
    {
        BoundingBox groupBox = new BoundingBox();
        foreach (var child in Children)
        {
            groupBox.Include(child.BBox.MinX - child.StrokeThickness/2, child.BBox.MinY - child.StrokeThickness/2);
            groupBox.Include(child.BBox.MaxX + child.StrokeThickness/2, child.BBox.MaxY + child.StrokeThickness/2);
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
