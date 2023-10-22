using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Test.ChangeSeriesInstance;

namespace GodotSample.Test.ChangeSeriesInstance;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var changeContentButton = new Button() { Text = "Change content" };
        changeContentButton.Pressed += viewModel.GenerateData;
        AddChild(changeContentButton);

        AddChild(new CartesianChart
        {
            Series = viewModel.CartesianSeries
        });
        AddChild(new PieChart
        {
            Series = viewModel.PieSeries
        });
        AddChild(new PolarChart
        {
            Series = viewModel.PolarSeries
        });
        AddChild(new GeoMap
        {
            Series = viewModel.GeoSeries
        });
    }
}
