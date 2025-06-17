using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using DrawMat.ViewModels;
using System.Collections.Generic;

namespace DrawMat.Views;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel => (MainViewModel)DataContext!;
    public Point _pressedLocation { get; set; } = new Point(0,0);

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _pressedLocation = e.GetPosition(DrawArea);
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (e.GetCurrentPoint(DrawArea).Properties.IsLeftButtonPressed)
        {
            if (_pressedLocation.X != 0 && _pressedLocation.Y != 0)
            {
                ViewModel.StartPolyline(_pressedLocation, e.GetPosition(DrawArea));
                _pressedLocation = new Point(0,0);
            } else
            {
                ViewModel.ExtendPolyline(e.GetPosition(DrawArea));
            }
            Redraw();
        }
    }

    private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        ViewModel.FinishPolyline();
        Redraw();
    }

    private void Redraw()
    {
        DrawArea.Children.Clear();
        foreach (var control in ViewModel.GetVisuals())
        {
            DrawArea.Children.Add(control);
        }
    }
}

