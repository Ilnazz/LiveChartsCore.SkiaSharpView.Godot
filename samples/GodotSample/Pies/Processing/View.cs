using Godot;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Pies.Processing;
using SkiaSharp.Views.Godot;

namespace GodotSample.Pies.Processing;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var labels = new HBoxContainer();

        var b1 = new Label();
        var b2 = new Label();
        var b3 = new Label();

        var pieChart = new PieChart
        {
            Series = viewModel.Series
        };
        pieChart.UpdateStarted += _ =>
        {
            var series = (PieSeries<ObservableValue>)viewModel.Series[0];
            var values = (ObservableValue[])series.Values;
            b1.Text = values[0].Value + " " + series.Name;
            b1.SelfModulate = GetForeColor(series);

            series = (PieSeries<ObservableValue>)viewModel.Series[1];
            values = (ObservableValue[])series.Values;
            b2.Text = values[0].Value + " " + series.Name;
            b2.SelfModulate = GetForeColor(series);

            series = (PieSeries<ObservableValue>)viewModel.Series[2];
            values = (ObservableValue[])series.Values;
            b3.Text = values[0].Value + " " + series.Name;
            b3.SelfModulate = GetForeColor(series);
        };

        AddChild(labels);
        AddChild(pieChart);
    }

    private static Color GetForeColor(PieSeries<ObservableValue> pieSeries)
    {
        return pieSeries.Fill is not SolidColorPaint solidColorBrush
            ? new Color()
            : solidColorBrush.Color.ToGDColor();
    }
}
