using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using DrawMat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace DrawMat.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public GroupShape RootGroup { get; } = new();
    private PolylineShape? _currentShape;
    private string _title = "";
    private Rect? _selectionRect;
    public Rect? SelectionRect
    {
        get => _selectionRect;
        private set
        {
            _selectionRect = value;
            OnPropertyChanged(nameof(SelectionRect));
        }
    }
    private Point _selectionStart;

    public void StartSelection(Point start)
    {
        _selectionStart = start;
        SelectionRect = new Rect(start, new Size(0, 0));
    }

    public void ExtendSelection(Point next)
    {
        if (SelectionRect == null) return;
        var x = Math.Min(_selectionStart.X, next.X);
        var y = Math.Min(_selectionStart.Y, next.Y);
        var width = Math.Abs(next.X - _selectionStart.X);
        var height = Math.Abs(next.Y - _selectionStart.Y);

        SelectionRect = new Rect(x, y, width, height);
    }

    public void FinishSelection()
    {
        // TODO: find shapes inside
        SelectionRect = null;
    }

    public void StartPolyline(Point start)
    {
        _currentShape = new PolylineShape(new List<Point> { start });
        RootGroup.Children.Add(_currentShape);
    }

    public void ExtendPolyline(Point next)
    {
        _currentShape?.AddPoint(next);
    }

    public void FinishPolyline()
    {
        _currentShape = null;
    }

    public IEnumerable<Control> GetVisuals()
    {
        yield return RootGroup.ToControl();
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public event PropertyChangedEventHandler? PropertyChanged;
}
