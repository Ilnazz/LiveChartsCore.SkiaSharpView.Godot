using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Lines.Properties;

namespace GodotSample.Lines.Properties;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var cartesianChart = new CartesianChart
        {
            Series = viewModel.Series
        };

        var buttonsBox = new HBoxContainer();
        AddChild(buttonsBox);

        var valuesButton = new Button { Text = "values" };
        valuesButton.Pressed += viewModel.ChangeValuesInstance;
        buttonsBox.AddChild(valuesButton);

        var seriesButton = new Button { Text = "series" };
        seriesButton.Pressed += () =>
        {
            viewModel.ChangeSeriesInstance();
            cartesianChart.Series = viewModel.Series;
        };
        buttonsBox.AddChild(seriesButton);

        var fillButton = new Button { Text = "fill" };
        fillButton.Pressed += viewModel.NewFill;
        buttonsBox.AddChild(fillButton);

        var strokeButton = new Button { Text = "stroke" };
        strokeButton.Pressed += viewModel.NewStroke;
        buttonsBox.AddChild(strokeButton);

        var geometryFillButton = new Button { Text = "geom fill" };
        geometryFillButton.Pressed += viewModel.NewGeometryFill;
        buttonsBox.AddChild(geometryFillButton);

        var geometryStrokeButton = new Button { Text = "geom stroke" };
        geometryStrokeButton.Pressed += viewModel.NewGeometryStroke;
        buttonsBox.AddChild(geometryStrokeButton);

        var increaseSmoothnessButton = new Button { Text = "+ smooth" };
        increaseSmoothnessButton.Pressed += viewModel.IncreaseLineSmoothness;
        buttonsBox.AddChild(increaseSmoothnessButton);

        var decreaseSmoothnessButton = new Button { Text = "- smooth" };
        decreaseSmoothnessButton.Pressed += viewModel.DecreaseLineSmoothness;
        buttonsBox.AddChild(decreaseSmoothnessButton);

        var increaseSizeButton = new Button { Text = "+ geom size" };
        increaseSizeButton.Pressed += viewModel.IncreaseGeometrySize;
        buttonsBox.AddChild(increaseSizeButton);

        var decreaseSizeButton = new Button { Text = "- geom size" };
        decreaseSizeButton.Pressed += viewModel.DecreaseGeometrySize;
        buttonsBox.AddChild(decreaseSizeButton);

        AddChild(cartesianChart);
    }
}
