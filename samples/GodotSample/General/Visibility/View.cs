using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.Visibility;

namespace GodotSample.General.Visibility;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var buttons = new HBoxContainer();

        var toggle1Button = new Button { Text = "toggle 1" };
        toggle1Button.Pressed += viewModel.ToggleSeries0;
        buttons.AddChild(toggle1Button);

        var toggle2Button = new Button { Text = "toggle 2" };
        toggle2Button.Pressed += viewModel.ToggleSeries1;
        buttons.AddChild(toggle2Button);

        var toggle3Button = new Button { Text = "toggle 3" };
        toggle3Button.Pressed += viewModel.ToggleSeries2;
        buttons.AddChild(toggle3Button);

        AddChild(buttons);
        AddChild(new CartesianChart
        {
            Series = viewModel.Series
        });
    }
}
