using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using DrawMat.Models;
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
