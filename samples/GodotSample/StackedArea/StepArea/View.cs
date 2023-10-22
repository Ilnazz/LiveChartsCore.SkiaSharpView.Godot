﻿using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.StackedArea.StepArea;

namespace GodotSample.StackedArea.StepArea;

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
