﻿using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Design.StrokeDashArray;

namespace GodotSample.Design.StrokeDashArray;

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
