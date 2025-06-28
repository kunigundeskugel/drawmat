using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DrawMat.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace DrawMat.Views;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel => (MainViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();

        DrawPolylineButton.Click += (s, e) => ViewModel.SwitchToPolylineDrawingMode();
        EraseButton.Click += (s, e) => ViewModel.SwitchToErasingMode();
        SelectButton.Click += (s, e) => ViewModel.SwitchToSelectionInteractionMode();
        SaveImageButton.Click += (s, e) => OnSaveImageClick(s, e);
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(DrawArea);

        if (point.Properties.IsRightButtonPressed)
        {
            ViewModel.PointerPressedRight(point.Position);
        }
        else if (point.Properties.IsLeftButtonPressed)
        {
            ViewModel.PointerPressed(point.Position);
        }
        Redraw();
    }

    private void Canvas_PointerMoved(object? sender, PointerEventArgs e)
    {
        ViewModel.PointerMoved(e.GetPosition(DrawArea));
        Redraw();
    }

    private void Canvas_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        ViewModel.PointerReleased(e.GetPosition(DrawArea));
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
    
    private async void OnSaveImageClick(object? sender, RoutedEventArgs e)
    {
        var width = (int)DrawArea.Bounds.Width;
        var height = (int)DrawArea.Bounds.Height;

        if (width == 0 || height == 0)
            return;

        using var rtb = new RenderTargetBitmap(new PixelSize(width, height));
        rtb.Render(DrawArea);

        var file = await PickSavePathAsync();
        if (file != null)
        {
            using var stream = await file.OpenWriteAsync();
            rtb.Save(stream);
        }
    }

    private async Task<IStorageFile?> PickSavePathAsync()
    {
        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Image As...",
            SuggestedFileName = "canvas_output.png",
            FileTypeChoices = new[]
            {
                new FilePickerFileType("PNG Image")
                {
                    Patterns = new[] { "*.png" }
                }
            }
        });

        return file;
    }
}
