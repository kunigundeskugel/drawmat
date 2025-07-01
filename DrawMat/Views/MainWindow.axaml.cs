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
    private Flyout? _activeFlyout;

    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();

        DrawPolylineButton.Click += (s, e) => ViewModel.SwitchToPolylineDrawingMode();
        SelectButton.Click += (s, e) => ViewModel.SwitchToSelectionInteractionMode();
        SaveImageButton.Click += (s, e) => OnSaveImageClick(s, e);
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(DrawArea);

        if (point.Properties.IsRightButtonPressed)
        {
            _activeFlyout?.Hide();
            _activeFlyout = new Flyout
            {
                Placement = PlacementMode.Pointer,
                Content = CreateFlyoutContentForSelect(point.Position)
            };
            _activeFlyout.ShowAt(DrawArea);
        }
        else if (point.Properties.IsLeftButtonPressed)
        {
            _activeFlyout?.Hide();
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

    private Control CreateFlyoutContentForSelect(Point clickPosition)
    {
        var panel = new StackPanel();

        var removeItem = new MenuItem
        {
            Header = "Remove"
        };
        removeItem.Click += (_, __) =>
        {
            ViewModel.FlyOutPressed(0);
            Redraw();
            _activeFlyout?.Hide();
        };
        panel.Children.Add(removeItem);
        return panel;
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
