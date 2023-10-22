using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.VisualElements;

namespace GodotSample.General.VisualElements;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            VisualElements = viewModel.VisualElements,
            ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X
        });
    }
}
