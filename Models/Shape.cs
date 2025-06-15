using Avalonia;
using Avalonia.Collections;
using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using System.Linq;

namespace DrawMat.Models;

public abstract class ShapeBase
{
    public Rect BoundingBox { get; set; } = new();
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
    
    protected override Rectangle CreateBoundingBoxVisual()
    {
        var rect = base.CreateBoundingBoxVisual();
        rect.Stroke = Brushes.Red;
        rect.StrokeThickness = 1;
        return rect;
    }
    
    public override Control ToControl()
    {
        var canvas = (Canvas)base.ToControl();

        var polyline = new Polyline
        {
            Points = new AvaloniaList<Point>(Points),
            Stroke = Brushes.Black,
            StrokeThickness = StrokeThickness
        };

        canvas.Children.Add(polyline);
        return canvas;
    }
    public void AddPoint(Point next)
    {
        Points.Add(next);
        double left   = Math.Min(BoundingBox.X, next.X);
        double top    = Math.Min(BoundingBox.Y, next.Y);
        double right  = Math.Max(BoundingBox.Right, next.X);
        double bottom = Math.Max(BoundingBox.Bottom, next.Y);

        BoundingBox = new Rect(left, top, right - left, bottom - top);
    }
}

public class GroupShape : ShapeBase
{
    public List<ShapeBase> Children { get; set; } = new();
    public void UpdateBoundingBox()
    {
        Rect? groupBox = null;

        foreach (var child in Children)
        {
            if (groupBox is null)
            {
                groupBox = child.BoundingBox;
            }
            else
            {
                groupBox = Union(groupBox.Value, child.BoundingBox);
            }
        }
        if (groupBox is not null)
        {
            BoundingBox = groupBox.Value;
        }
    }
    
    public override Control ToControl() 
    {
        UpdateBoundingBox();
        var canvas = (Canvas)base.ToControl();

        foreach (var child in Children)
        {
            var control = child.ToControl();
            if (control != null)
            {
                canvas.Children.Add(control);
            }
        }
        return canvas;
    }
    
    private static Rect Union(Rect a, Rect b)
    {
        var left = Math.Min(a.X, b.X);
        var top = Math.Min(a.Y, b.Y);
        var right = Math.Max(a.Right, b.Right);
        var bottom = Math.Max(a.Bottom, b.Bottom);
        return new Rect(left, top, right - left, bottom - top);
    }
}
