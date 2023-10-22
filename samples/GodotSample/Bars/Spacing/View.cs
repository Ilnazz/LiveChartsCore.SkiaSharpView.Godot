﻿using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Bars.Spacing;

namespace GodotSample.Bars.Spacing;

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
