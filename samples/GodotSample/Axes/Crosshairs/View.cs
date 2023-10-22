using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Axes.Crosshairs;

namespace GodotSample.Axes.Crosshairs;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            XAxes = viewModel.XAxes,
            YAxes = viewModel.YAxes
        });
    }
}
