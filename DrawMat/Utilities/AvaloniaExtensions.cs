using Avalonia;
using System;

namespace DrawMat.Utilities;

public static class RectExtensions
{
    public static Rect Union(this Rect a, Rect b)
    {
        double x1 = Math.Min(a.X, b.X);
        double y1 = Math.Min(a.Y, b.Y);
        double x2 = Math.Max(a.X + a.Width, b.X + b.Width);
        double y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    public static Rect Union(this Rect rect, Point pt)
    {
        double x1 = Math.Min(rect.X, pt.X);
        double y1 = Math.Min(rect.Y, pt.Y);
        double x2 = Math.Max(rect.Right, pt.X);
        double y2 = Math.Max(rect.Bottom, pt.Y);

        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    public static Rect Union(this Point pt1, Point pt2)
    {
        double x1 = Math.Min(pt1.X, pt2.X);
        double y1 = Math.Min(pt1.Y, pt2.Y);
        double x2 = Math.Max(pt1.X, pt2.X);
        double y2 = Math.Max(pt1.Y, pt2.Y);

        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    public static Rect AddMargin(this Rect rect, double Margin)
    {
        return new Rect(rect.X - Margin/2, rect.Y - Margin/2, rect.Width + Margin, rect.Height + Margin);
    }    
}


