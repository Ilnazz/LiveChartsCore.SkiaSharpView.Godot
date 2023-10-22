using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Events.Cartesian;

namespace GodotSample.Events.Cartesian;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            TooltipFindingStrategy = LiveChartsCore.Measure.TooltipFindingStrategy.CompareOnlyX
        });
    }
}
