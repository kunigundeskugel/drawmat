using Avalonia;
using Avalonia.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawMat.Models;

public class BoundingBox
{
    public double MinX { get; private set; } = double.PositiveInfinity;
    public double MinY { get; private set; } = double.PositiveInfinity;
    public double MaxX { get; private set; } = double.NegativeInfinity;
    public double MaxY { get; private set; } = double.NegativeInfinity;

    public double Width => MaxX - MinX;
    public double Height => MaxY - MinY;

    public bool IsEmpty => Width < 0 || Height < 0;

    public BoundingBox()
    {
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
        Include(p.X, p.Y);
    }

    public BoundingBox(IEnumerable<Point> points)
    {
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
        {
            Include(p);
        }
    }

    public void Include(BoundingBox bbox)
    {
        if (bbox.IsEmpty) return;

        Include(bbox.MinX, bbox.MinY);
        Include(bbox.MaxX, bbox.MaxY);
    }

    public bool Contains(Point p)
    {
        return MinX <= p.X && p.X <= MaxX && MinY <= p.Y && p.Y <= MaxY;
    }
}
