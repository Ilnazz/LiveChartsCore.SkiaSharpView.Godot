using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.NullPoints;

namespace GodotSample.General.NullPoints;

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
