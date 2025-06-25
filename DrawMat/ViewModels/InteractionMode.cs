using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using DrawMat.Models;
using System;
using System.Collections.Generic;

namespace DrawMat.ViewModels;

public interface IInteractionMode
{
    void PointerPressed(MainViewModel vm, Point position);
    void PointerMoved(MainViewModel vm, Point position);
    void PointerReleased(MainViewModel vm, Point position);
}

public class PolylineDrawingMode : IInteractionMode
{
    private PolylineShape? _currentShape;

    public void PointerPressed(MainViewModel vm, Point position)
    {
        _currentShape = new PolylineShape(new List<Point> { position });
        vm.RootGroup.Children.Add(_currentShape);
    }

    public void PointerMoved(MainViewModel vm, Point position)
    {
        _currentShape?.AddPoint(position);
    }

    public void PointerReleased(MainViewModel vm, Point position)
    {
        _currentShape = null;
    }
}

public class SelectionInteractionMode : IInteractionMode
{
    public void PointerPressed(MainViewModel vm, Point position) => vm.Selection.StartSelection(position);
    public void PointerMoved(MainViewModel vm, Point position) => vm.Selection.ExtendSelection(position);
    public void PointerReleased(MainViewModel vm, Point position) => vm.Selection.FinishSelection();
}
