using Avalonia;
using System;
using System.Collections.Generic;

namespace DrawMat.Models;

public abstract class MyNode
{
    public string Type { get; set; } = "";
    public string? Id { get; set; }
    public Point? Position { get; set; } = new();
}

public class MyShape : MyNode
{
    public string? FillColor { get; set; }
    public string? StrokeColor { get; set; }
    public double StrokeThickness { get; set; } = 1.0;

    public List<Point>? Points { get; set; }
}

public class MyGroup : MyNode
{
    public List<MyNode> Children { get; set; } = new();
}
