using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.StepLines.Area;

namespace GodotSample.StepLines.Area;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            DrawMarginFrame = viewModel.DrawMarginFrame
        });
    }
}
