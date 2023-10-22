using LiveChartsCore.SkiaSharpView.Godot;
using ViewModelsSamples.Maps.World;

namespace GodotSample.Maps.World;

public partial class View : ViewBase
{
    public View()
    {
        var viewModel = new ViewModel();

        AddChild(new GeoMap
        {
            Series = viewModel.Series,
            MapProjection = LiveChartsCore.Geo.MapProjection.Mercator
        });
    }
}
