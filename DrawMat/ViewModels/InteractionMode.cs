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
    void PointerPressedRight(MainViewModel vm, Point position) {}

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

public class ErasingMode : IInteractionMode
{
    private bool _dragging = false;
    public void Erase(MainViewModel vm, Point position){
        var hits = vm.RootGroup.SearchChildren(position);
        foreach (var child in hits)
        {
            vm.RootGroup.Children.Remove(child);
        }
    }
    public void PointerPressed(MainViewModel vm, Point position)
    {
        _dragging = true;
        Erase(vm, position);
    }

    public void PointerMoved(MainViewModel vm, Point position)
    {
        if (_dragging) Erase(vm, position);
    }

    public void PointerReleased(MainViewModel vm, Point position)
    {
        _dragging = false;
    }

    public IEnumerable<Control> GetVisuals()
    {
        return Enumerable.Empty<Control>();
    }
}

public class SelectionInteractionMode : IInteractionMode
{
    private Point _selectionStart;
    public Rect? SelectionRect;
    public GroupShape? SelectedShapes;

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
        var shapes = vm.RootGroup.SearchChildren(new BoundingBox(rect.X, rect.Y, rect.Right, rect.Bottom));
        SelectedShapes = new GroupShape();
        SelectedShapes.Add(shapes);
        SelectionRect = null;
    }

    public void PointerPressedRight(MainViewModel vm, Point position)
    {
        if (SelectedShapes != null){
            foreach (var child in SelectedShapes.Children)
            {
                vm.RootGroup.Children.Remove(child);
            }
        }
        SelectedShapes = null;
    }

    public IEnumerable<Control> GetVisuals()
    {
        var canvas = new Canvas();
        if (SelectedShapes != null){
            foreach (var child in SelectedShapes.Children)
            {
                var bboxRect = child.CreateBoundingBoxVisual();
                if (bboxRect != null)
                {
                    canvas.Children.Add(bboxRect);
                }
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
