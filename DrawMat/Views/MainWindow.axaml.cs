using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using DrawMat.ViewModels;
using System.Collections.Generic;

namespace DrawMat.Views;

public partial class MainWindow : Window
{
    private enum Mode { None, DrawingPolyline, Selection }
    private Mode currentMode = Mode.Selection;
    public MainViewModel ViewModel => (MainViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
        DrawPolylineButton.Click += DrawPolylineButton_Click;
        SelectButton.Click += SelectButton_Click;
    }

    private void DrawPolylineButton_Click(object? sender, RoutedEventArgs e)
    {
        currentMode = Mode.DrawingPolyline;
    }

    private void SelectButton_Click(object? sender, RoutedEventArgs e)
    {
        currentMode = Mode.Selection;
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (currentMode == Mode.DrawingPolyline)
        {
            ViewModel.StartPolyline(e.GetPosition(DrawArea));
        }
        else if (currentMode == Mode.Selection)
        {
            ViewModel.StartSelection(e.GetPosition(DrawArea));
        }
        Redraw();
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        if (currentMode == Mode.DrawingPolyline)
        {
            ViewModel.ExtendPolyline(e.GetPosition(DrawArea));
        }
        else if (currentMode == Mode.Selection)
        {
            ViewModel.ExtendSelection(e.GetPosition(DrawArea));
        }
        Redraw();
    }

    private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (currentMode == Mode.DrawingPolyline)
        {
            ViewModel.FinishPolyline();
        }
        else if (currentMode == Mode.Selection)
        {
            ViewModel.FinishSelection();
        }
        Redraw();
    }

    private void Redraw()
    {
        DrawArea.Children.Clear();

        foreach (var control in ViewModel.GetVisuals())
        {
            DrawArea.Children.Add(control);
        }

        if (ViewModel.SelectionRect is Rect rect)
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

            DrawArea.Children.Add(selectionRect);
        }
    }

}

