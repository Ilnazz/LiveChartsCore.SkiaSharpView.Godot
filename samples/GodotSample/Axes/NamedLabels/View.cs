using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Axes.NamedLabels;

namespace GodotSample.Axes.NamedLabels;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            XAxes = viewModel.XAxes,
            YAxes = viewModel.YAxes,
            TooltipPosition = LiveChartsCore.Measure.TooltipPosition.Left, // mark
            TooltipTextPaint = viewModel.TooltipTextPaint, // mark
            TooltipBackgroundPaint = viewModel.TooltipBackgroundPaint, // mark
            TooltipTextSize = 16
        });
    }
}
