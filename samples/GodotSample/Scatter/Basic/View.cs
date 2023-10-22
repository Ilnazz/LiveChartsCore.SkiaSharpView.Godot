using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Scatter.Basic;

namespace GodotSample.Scatter.Basic;

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
