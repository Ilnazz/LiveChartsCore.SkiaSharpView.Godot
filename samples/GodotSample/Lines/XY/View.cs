using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Lines.XY;

namespace GodotSample.Lines.XY;

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
