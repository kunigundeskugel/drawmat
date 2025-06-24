using Avalonia;

namespace DrawMat.ViewModels;

public interface IInteractionMode
{
    void PointerPressed(MainViewModel vm, Point position);
    void PointerMoved(MainViewModel vm, Point position);
    void PointerReleased(MainViewModel vm, Point position);
}

public class PolylineDrawingMode : IInteractionMode
{
    public void PointerPressed(MainViewModel vm, Point position) => vm.StartPolyline(position);
    public void PointerMoved(MainViewModel vm, Point position) => vm.ExtendPolyline(position);
    public void PointerReleased(MainViewModel vm, Point position) => vm.FinishPolyline();
}

public class SelectionInteractionMode : IInteractionMode
{
    public void PointerPressed(MainViewModel vm, Point position) => vm.StartSelection(position);
    public void PointerMoved(MainViewModel vm, Point position) => vm.ExtendSelection(position);
    public void PointerReleased(MainViewModel vm, Point position) => vm.FinishSelection();
}
