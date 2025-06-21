using Avalonia;
using Avalonia.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawMat.Models;

public class BoundingBox
{
    public double MinX { get; private set; }
    public double MinY { get; private set; }
    public double MaxX { get; private set; }
    public double MaxY { get; private set; }

    public double Width => MaxX - MinX;
    public double Height => MaxY - MinY;

    public bool IsEmpty => Width < 0 || Height < 0;

    public BoundingBox()
    {
        MinX = MinY = double.PositiveInfinity;
        MaxX = MaxY = double.NegativeInfinity;
    }

    public BoundingBox(double minX, double minY, double maxX, double maxY)
    {
        MinX = minX;
        MinY = minY;
        MaxX = maxX;
        MaxY = maxY;
    }

    public BoundingBox(Point p)
    {
        MinX = MinY = double.PositiveInfinity;
        MaxX = MaxY = double.NegativeInfinity;
        Include(p.X, p.Y);
    }

    public BoundingBox(IEnumerable<Point> points)
    {
        MinX = MinY = double.PositiveInfinity;
        MaxX = MaxY = double.NegativeInfinity;
        Include(points);
    }

    public void Include(double x, double y)
    {
        MinX = Math.Min(MinX, x);
        MinY = Math.Min(MinY, y);
        MaxX = Math.Max(MaxX, x);
        MaxY = Math.Max(MaxY, y);
    }

    public void Include(Point p)
    {
        Include(p.X, p.Y);
    }

    public void Include(IEnumerable<Point> points)
    {
        foreach (var p in points)
            Include(p);
    }

    public bool Contains(Point p)
    {
        return p.X >= MinX && p.X <= MaxX && p.Y >= MinY && p.Y <= MaxY;
    }
}
