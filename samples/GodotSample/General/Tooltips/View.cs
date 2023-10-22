using System;
using Godot;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.Tooltips;

namespace GodotSample.General.Tooltips;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var cartesianChart = new CartesianChart
        {
            Series = viewModel.Series,
            //TODO: check it will work: TooltipPosition = viewModel.Position,
            LegendPosition = LegendPosition.Left
        };

        var optionButton = new OptionButton();
        optionButton.ItemSelected += index =>
            cartesianChart.TooltipPosition = (TooltipPosition)index;

        var options = Enum.GetNames(typeof(TooltipPosition));
        foreach (var option in options)
            optionButton.AddItem(option);

        optionButton.Selected = 0;

        AddChild(optionButton);
        AddChild(cartesianChart);
    }
}
