using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
using DrawMat.Shared;
using DrawMat.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace DrawMat.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    public GroupShape RootGroup { get; } = new();
    private string _title = "";
    private IInteractionMode _currentMode;

    public MainViewModel()
    {
        _currentMode = new SelectionInteractionMode();
    }

    public void SwitchToSelectionInteractionMode() => _currentMode = new SelectionInteractionMode();
    public void SwitchToPolylineDrawingMode() => _currentMode = new PolylineDrawingMode();
    public void PointerPressed(Point position) => _currentMode.PointerPressed(this, position);
    public void PointerMoved(Point position) => _currentMode.PointerMoved(this, position);
    public void PointerReleased(Point position) => _currentMode.PointerReleased(this, position);
    public void FlyOutPressed(FlyoutActionType type) => _currentMode.FlyOutPressed(this, type);
    public IEnumerable<FlyoutActionType> GetSupportedFlyoutActions() => _currentMode.GetSupportedFlyoutActions();

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
