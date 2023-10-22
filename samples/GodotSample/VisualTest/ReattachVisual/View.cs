using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.VisualTest.ReattachVisual;

namespace GodotSample.VisualTest.ReattachVisual;

public partial class View : VBoxViewBase
{
    private bool _isInVisualTree = true;
    private CartesianChart _chart;

    public View()
    {
        var viewModel = new ViewModel();

        var toggleAttachButton = new Button { Text = "Toggle attach" };
        toggleAttachButton.Pressed += ToggleAttachButtonPressed;
        AddChild(toggleAttachButton);

        _chart = new CartesianChart
        {
            Series = viewModel.Series,
            Sections = viewModel.Sections,
            ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X
        };
        AddChild(_chart);
    }

    private void ToggleAttachButtonPressed()
    {
        if (_isInVisualTree)
        {
            RemoveChild(_chart);
            _isInVisualTree = false;
            return;
        }

        AddChild(_chart);
        _isInVisualTree = true;
    }
}
