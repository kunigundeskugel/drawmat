using Avalonia;
using Avalonia.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
namespace DrawMat.Utilities;

public static class RectExtensions
{
    public static Rect Union(this Rect a, Rect b)
    {
        if (a.IsEmpty()) return b;
        if (b.IsEmpty()) return a;

        double x1 = Math.Min(a.X, b.X);
        double y1 = Math.Min(a.Y, b.Y);
        double x2 = Math.Max(a.Right, b.Right);
        double y2 = Math.Max(a.Bottom, b.Bottom);

        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    public static Rect Union(this Rect rect, Point pt)
    {
        return RectExtensions.Union(rect, new Rect(pt.X, pt.Y, 0, 0));
    }

    public static Rect Union(this Point pt1, Point pt2)
    { 
        return RectExtensions.Union(new Rect(pt1.X, pt1.Y, 0, 0), new Rect(pt2.X, pt2.Y, 0, 0));
    }

    public static Rect GetBoundingBox(this List<Point> pts)
    {
        Rect groupBox = new Rect(pts[0].X, pts[0].Y, 0, 0);
        foreach (var pt in pts.Skip(1))
        {
            groupBox = groupBox.Union(pt);
        }
        return groupBox;
    }

    public static Rect AddMargin(this Rect rect, double Margin)
    {
        return new Rect(rect.X - Margin/2, rect.Y - Margin/2, rect.Width + Margin, rect.Height + Margin);
    }
    
    public static bool IsEmpty(this Rect rect)
    {
        return rect.X <= 0 && rect.Y <= 0 && rect.Width <= 0 && rect.Height <= 0;
    }
}


