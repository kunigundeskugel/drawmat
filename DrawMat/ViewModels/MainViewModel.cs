using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using Avalonia.Media;
using DrawMat.Models;
using DrawMat.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reactive.Linq;
using System.Linq;

namespace DrawMat.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public GroupShape RootGroup { get; } = new();
    private string _title = "";
    private IInteractionMode _currentMode;
    private Color _color = Colors.Black;

    public MainViewModel()
    {
        _currentMode = new SelectionInteractionMode();
    }

    public void SwitchToSelectionInteractionMode() => _currentMode = new SelectionInteractionMode();
    public void SwitchToPolylineDrawingMode() => _currentMode = new PolylineDrawingMode();
    public void PointerPressed(Point position)
    {
        _currentMode.ColorSelected(this, _color);
        _currentMode.PointerPressed(this, position);
    }
    public void PointerMoved(Point position) => _currentMode.PointerMoved(this, position);
    public void PointerReleased(Point position) => _currentMode.PointerReleased(this, position);
    public IEnumerable<FlyoutAction> GetSupportedFlyoutActions() => _currentMode.GetSupportedFlyoutActions(this);
    public void SelectColor(Color selectedColor)
    {
        _color = selectedColor;
    }

    public IEnumerable<Control> GetVisuals()
    {
        return new[] { RootGroup.ToControl() }
        .Concat(_currentMode.GetVisuals());
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public event PropertyChangedEventHandler? PropertyChanged;
}
