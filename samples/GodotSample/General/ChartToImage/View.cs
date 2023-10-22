using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.ChartToImage;

namespace GodotSample.General.ChartToImage;

public partial class View : VBoxViewBase
{
    private readonly CartesianChart _cartesian;
    private readonly PieChart _pie;
    private readonly GeoMap _map;

    public View()
    {
        var viewModel = new ViewModel();

        _cartesian = new CartesianChart
        {
            Series = viewModel.CatesianSeries,
        };
        AddChild(_cartesian);

        _pie = new PieChart
        {
            Series = viewModel.PieSeries,
        };
        AddChild(_pie);

        _map = new GeoMap
        {
            Series = viewModel.GeoSeries,
        };
        AddChild(_map);

        // now lets create the images // mark
        CreateImageFromCartesianControl(); // mark
        CreateImageFromPieControl(); // mark
        CreateImageFromGeoControl(); // mark
    }

    private void CreateImageFromCartesianControl()
    {
        // you can take any chart in the UI, and build an image from it // mark
        var chartControl = _cartesian;
        var skChart = new SKCartesianChart(chartControl) { Width = 900, Height = 600, };
        skChart.SaveImage("CartesianImageFromControl.png");
    }

    private void CreateImageFromPieControl()
    {
        var chartControl = _pie;
        var skChart = new SKPieChart(chartControl) { Width = 900, Height = 600, };
        skChart.SaveImage("PieImageFromControl.png");
    }

    private void CreateImageFromGeoControl()
    {
        var chartControl = _map;
        var skChart = new SKGeoMap(chartControl) { Width = 900, Height = 600, };
        skChart.SaveImage("MapImageFromControl.png");
    }
}
