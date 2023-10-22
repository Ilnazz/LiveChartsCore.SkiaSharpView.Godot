using Godot;
using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Test.MotionCanvasDispose;

namespace GodotSample.Test.MotionCanvasDispose;

public partial class View : ViewBase
{
    public View()
    {
        var changeContentButton = new Button() { Text = "Change content" };

        changeContentButton.Pressed += () =>
        {
            RemoveChild(GetChild(0));
            AddChild(GenerateControls(changeContentButton));
        };

        AddChild(GenerateControls(changeContentButton));
    }

    private static Control GenerateControls(Button button)
    {
        var canvas = new MotionCanvas();

        ViewModel.Generate(canvas.CoreCanvas);

        var vBoxView = new VBoxViewBase();
        vBoxView.AddChild(button);
        vBoxView.AddChild(canvas);

        return vBoxView;
    }
}
