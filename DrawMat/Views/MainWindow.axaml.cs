using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using DrawMat.Shared;
using DrawMat.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
ColorPickerControl
            .GetObservable(Avalonia.Controls.ColorView.ColorProperty)
            .Subscribe(color => ViewModel.SelectColor(color));
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(DrawArea);

        if (point.Properties.IsRightButtonPressed)
        {
            _activeFlyout?.Hide();
            var supportedActions = ViewModel.GetSupportedFlyoutActions();
            if (supportedActions.Any())
            {
                var panel = new StackPanel();
                foreach (var action in supportedActions)
                {
                    var item = new MenuItem
                    {
                        Header = action.Label
                    };
                    item.Click += (_, __) =>
                    {
                        action.Execute();
                        Redraw();
                        _activeFlyout?.Hide();
                    };
                    panel.Children.Add(item);
                }
                _activeFlyout = new Flyout
                {
                    Placement = PlacementMode.Pointer,
                    Content = panel
                };
                _activeFlyout.ShowAt(DrawArea);
            }
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
