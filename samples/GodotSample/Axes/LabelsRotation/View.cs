using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Axes.LabelsRotation;

namespace GodotSample.Axes.LabelsRotation;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var sliderContainer = new VBoxContainer();

        var sliderLabel = new Label();
        sliderLabel.AddThemeColorOverride("font_color", Colors.Black);
        var slider = new HSlider() { MinValue = -360, MaxValue = 720 };
        slider.ValueChanged += value =>
        {
            viewModel.SliderValue = value;
            sliderLabel.Text = $"Rotation: {value} deg.";
        };
        sliderLabel.Text = $"Rotation: {viewModel.SliderValue} deg.";
        sliderContainer.AddChild(sliderLabel);
        sliderContainer.AddChild(slider);

        AddChild(sliderContainer);

        AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            XAxes = viewModel.XAxes,
            YAxes = viewModel.YAxes
        });
    }
}
