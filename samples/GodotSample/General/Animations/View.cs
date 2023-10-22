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

        var curveOptionButton = new OptionButton();
        curveOptionButton.ItemSelected += index => viewModel.SelectedCurve = viewModel.AvalaibaleCurves[index];

        foreach (var curve in viewModel.AvalaibaleCurves.Select(p => p.Item1))
            curveOptionButton.AddItem(curve);
        curveOptionButton.Selected = viewModel.AvalaibaleCurves.ToList().IndexOf(viewModel.SelectedCurve);

        var speedOptionButton = new OptionButton();
        speedOptionButton.ItemSelected += index => viewModel.SelectedSpeed = viewModel.AvailableSpeeds[index];
        buttons.AddChild(new Label { Text = "Curve: ", SelfModulate = Colors.Black });
        buttons.AddChild(curveOptionButton);

        foreach (var speed in viewModel.AvailableSpeeds.Select(p => p.Item1))
            speedOptionButton.AddItem(speed);
        speedOptionButton.Selected = viewModel.AvailableSpeeds.ToList().IndexOf(viewModel.SelectedSpeed);
        buttons.AddChild(new Label { Text = "Speed: ", SelfModulate = Colors.Black });
        buttons.AddChild(speedOptionButton);

        AddChild(buttons);
        AddChild(new CartesianChart
        {
            Series = viewModel.Series
        });
    }
}
