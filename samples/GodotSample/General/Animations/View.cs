using System.Linq;
using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.General.Animations;

namespace GodotSample.General.Animations;

public partial class View : VBoxViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        var buttons = new HBoxContainer();
        AddChild(buttons);

        buttons.AddChild(new Label { Text = "Curve: ", SelfModulate = Colors.Black });
        var curveOptionButton = new OptionButton();
        curveOptionButton.ItemSelected += index => viewModel.SelectedCurve = viewModel.AvalaibaleCurves[index];
        buttons.AddChild(curveOptionButton);

        foreach (var curve in viewModel.AvalaibaleCurves.Select(p => p.Item1))
            curveOptionButton.AddItem(curve);
        curveOptionButton.Selected = viewModel.AvalaibaleCurves.ToList().IndexOf(viewModel.SelectedCurve);

        buttons.AddChild(new Label { Text = "Speed: ", SelfModulate = Colors.Black });
        var speedOptionButton = new OptionButton();
        speedOptionButton.ItemSelected += index => viewModel.SelectedSpeed = viewModel.AvailableSpeeds[index];
        buttons.AddChild(speedOptionButton);

        foreach (var speed in viewModel.AvailableSpeeds.Select(p => p.Item1))
            speedOptionButton.AddItem(speed);
        speedOptionButton.Selected = viewModel.AvailableSpeeds.ToList().IndexOf(viewModel.SelectedSpeed);

        AddChild(new CartesianChart
        {
            Series = viewModel.Series
        });
    }
}
