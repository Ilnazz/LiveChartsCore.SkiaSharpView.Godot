using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.StepLines.Basic;

namespace GodotSample.StepLines.Basic;

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
