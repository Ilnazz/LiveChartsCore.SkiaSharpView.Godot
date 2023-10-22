using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Pies.Gauge;

namespace GodotSample.Pies.Gauge;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var vBox = new VBoxContainer();
        AddChild(vBox);

        vBox.AddChild(new Label { Text = "Initial rotation" });
        var initialRotationSlider = new HSlider() { MinValue = -360, MaxValue = 720 };
        initialRotationSlider.ValueChanged += value => viewModel.InitialRotation = value;
        vBox.AddChild(initialRotationSlider);

        vBox = new VBoxContainer();
        AddChild(vBox);

        vBox.AddChild(new Label { Text = "Max angle" });
        var maxAngleSlider = new HSlider() { MinValue = 0, MaxValue = 360 };
        maxAngleSlider.ValueChanged += value => viewModel.MaxAngle = value;
        vBox.AddChild(maxAngleSlider);

        vBox = new VBoxContainer();
        AddChild(vBox);

        vBox.AddChild(new Label { Text = "Inner radius" });
        var innerRadiusSlider = new HSlider() { MinValue = 0, MaxValue = 50 };
        innerRadiusSlider.ValueChanged += value => viewModel.InnerRadius = value;
        vBox.AddChild(innerRadiusSlider);

        vBox = new VBoxContainer();
        AddChild(vBox);

        vBox.AddChild(new Label { Text = "Offset radius" });
        var offsetRadiusSlider = new HSlider() { MinValue = 0, MaxValue = 50 };
        offsetRadiusSlider.ValueChanged += value => viewModel.OffsetRadius = value;
        vBox.AddChild(offsetRadiusSlider);

        vBox = new VBoxContainer();
        AddChild(vBox);

        vBox.AddChild(new Label { Text = "Background inner radius" });
        var backgroundInnerRadiusSlider = new HSlider() { MinValue = 0, MaxValue = 50 };
        backgroundInnerRadiusSlider.ValueChanged += value => viewModel.BackgroundInnerRadius = value;
        vBox.AddChild(backgroundInnerRadiusSlider);

        vBox = new VBoxContainer();
        AddChild(vBox);

        vBox.AddChild(new Label { Text = "Background offset radius" });
        var backgroundOffsetRadiusSlider = new HSlider() { MinValue = 0, MaxValue = 50 };
        backgroundOffsetRadiusSlider.ValueChanged += value => viewModel.BackgroundOffsetRadius = value;
        vBox.AddChild(backgroundOffsetRadiusSlider);

        viewModel.PropertyChanged += (_, e) =>
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.InitialRotation):
                    initialRotationSlider.Value = viewModel.InitialRotation;
                    break;
                case nameof(ViewModel.MaxAngle):
                    maxAngleSlider.Value = viewModel.MaxAngle;
                    break;
                case nameof(ViewModel.InnerRadius):
                    innerRadiusSlider.Value = viewModel.InnerRadius;
                    break;
                case nameof(ViewModel.OffsetRadius):
                    offsetRadiusSlider.Value = viewModel.OffsetRadius;
                    break;
                case nameof(ViewModel.BackgroundInnerRadius):
                    backgroundInnerRadiusSlider.Value = viewModel.BackgroundInnerRadius;
                    break;
                case nameof(ViewModel.BackgroundOffsetRadius):
                    backgroundOffsetRadiusSlider.Value = viewModel.BackgroundOffsetRadius;
                    break;
                default:
                    break;
            }
        };

        AddChild(new PieChart
        {
            Series = viewModel.Series,
        });
    }
}
