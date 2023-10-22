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
using LiveChartsCore.SkiaSharpView.Drawing.Geometries;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.VisualElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <inheritdoc cref="ICartesianChartView{TDrawingContext}" />
public partial class CartesianChart : Chart, ICartesianChartView<SkiaSharpDrawingContext>
{
    #region Properties
    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.Series" />
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

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.XAxes" />
    public IEnumerable<ICartesianAxis> XAxes
    {
        get => _xAxes;
        set
        {
            _xAxesObserver.Dispose(_xAxes);

            _xAxes = value;
            _xAxesObserver.Initialize(_xAxes);

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.YAxes" />
    public IEnumerable<ICartesianAxis> YAxes
    {
        get => _yAxes;
        set
        {
            _yAxesObserver.Dispose(_yAxes);

            _yAxes = value;
            _yAxesObserver.Initialize(_yAxes);

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.Sections" />
    public IEnumerable<Section<SkiaSharpDrawingContext>> Sections
    {
        get => _sections;
        set
        {
            _sectionsObserver.Dispose(_sections);

            _sections = value;
            _sectionsObserver.Initialize(_sections);

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.DrawMarginFrame" />
    public DrawMarginFrame<SkiaSharpDrawingContext>? DrawMarginFrame { get; set; }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.ZoomMode" />
    public ZoomAndPanMode ZoomMode { get; set; }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.ZoomingSpeed" />
    public double ZoomingSpeed { get; set; }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.TooltipFindingStrategy" />
    public TooltipFindingStrategy TooltipFindingStrategy
    {
        get => _tooltipFindingStrategy;
        set
        {
            _tooltipFindingStrategy = value;

            coreChart.Update();
        }
    }
    #endregion

    #region ICartesianChartView properties
    CartesianChart<SkiaSharpDrawingContext> ICartesianChartView<SkiaSharpDrawingContext>.Core =>
        (CartesianChart<SkiaSharpDrawingContext>)coreChart;

    ZoomAndPanMode ICartesianChartView<SkiaSharpDrawingContext>.ZoomMode
    {
        get => ZoomMode;
        set => ZoomMode = value;
    }

    double ICartesianChartView<SkiaSharpDrawingContext>.ZoomingSpeed
    {
        get => ZoomingSpeed;
        set => ZoomingSpeed = value;
    }
    #endregion

    #region Fields
    private readonly CollectionDeepObserver<ISeries> _seriesObserver;
    private readonly CollectionDeepObserver<ICartesianAxis> _xAxesObserver;
    private readonly CollectionDeepObserver<ICartesianAxis> _yAxesObserver;
    private readonly CollectionDeepObserver<Section<SkiaSharpDrawingContext>> _sectionsObserver;

    private IEnumerable<ISeries> _series;
    private IEnumerable<ICartesianAxis> _xAxes;
    private IEnumerable<ICartesianAxis> _yAxes;
    private IEnumerable<Section<SkiaSharpDrawingContext>> _sections;

    private TooltipFindingStrategy _tooltipFindingStrategy;
    #endregion

    #region Setting up
    /// <summary>
    /// Initializes a new instance of the <see cref="CartesianChart"/> class.
    /// </summary>
    public CartesianChart() : base()
    {
        _seriesObserver = new CollectionDeepObserver<ISeries>
        (
            OnDeepCollectionChanged,
            OnDeepCollectionPropertyChanged,
            true
        );
        _sectionsObserver = new CollectionDeepObserver<Section<SkiaSharpDrawingContext>>
        (
            OnDeepCollectionChanged,
            OnDeepCollectionPropertyChanged,
            true
        );
        _xAxesObserver = new CollectionDeepObserver<ICartesianAxis>
        (
            OnDeepCollectionChanged,
            OnDeepCollectionPropertyChanged,
            true
        );
        _yAxesObserver = new CollectionDeepObserver<ICartesianAxis>
        (
            OnDeepCollectionChanged,
            OnDeepCollectionPropertyChanged,
            true
        );

        _series = new ObservableCollection<ISeries>();
        _sections = new ObservableCollection<Section<SkiaSharpDrawingContext>>();

        _xAxes = new ObservableCollection<ICartesianAxis>()
        {
            LiveCharts.DefaultSettings.GetProvider<SkiaSharpDrawingContext>().GetDefaultCartesianAxis()
        };
        _yAxes = new ObservableCollection<ICartesianAxis>()
        {
            LiveCharts.DefaultSettings.GetProvider<SkiaSharpDrawingContext>().GetDefaultCartesianAxis()
        };

        ZoomMode = LiveCharts.DefaultSettings.ZoomMode;
        ZoomingSpeed = LiveCharts.DefaultSettings.ZoomSpeed;
        TooltipFindingStrategy = LiveCharts.DefaultSettings.TooltipFindingStrategy;

        VisualElements = new ObservableCollection<ChartElement<SkiaSharpDrawingContext>>();
    }

    protected override void InitializeCoreChart()
    {
        var zoomingSection = new RectangleGeometry();
        var zoomingSectionPaint = new SolidColorPaint
        {
            IsFill = true,
            Color = new SkiaSharp.SKColor(33, 150, 243, 50),
            ZIndex = int.MaxValue
        };
        zoomingSectionPaint.AddGeometryToPaintTask(CoreCanvas, zoomingSection);
        CoreCanvas.AddDrawableTask(zoomingSectionPaint);

        coreChart = new CartesianChart<SkiaSharpDrawingContext>
        (
            this,
            config => config.UseDefaults(),
            CoreCanvas,
            zoomingSection
        );
    }
    #endregion

    #region Event handlers
    public override void _MouseDown(InputEventMouseButton mouseButtonEvent)
    {
        base._MouseDown(mouseButtonEvent);

        var isModifierPressed =
            mouseButtonEvent.CtrlPressed
            || mouseButtonEvent.AltPressed
            || mouseButtonEvent.MetaPressed
            || mouseButtonEvent.ShiftPressed;

        if (isModifierPressed)
            return;

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

    public override void _MouseWheel(InputEventMouseButton mouseButtonEvent)
    {
        var cartesianChart = (CartesianChart<SkiaSharpDrawingContext>)coreChart;

        var mousePos = mouseButtonEvent.Position;
        var zoomDirection = mouseButtonEvent.ButtonIndex == MouseButton.WheelUp
            ? ZoomDirection.ZoomIn
            : ZoomDirection.ZoomOut;

        cartesianChart.Zoom(new LvcPoint(mousePos.X, mousePos.Y), zoomDirection);
    }
    #endregion

    #region Methods
    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.ScaleUIPoint(LvcPoint, int, int)" />
    [Obsolete("Use the ScalePixelsToData method instead.")]
    public double[] ScaleUIPoint(LvcPoint point, int xAxisIndex = 0, int yAxisIndex = 0)
    {
        var cartesianChart = (CartesianChart<SkiaSharpDrawingContext>)coreChart;
        return cartesianChart.ScaleUIPoint(point, xAxisIndex, yAxisIndex);
    }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.ScalePixelsToData(LvcPointD, int, int)"/>
    public LvcPointD ScalePixelsToData(LvcPointD point, int xAxisIndex = 0, int yAxisIndex = 0)
    {
        var cartesianChart = (CartesianChart<SkiaSharpDrawingContext>)coreChart;

        var xScaler = new Scaler(cartesianChart.DrawMarginLocation, cartesianChart.DrawMarginSize, cartesianChart.XAxes[xAxisIndex]);
        var yScaler = new Scaler(cartesianChart.DrawMarginLocation, cartesianChart.DrawMarginSize, cartesianChart.YAxes[yAxisIndex]);

        return new LvcPointD { X = xScaler.ToChartValues(point.X), Y = yScaler.ToChartValues(point.Y) };
    }

    /// <inheritdoc cref="ICartesianChartView{TDrawingContext}.ScaleDataToPixels(LvcPointD, int, int)"/>
    public LvcPointD ScaleDataToPixels(LvcPointD point, int xAxisIndex = 0, int yAxisIndex = 0)
    {
        var cartesianChart = (CartesianChart<SkiaSharpDrawingContext>)coreChart;

        var xScaler = new Scaler(cartesianChart.DrawMarginLocation, cartesianChart.DrawMarginSize, cartesianChart.XAxes[xAxisIndex]);
        var yScaler = new Scaler(cartesianChart.DrawMarginLocation, cartesianChart.DrawMarginSize, cartesianChart.YAxes[yAxisIndex]);

        return new LvcPointD { X = xScaler.ToPixels(point.X), Y = yScaler.ToPixels(point.Y) };
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.GetPointsAt(LvcPoint, TooltipFindingStrategy)"/>
    public override IEnumerable<ChartPoint> GetPointsAt(LvcPoint point, TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic)
    {
        var cartesianChart = (CartesianChart<SkiaSharpDrawingContext>)coreChart;

        if (strategy == TooltipFindingStrategy.Automatic)
            strategy = cartesianChart.Series.GetTooltipFindingStrategy();

        return cartesianChart.Series.SelectMany(series => series.FindHitPoints(cartesianChart, point, strategy));
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.GetVisualsAt(LvcPoint)"/>
    public override IEnumerable<VisualElement<SkiaSharpDrawingContext>> GetVisualsAt(LvcPoint point)
    {
        var cartesianChart = (CartesianChart<SkiaSharpDrawingContext>)coreChart;

        return cartesianChart.VisualElements.SelectMany(visual =>
            ((VisualElement<SkiaSharpDrawingContext>)visual).IsHitBy(cartesianChart, point));
    }
    #endregion
}
