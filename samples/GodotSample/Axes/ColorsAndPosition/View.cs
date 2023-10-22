using Godot;

using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Axes.ColorsAndPosition;

namespace GodotSample.Axes.ColorsAndPosition;

public partial class View : VBoxViewBase
{
	public View()
	{
		var viewModel = new ViewModel();

		var buttonsBox = new HBoxContainer();

		var togglePositionButton = new Button { Text = "toggle position" };
		togglePositionButton.Pressed += viewModel.TogglePosition;
		buttonsBox.AddChild(togglePositionButton);

		var setNewColorButton = new Button { Text = "new color" };
		setNewColorButton.Pressed += () => viewModel.SetNewColor();
		buttonsBox.AddChild(setNewColorButton);

        AddChild(buttonsBox);

		AddChild(new CartesianChart
        {
            Series = viewModel.Series,
            XAxes = viewModel.XAxes,
            YAxes = viewModel.YAxes
        });
	}
}
