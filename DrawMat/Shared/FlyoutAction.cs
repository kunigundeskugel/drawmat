using System;

namespace DrawMat.Shared;

public struct FlyoutAction
{
    public string Label;
    public Action Execute;

    public FlyoutAction(string label, Action execute)
    {
        Label = label;
        Execute = execute;
    }
}
