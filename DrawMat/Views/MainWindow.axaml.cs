using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using DrawMat.ViewModels;

namespace DrawMat.Views;

public partial class MainWindow : Window
{
    public MainViewModel ViewModel => (MainViewModel)DataContext!;

    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainViewModel();

        DrawPolylineButton.Click += (s, e) => ViewModel.SwitchToPolylineDrawingMode();
        SelectButton.Click += (s, e) => ViewModel.SwitchToSelectionInteractionMode();
    }

    private void Canvas_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        ViewModel.PointerPressed(e.GetPosition(DrawArea));
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
}
