using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Test.Dispose;

namespace GodotSample.Test.Dispose;

public partial class View : ViewBase
{
    public View()
    {
        var changeContentButton = new Button() { Text = "Change content" };
        changeContentButton.Pressed += () =>
        {
            RemoveChild(GetChild(0));
            AddChild(GenerateControls(changeContentButton));
        };

        AddChild(GenerateControls(changeContentButton));
    }

    private static Control GenerateControls(Button button)
    {
        var viewModel = new ViewModel();

        var vBoxView = new VBoxViewBase();

        vBoxView.AddChild(button);
        vBoxView.AddChild(new CartesianChart
        {
            Series = viewModel.CartesianSeries
        });
        vBoxView.AddChild(new PieChart
        {
            Series = viewModel.PieSeries
        });
        vBoxView.AddChild(new PolarChart
        {
            Series = viewModel.PolarSeries
        });
        vBoxView.AddChild(new GeoMap
        {
            Series = viewModel.GeoSeries
        });

        return vBoxView;
    }
}
