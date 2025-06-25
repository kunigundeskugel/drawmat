using Avalonia;
using Avalonia.Controls;
using Avalonia.Collections;
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

    public SelectionHandler Selection { get; }

    public MainViewModel()
    {
        Selection = new SelectionHandler(this);
    }

    public IEnumerable<Control> GetVisuals()
    {
        return new[] { RootGroup.ToControl() }
        .Concat(Selection.GetSelectionVisuals());
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
