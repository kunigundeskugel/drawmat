using Avalonia;
using Avalonia.Collections;
using DrawMat.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DrawMat.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public ObservableCollection<MyShape> Shapes { get; } = new();

    private MyShape? _currentShape;

    public void StartPolyline(Point start)
    {
        _currentShape = new MyShape
        {
            Type = "Polyline",
            StrokeColor = "Black",
            StrokeThickness = 2,
            Points = new List<Point> { start }
        };
        Shapes.Add(_currentShape);
    }

    public void ExtendPolyline(Point next)
    {
        _currentShape?.Points?.Add(next);
    }

    public void FinishPolyline()
    {
        _currentShape = null;
    }
    
    private string _title = "";
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
