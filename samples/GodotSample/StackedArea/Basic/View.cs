using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.StackedArea.Basic;

namespace GodotSample.StackedArea.Basic;

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
