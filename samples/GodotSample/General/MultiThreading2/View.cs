using System;
using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.MultiThreading2;

namespace GodotSample.General.MultiThreading2;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel((Action action) => Callable.From(action).CallDeferred());

        AddChild(new CartesianChart
        {
            Series = viewModel.Series
        });
    }
}
