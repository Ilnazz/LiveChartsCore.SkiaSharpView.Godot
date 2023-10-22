using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.MultiThreading;

namespace GodotSample.General.MultiThreading;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new CartesianChart
        {
            SyncContext = viewModel.Sync,
            Series = viewModel.Series
        });
    }
}
