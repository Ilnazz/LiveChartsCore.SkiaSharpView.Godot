﻿using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Financial.BasicCandlesticks;

namespace GodotSample.Financial.BasicCandlesticks;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            XAxes = viewModel.XAxes
        });
    }
}
