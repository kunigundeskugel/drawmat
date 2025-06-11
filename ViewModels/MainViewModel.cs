using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DrawMat.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private string _greeting = "Hello Avalonia with MVVM!";

    public string Greeting
    {
        get => _greeting;
        set
        {
            if (_greeting != value)
            {
                _greeting = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

