using System.Threading.Tasks;
using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Lines.AutoUpdate;

namespace GodotSample.Lines.AutoUpdate;

public partial class View : VBoxViewBase
{
    private readonly ViewModel _viewModel;
    private bool _isStreaming;

    public View()
    {
        _viewModel = new ViewModel();

        var buttonsBox = new HBoxContainer();
        AddChild(buttonsBox);

        var addButton = new Button { Text = "Add" };
        addButton.Pressed += _viewModel.AddItem;
        buttonsBox.AddChild(addButton);

        var removeButton = new Button { Text = "Remove" };
        removeButton.Pressed += _viewModel.RemoveItem;
        buttonsBox.AddChild(removeButton);

        var updateButton = new Button { Text = "Update" };
        updateButton.Pressed += _viewModel.UpdateItem;
        buttonsBox.AddChild(updateButton);

        var replaceButton = new Button { Text = "Replace" };
        replaceButton.Pressed += _viewModel.ReplaceItem;
        buttonsBox.AddChild(replaceButton);

        var addSeriesButton = new Button { Text = "Add series" };
        addSeriesButton.Pressed += _viewModel.AddSeries;
        buttonsBox.AddChild(addSeriesButton);

        var removeSeriesButton = new Button { Text = "Remove series" };
        removeSeriesButton.Pressed += _viewModel.RemoveSeries;
        buttonsBox.AddChild(removeSeriesButton);

        var constantChangesButton = new Button { Text = "Constant changes" };
        constantChangesButton.Pressed += OnConstantChanges;
        buttonsBox.AddChild(constantChangesButton);

        AddChild(new CartesianChart
        {
            Series = _viewModel.Series
        });
    }

    private async void OnConstantChanges()
    {
        _isStreaming = !_isStreaming;

        while (_isStreaming)
        {
            _viewModel.RemoveItem();
            _viewModel.AddItem();
            await Task.Delay(1000);
        }
    }
}
