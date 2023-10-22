﻿using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Bars.DelayedAnimation;

namespace GodotSample.Bars.DelayedAnimation;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series
        });
    }
}
