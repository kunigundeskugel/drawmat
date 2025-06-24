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
    private IInteractionMode _currentMode;
    public MainViewModel ViewModel => (MainViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();
        _currentMode = new SelectionInteractionMode();

        DrawPolylineButton.Click += (s, e) => _currentMode = new PolylineDrawingMode();
        SelectButton.Click += (s, e) => _currentMode = new SelectionInteractionMode();
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        _currentMode.PointerPressed(ViewModel, e.GetPosition(DrawArea));
        Redraw();
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        _currentMode.PointerMoved(ViewModel, e.GetPosition(DrawArea));
        Redraw();
    }

    private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        _currentMode.PointerReleased(ViewModel, e.GetPosition(DrawArea));
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
