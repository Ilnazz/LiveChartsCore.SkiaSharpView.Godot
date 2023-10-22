using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Axes.Logarithmic;

namespace GodotSample.Axes.Logarithmic;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            YAxes = viewModel.YAxes
        });
    }
}
