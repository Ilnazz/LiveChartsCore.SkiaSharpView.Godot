// The MIT License(MIT)
//
// Copyright(c) 2021 Alberto Rodriguez Orozco & LiveCharts Contributors
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Godot;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.VisualElements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <inheritdoc cref="IPolarChartView{TDrawingContext}" />
public partial class PolarChart : Chart, IPolarChartView<SkiaSharpDrawingContext>
{
    #region Properties
    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.FitToBounds" />
    public bool FitToBounds
    {
        get => _fitToBounds;
        set
        {
            _fitToBounds = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.TotalAngle" />
    public double TotalAngle
    {
        get => _totalAngle;
        set
        {
            _totalAngle = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.InnerRadius" />
    public double InnerRadius
    {
        get => _innerRadius;
        set
        {
            _innerRadius = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.InitialRotation" />
    public double InitialRotation
    {
        get => _initialRotation;
        set
        {
            _initialRotation = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.Series" />
    public IEnumerable<ISeries> Series
    {
        get => _series;
        set
        {
            _seriesObserver.Dispose(_series);

            _series = value;
            _seriesObserver.Initialize(_series);

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.AngleAxes" />
    public IEnumerable<IPolarAxis> AngleAxes
    {
        get => _angleAxes;
        set
        {
            _angleObserver.Dispose(_angleAxes);

            _angleAxes = value;
            _angleObserver.Initialize(_angleAxes);

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.RadiusAxes" />
    public IEnumerable<IPolarAxis> RadiusAxes
    {
        get => _radiusAxes;
        set
        {
            _radiusObserver.Dispose(_radiusAxes);

            _radiusAxes = value;
            _radiusObserver.Initialize(_radiusAxes);

            coreChart.Update();
        }
    }
    #endregion

    PolarChart<SkiaSharpDrawingContext> IPolarChartView<SkiaSharpDrawingContext>.Core => (PolarChart<SkiaSharpDrawingContext>)coreChart;

    #region Fields
    private readonly CollectionDeepObserver<ISeries> _seriesObserver;
    private readonly CollectionDeepObserver<IPolarAxis> _angleObserver;
    private readonly CollectionDeepObserver<IPolarAxis> _radiusObserver;

    private IEnumerable<ISeries> _series;
    private IEnumerable<IPolarAxis> _angleAxes;
    private IEnumerable<IPolarAxis> _radiusAxes;

    private bool _fitToBounds;
    private double _totalAngle;
    private double _innerRadius;
    private double _initialRotation;
    #endregion

    #region Setting up
    /// <summary>
    /// Initializes a new instance of the <see cref="PolarChart"/> class.
    /// </summary>
    public PolarChart() : base()
    {
        _seriesObserver = new CollectionDeepObserver<ISeries>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
        _angleObserver = new CollectionDeepObserver<IPolarAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);
        _radiusObserver = new CollectionDeepObserver<IPolarAxis>(OnDeepCollectionChanged, OnDeepCollectionPropertyChanged, true);

        _series = new ObservableCollection<ISeries>();
        _angleAxes = new ObservableCollection<IPolarAxis>()
        {
            LiveCharts.DefaultSettings.GetProvider<SkiaSharpDrawingContext>().GetDefaultPolarAxis()
        };
        _radiusAxes = new ObservableCollection<IPolarAxis>()
        {
            LiveCharts.DefaultSettings.GetProvider<SkiaSharpDrawingContext>().GetDefaultPolarAxis()
        };

        _totalAngle = 360;
        _initialRotation = LiveCharts.DefaultSettings.PolarInitialRotation;

        VisualElements = new ObservableCollection<ChartElement<SkiaSharpDrawingContext>>();
    }

    protected override void InitializeCoreChart()
    {
        coreChart = new PolarChart<SkiaSharpDrawingContext>(this, config => config.UseDefaults(), CoreCanvas);
    }
    #endregion

    #region Event handlers
    public override void _MouseDown(InputEventMouseButton mouseButtonEvent)
    {
        base._MouseDown(mouseButtonEvent);

        var mousePos = mouseButtonEvent.Position;
        var isSecondaryButton = mouseButtonEvent.ButtonIndex == MouseButton.Right;
        coreChart.InvokePointerDown(new LvcPoint(mousePos.X, mousePos.Y), isSecondaryButton);
    }

    public override void _MouseUp(InputEventMouseButton mouseButtonEvent)
    {
        base._MouseUp(mouseButtonEvent);

        var mousePos = mouseButtonEvent.Position;
        var isSecondaryButton = mouseButtonEvent.ButtonIndex == MouseButton.Right;
        coreChart.InvokePointerUp(new LvcPoint(mousePos.X, mousePos.Y), isSecondaryButton);
    }

    /* Zoom() in PolarChart is not implemented yet.
    public override void _MouseWheel(InputEventMouseButton mouseButtonEvent)
    {
        var polarChart = (PolarChart<SkiaSharpDrawingContext>)coreChart;

        var mousePos = mouseButtonEvent.Position;
        var zoomDirection = mouseButtonEvent.ButtonIndex == MouseButton.WheelUp
            ? ZoomDirection.ZoomIn
            : ZoomDirection.ZoomOut;

        polarChart.Zoom(new LvcPoint(mousePos.X, mousePos.Y), zoomDirection);
    }
    */
    #endregion

    #region Methods
    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.ScalePixelsToData(LvcPointD, int, int)"/>
    public LvcPointD ScalePixelsToData(LvcPointD point, int angleAxisIndex = 0, int radiusAxisIndex = 0)
    {
        var polarChart = (PolarChart<SkiaSharpDrawingContext>)coreChart;

        var scaler = new PolarScaler
        (
            polarChart.DrawMarginLocation,
            polarChart.DrawMarginSize,
            polarChart.AngleAxes[angleAxisIndex],
            polarChart.RadiusAxes[radiusAxisIndex],
            polarChart.InnerRadius,
            polarChart.InitialRotation,
            polarChart.TotalAnge
        );
        return scaler.ToChartValues(point.X, point.Y);
    }

    /// <inheritdoc cref="IPolarChartView{TDrawingContext}.ScaleDataToPixels(LvcPointD, int, int)"/>
    public LvcPointD ScaleDataToPixels(LvcPointD point, int angleAxisIndex = 0, int radiusAxisIndex = 0)
    {
        var polarChart = (PolarChart<SkiaSharpDrawingContext>)coreChart;

        var scaler = new PolarScaler
        (
            polarChart.DrawMarginLocation,
            polarChart.DrawMarginSize,
            polarChart.AngleAxes[angleAxisIndex],
            polarChart.RadiusAxes[radiusAxisIndex],
            polarChart.InnerRadius,
            polarChart.InitialRotation,
            polarChart.TotalAnge
        );
        var radius = scaler.ToPixels(point.X, point.Y);
        return new LvcPointD { X = (float)radius.X, Y = (float)radius.Y };
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.GetPointsAt(LvcPoint, TooltipFindingStrategy)"/>
    public override IEnumerable<ChartPoint> GetPointsAt(LvcPoint point, TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic)
    {
        var polarChart = (PolarChart<SkiaSharpDrawingContext>)coreChart;

        if (strategy == TooltipFindingStrategy.Automatic)
            strategy = polarChart.Series.GetTooltipFindingStrategy();

        return polarChart.Series.SelectMany(series => series.FindHitPoints(polarChart, point, strategy));
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.GetVisualsAt(LvcPoint)"/>
    public override IEnumerable<VisualElement<SkiaSharpDrawingContext>> GetVisualsAt(LvcPoint point)
    {
        var polarChart = (PolarChart<SkiaSharpDrawingContext>)coreChart;

        return polarChart.VisualElements.SelectMany(visual =>
            ((VisualElement<SkiaSharpDrawingContext>)visual).IsHitBy(coreChart, point));
    }
    #endregion
}
