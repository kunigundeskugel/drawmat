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
    public abstract Control ToControl();
    public Point Origin { get; set; } = new();
}


public class PolylineShape : ShapeBase
{
    public double StrokeThickness { get; set; } = 1.0;
    public List<Point> Points { get; set; } = new();
    
    public override Control ToControl() 
    {
        return new Polyline
        {
                Points = new AvaloniaList<Point>(Points),
                Stroke = Brushes.Black,
                StrokeThickness = StrokeThickness
       };
    }
}

public class GroupShape : ShapeBase
{
    public List<ShapeBase> Children { get; set; } = new();
    
    public override Control ToControl() 
    {
        var panel = new Canvas();

        foreach (var child in Children)
        {
            var control = child.ToControl();
            if (control != null)
            {
                panel.Children.Add(control);
            }
        }
        return panel;
    }
}
