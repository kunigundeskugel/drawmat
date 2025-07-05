using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Collections;
using Avalonia.Media;
using DrawMat.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DrawMat.ViewModels;

public interface IInteractionMode
{
    void PointerPressed(MainViewModel vm, Point position) {}
    void PointerMoved(MainViewModel vm, Point position) {}
    void PointerReleased(MainViewModel vm, Point position) {}

    IEnumerable<KeyValuePair<string, Action>> GetSupportedFlyoutActions(MainViewModel vm)
    {
        return Array.Empty<KeyValuePair<string, Action>>();
    }
    IEnumerable<Control> GetVisuals();
}

public class PolylineDrawingMode : IInteractionMode
{
    private PolylineShape? _currentShape;

    public void PointerPressed(MainViewModel vm, Point position)
    {
        _currentShape = new PolylineShape(new List<Point> { position });
    }

    public void PointerMoved(MainViewModel vm, Point position)
    {
        _currentShape?.AddPoint(position);
    }

    public void PointerReleased(MainViewModel vm, Point position)
    {
        if (_currentShape != null)
        {
            vm.RootGroup.Add(_currentShape);
        }
        _currentShape = null;
    }

    public IEnumerable<Control> GetVisuals()
    {
        if (_currentShape == null) return Enumerable.Empty<Control>();

        var canvas = new Canvas();
        canvas.Children.Add(_currentShape.ToControl());
        return new [] {canvas};
    }
}

public class SelectionInteractionMode : IInteractionMode
{
    private Point _selectionStart;
    public Rect? SelectionRect;
    public List<ShapeBase> SelectedShapes = new List<ShapeBase>();

    public void PointerPressed(MainViewModel vm, Point position)
    {
        _selectionStart = position;
        SelectionRect = new Rect(position, new Size(0, 0));
    }

    public void PointerMoved(MainViewModel vm, Point position)
    {
        if (SelectionRect == null) return;
        var x = Math.Min(_selectionStart.X, position.X);
        var y = Math.Min(_selectionStart.Y, position.Y);
        var width = Math.Abs(position.X - _selectionStart.X);
        var height = Math.Abs(position.Y - _selectionStart.Y);

        SelectionRect = new Rect(x, y, width, height);
    }

    public void PointerReleased(MainViewModel vm, Point position)
    {
        if (SelectionRect == null) return;
        var rect = SelectionRect.Value;
        SelectedShapes = vm.RootGroup.SearchChildren(new BoundingBox(rect.X, rect.Y, rect.Right, rect.Bottom));
        SelectionRect = null;
    }

    public IEnumerable<KeyValuePair<string, Action>> GetSupportedFlyoutActions(MainViewModel vm)
    {
        return new List<KeyValuePair<string, Action>>
        {
            new("Delete", () =>
            {
                foreach (var child in SelectedShapes)
                {
                    vm.RootGroup.Children.Remove(child);
                }
                SelectedShapes.Clear();
            })
        };
    }

    public IEnumerable<Control> GetVisuals()
    {
        var canvas = new Canvas();
        foreach (var child in SelectedShapes)
        {
            var bboxRect = child.CreateBoundingBoxVisual();
            if (bboxRect != null)
            {
                canvas.Children.Add(bboxRect);
            }
        }
        if (SelectionRect is Rect rect)
        {
            var selectionRect = new Rectangle
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 1,
                StrokeDashArray = new Avalonia.Collections.AvaloniaList<double> { 4, 2 },
                Fill = new SolidColorBrush(Color.FromArgb(50, 0, 120, 200)),
                Width = rect.Width,
                Height = rect.Height,
                IsHitTestVisible = false
            };

            Canvas.SetLeft(selectionRect, rect.X);
            Canvas.SetTop(selectionRect, rect.Y);

            canvas.Children.Add(selectionRect);
        }
        return new [] {canvas};
    }
}
