using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Axes.DateTimeScaled;

namespace GodotSample.Axes.DateTimeScaled;

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
