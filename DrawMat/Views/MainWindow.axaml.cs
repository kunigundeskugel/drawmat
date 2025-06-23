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
    private Mode currentMode = Mode.None;
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
            //TODO: implement selection
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
            //TODO: implement selection
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
            //TODO: implement selection
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
    }
}

