using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Collections;
using Avalonia.Media;
using DrawMat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace DrawMat.ViewModels;

public class SelectionHandler : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
    private readonly MainViewModel _vm;
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

    public SelectionHandler(MainViewModel vm)
    {
        _vm = vm;
    }

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

    public IEnumerable<Control> GetSelectionVisuals()
    {
        var canvas = new Canvas();
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

