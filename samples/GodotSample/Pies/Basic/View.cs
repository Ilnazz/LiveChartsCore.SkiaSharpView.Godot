using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Pies.Basic;

namespace GodotSample.Pies.Basic;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new PieChart
        {
            Series = viewModel.Series,
            Title = viewModel.Title
        });
    }
}
