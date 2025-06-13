using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using System.Collections.Generic;

using DrawMat.ViewModels;

namespace DrawMat.Views;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel => (MainViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        ViewModel.StartPolyline(e.GetPosition(DrawArea));
        Redraw();
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (e.GetCurrentPoint(DrawArea).Properties.IsLeftButtonPressed)
        {
            ViewModel.ExtendPolyline(e.GetPosition(DrawArea));
            Redraw();
        }
    }

    private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        ViewModel.FinishPolyline();
    }

    private void Redraw()
    {
        DrawArea.Children.Clear();
        foreach (var shape in ViewModel.Shapes)
        {
            if (shape.Type == "Polyline" && shape.Points != null)
            {
                DrawArea.Children.Add(new Polyline
                {
                    Points = new AvaloniaList<Point>(shape.Points),
                    Stroke = Brushes.Black,
                    StrokeThickness = shape.StrokeThickness
                });
            }
        }
    }
}

