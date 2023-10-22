using Godot;
using System;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.Legends;

namespace GodotSample.General.Legends;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var cartesianChart = new CartesianChart
        {
            Series = viewModel.Series,
            //TODO: check it will work: TooltipPosition = viewModel.Position,
            LegendPosition = LegendPosition.Hidden
        };

        var optionButton = new OptionButton();
        optionButton.ItemSelected += index =>
            cartesianChart.LegendPosition = (LegendPosition)index;

        var options = Enum.GetNames(typeof(LegendPosition));
        foreach (var option in options)
            optionButton.AddItem(option);

        optionButton.Selected = 0;

        AddChild(optionButton);
        AddChild(cartesianChart);
    }
}
