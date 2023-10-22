using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Events.AddPointOnClick;

namespace GodotSample.Events.AddPointOnClick;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.SeriesCollection,
            PointerPressedCommand = viewModel.PointerDownCommand,
            TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Hidden
        });
    }
}
