using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        foreach (var control in ViewModel.GetVisuals())
        {
            DrawArea.Children.Add(control);
        }
    }
    
    private void OnSaveImageClick(object? sender, RoutedEventArgs e)
    {
        var width = (int)DrawArea.Bounds.Width;
        var height = (int)DrawArea.Bounds.Height;

        if (width == 0 || height == 0)
            return;

        var rtb = new RenderTargetBitmap(new PixelSize(width, height));
        rtb.Render(DrawArea);

        var fileName = "canvas_output.png";
        using var stream = File.Create(fileName);
        rtb.Save(stream);
    }
}

