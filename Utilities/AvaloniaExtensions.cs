using Avalonia;
using System;

namespace DrawMat.Utilities;

public static class RectExtensions
{
    public static Rect Union(this Rect a, Rect b)
    {
        if (a.IsEmpty()) return b;
        if (b.IsEmpty()) return a;

        double x1 = Math.Min(a.X, b.X);
        double y1 = Math.Min(a.Y, b.Y);
        double x2 = Math.Max(a.X + a.Width, b.X + b.Width);
        double y2 = Math.Max(a.Y + a.Height, b.Y + b.Height);

        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    public static Rect Union(this Rect rect, Point pt)
    {
        if (rect.IsEmpty())
            return new Rect(pt.X, pt.Y, 0, 0);

        double x1 = Math.Min(rect.X, pt.X);
        double y1 = Math.Min(rect.Y, pt.Y);
        double x2 = Math.Max(rect.Right, pt.X);
        double y2 = Math.Max(rect.Bottom, pt.Y);

        return new Rect(x1, y1, x2 - x1, y2 - y1);
    }

    public static bool IsEmpty(this Rect rect)
    {
        return rect.Width <= 0 || rect.Height <= 0;
    }
}


