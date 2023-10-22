using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Axes.Multiple;

namespace GodotSample.Axes.Multiple;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            YAxes = viewModel.YAxes,
            LegendPosition = LiveChartsCore.Measure.LegendPosition.Left,
            LegendTextPaint = viewModel.LegendTextPaint,
            LegendBackgroundPaint = viewModel.LedgendBackgroundPaint,
            LegendTextSize = 16
        });
    }
}
