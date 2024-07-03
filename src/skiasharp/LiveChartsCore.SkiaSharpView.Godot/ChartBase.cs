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

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using Godot;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Kernel.Events;
using LiveChartsCore.Kernel.Sketches;
using LiveChartsCore.Measure;
using LiveChartsCore.Motion;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.SKCharts;
using LiveChartsCore.VisualElements;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <inheritdoc cref="IChartView{TDrawingContext}" />
public abstract partial class ChartBase : ChartAndMapBase, IChartView<SkiaSharpDrawingContext>
{
    #region Properties
    /// <inheritdoc cref="IChartView.DrawMargin" />
    public Margin? DrawMargin
    {
        get => _drawMargin;
        set
        {
            _drawMargin = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView.AnimationsSpeed" />
    public TimeSpan AnimationsSpeed
    {
        get => _animationSpeed;
        set
        {
            _animationSpeed = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView.EasingFunction" />
    public Func<float, float>? EasingFunction
    {
        get => _easingFunction;
        set
        {
            _easingFunction = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView.LegendPosition" />
    public LegendPosition LegendPosition
    {
        get => _legendPosition;
        set
        {
            _legendPosition = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView.TooltipPosition" />
    public TooltipPosition TooltipPosition
    {
        get => _tooltipPosition;
        set
        {
            _tooltipPosition = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.TooltipBackgroundPaint" />
    public IPaint<SkiaSharpDrawingContext>? TooltipBackgroundPaint
    {
        get => _tooltipBackgroundPaint;
        set
        {
            _tooltipBackgroundPaint = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.TooltipTextPaint" />
    public IPaint<SkiaSharpDrawingContext>? TooltipTextPaint
    {
        get => _tooltipTextPaint;
        set
        {
            _tooltipTextPaint = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.LegendBackgroundPaint" />
    public IPaint<SkiaSharpDrawingContext>? LegendBackgroundPaint
    {
        get => _legendBackgroundPaint;
        set
        {
            _legendBackgroundPaint = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.LegendTextPaint" />
    public IPaint<SkiaSharpDrawingContext>? LegendTextPaint
    {
        get => _legendTextPaint;
        set
        {
            _legendTextPaint = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.LegendTextSize" />
    public double? LegendTextSize
    {
        get => _legendTextSize;
        set
        {
            _legendTextSize = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.TooltipTextSize" />
    public double? TooltipTextSize
    {
        get => _tooltipTextSize;
        set
        {
            _tooltipTextSize = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView.SyncContext" />
    public object SyncContext
    {
        get => CoreCanvas.Sync;
        set
        {
            CoreCanvas.Sync = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.VisualElements" />
    public IEnumerable<ChartElement<SkiaSharpDrawingContext>> VisualElements
    {
        get => _visualElements;
        set
        {
            _visualElementsObserver.Dispose(_visualElements);

            _visualElements = value;
            _visualElementsObserver.Initialize(_visualElements);

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.CoreCanvas" />
    public MotionCanvas<SkiaSharpDrawingContext> CoreCanvas => canvas.CoreCanvas;

    /// <inheritdoc cref="IChartView.CoreChart" />
    public IChart CoreChart => coreChart;

    /// <inheritdoc cref="IChartView{TDrawingContext}.Tooltip" />
    public IChartTooltip<SkiaSharpDrawingContext>? Tooltip { get => tooltip; set => tooltip = value; }

    /// <inheritdoc cref="IChartView{TDrawingContext}.Legend" />
    public IChartLegend<SkiaSharpDrawingContext>? Legend { get => legend; set => legend = value; }

    /// <inheritdoc cref="IChartView{TDrawingContext}.Title" />
    public VisualElement<SkiaSharpDrawingContext>? Title { get; set; }

    /// <inheritdoc cref="IChartView{TDrawingContext}.AutoUpdateEnabled" />
    public bool AutoUpdateEnabled { get; set; } = true;

    /// <inheritdoc cref="IChartView.UpdaterThrottler" />
    public TimeSpan UpdaterThrottler { get; set; } = LiveCharts.DefaultSettings.UpdateThrottlingTimeout;
    #endregion

    #region IChartView properties
    /// <inheritdoc cref="IChartView.DesignerMode" />
    bool IChartView.DesignerMode => false;

    Margin? IChartView.DrawMargin
    {
        get => DrawMargin;
        set => DrawMargin = value;
    }

    TimeSpan IChartView.AnimationsSpeed
    {
        get => AnimationsSpeed;
        set => AnimationsSpeed = value;
    }

    Func<float, float>? IChartView.EasingFunction
    {
        get => EasingFunction;
        set => EasingFunction = value;
    }

    LvcSize IChartView.ControlSize => new() { Width = Size.X, Height = Size.Y };

    LvcColor IChartView.BackColor
    {
        get => new((byte)Background.R8, (byte)Background.G8, (byte)Background.B8, (byte)Background.A8);
        set => Background = new(value.R, value.G, value.B, value.A);
    }
    #endregion

    #region Commands
    /// <summary>
    /// Gets or sets a command to execute when the chart update started.
    /// </summary>
    public ICommand? UpdateStartedCommand { get; set; }

    /// <summary>
    /// Gets or sets a command to execute when the pointer goes down on the chart.
    /// </summary>
    public ICommand? PointerPressedCommand { get; set; }

    /// <summary>
    /// Gets or sets a command to execute when the pointer goes up on the chart.
    /// </summary>
    public ICommand? PointerReleasedCommand { get; set; }

    /// <summary>
    /// Gets or sets a command to execute when the pointer moves over the chart.
    /// </summary>
    public ICommand? PointerMoveCommand { get; set; }

    /// <summary>
    /// Gets or sets a command to execute when a double click happens on a chart.
    /// </summary>
    public ICommand? DoubleClickCommand { get; set; }

    /// <summary>
    /// Gets or sets a command to execute when the pointer goes down on a data or data points.
    /// </summary>
    public ICommand? DataPointerDownCommand { get; set; }

    /// <summary>
    /// Gets or sets a command to execute when the pointer goes down on a chart point.
    /// </summary>
    public ICommand? ChartPointPointerDownCommand { get; set; }

    /// <summary>
    /// Gets or sets a command to execute when the pointer goes down on a visual element.
    /// </summary>
    public ICommand? VisualElementsPointerDownCommand { get; set; }
    #endregion

    #region Events
    /// <inheritdoc cref="IChartView{TDrawingContext}.Measuring" />
    public event ChartEventHandler<SkiaSharpDrawingContext>? Measuring;

    /// <inheritdoc cref="IChartView{TDrawingContext}.UpdateStarted" />
    public event ChartEventHandler<SkiaSharpDrawingContext>? UpdateStarted;

    /// <inheritdoc cref="IChartView{TDrawingContext}.UpdateFinished" />
    public event ChartEventHandler<SkiaSharpDrawingContext>? UpdateFinished;

    /// <inheritdoc cref="IChartView.DataPointerDown" />
    public event ChartPointsHandler? DataPointerDown;

    /// <inheritdoc cref="IChartView.ChartPointPointerDown" />
    public event ChartPointHandler? ChartPointPointerDown;

    /// <inheritdoc cref="IChartView{TDrawingContext}.VisualElementsPointerDown"/>
    public event VisualElementsHandler<SkiaSharpDrawingContext>? VisualElementsPointerDown;
    #endregion

    #region Protected fields
    /// <summary>
    /// Gets the core chart.
    /// </summary>
    protected Chart<SkiaSharpDrawingContext> coreChart { get; set; } = null!;

    /// <summary>
    /// Gets the legend.
    /// </summary>
    protected IChartLegend<SkiaSharpDrawingContext>? legend { get; set; }

    /// <summary>
    /// Gets the tool tip.
    /// </summary>
    protected IChartTooltip<SkiaSharpDrawingContext>? tooltip { get; set; }
    #endregion

    #region Private fields
    private IEnumerable<ChartElement<SkiaSharpDrawingContext>> _visualElements = null!;
    private readonly CollectionDeepObserver<ChartElement<SkiaSharpDrawingContext>> _visualElementsObserver;

    private Margin? _drawMargin;

    private TimeSpan _animationSpeed;
    private Func<float, float>? _easingFunction;

    private TooltipPosition _tooltipPosition;
    private LegendPosition _legendPosition;

    private IPaint<SkiaSharpDrawingContext>? _tooltipBackgroundPaint;
    private IPaint<SkiaSharpDrawingContext>? _tooltipTextPaint;
    private IPaint<SkiaSharpDrawingContext>? _legendBackgroundPaint;
    private IPaint<SkiaSharpDrawingContext>? _legendTextPaint;

    private double? _legendTextSize;
    private double? _tooltipTextSize;
    #endregion

    #region Setting up
    /// <summary>
    /// Initializes a new instance of the <see cref="ChartBase"/> class.
    /// </summary>
    protected ChartBase()
    {
        InitializeCoreChartCore();

        coreChart.Measuring += OnCoreMeasuring;
        coreChart.UpdateStarted += OnCoreUpdateStarted;
        coreChart.UpdateFinished += OnCoreUpdateFinished;

        legend = new SKDefaultLegend();
        tooltip = new SKDefaultTooltip();

        _visualElementsObserver = new CollectionDeepObserver<ChartElement<SkiaSharpDrawingContext>>
        (
            OnDeepCollectionChanged,
            OnDeepCollectionPropertyChanged,
            true
        );

        _animationSpeed = LiveCharts.DefaultSettings.AnimationsSpeed;
        _easingFunction = LiveCharts.DefaultSettings.EasingFunction;

        _legendPosition = LiveCharts.DefaultSettings.LegendPosition;
        _legendBackgroundPaint = LiveCharts.DefaultSettings.LegendBackgroundPaint as IPaint<SkiaSharpDrawingContext>;
        _legendTextPaint = LiveCharts.DefaultSettings.LegendTextPaint as IPaint<SkiaSharpDrawingContext>;
        _legendTextSize = LiveCharts.DefaultSettings.LegendTextSize;

        _tooltipPosition = LiveCharts.DefaultSettings.TooltipPosition;
        _tooltipBackgroundPaint = LiveCharts.DefaultSettings.TooltipBackgroundPaint as IPaint<SkiaSharpDrawingContext>;
        _tooltipTextPaint = LiveCharts.DefaultSettings.TooltipTextPaint as IPaint<SkiaSharpDrawingContext>;
        _tooltipTextSize = LiveCharts.DefaultSettings.TooltipTextSize;
    }

    /// <summary>
    /// Initializes the core chart.
    /// </summary>
    protected abstract void InitializeCoreChart();

    private void InitializeCoreChartCore()
    {
        InitializeCoreChart();

        if (coreChart is null)
            throw new InvalidOperationException(
                $"The {nameof(coreChart)} property must be initialized in the {nameof(InitializeCoreChart)} method.");
    }
    #endregion

    #region Event handlers
    protected void OnDeepCollectionChanged(object? _, NotifyCollectionChangedEventArgs __)
    {
        coreChart.Update();
    }

    protected void OnDeepCollectionPropertyChanged(object? _, PropertyChangedEventArgs __)
    {
        coreChart.Update();
    }

    public override void _EnterTree()
    {
        coreChart.Load();
    }

    public override void _TreeExiting()
    {
        coreChart.Unload();
    }

    public override void _Resized()
    {
        coreChart.Update();
    }

    #region Mouse event handlers
    public override void _MouseDown(InputEventMouseButton mouseButtonEvent)
    {
        if (PointerPressedCommand is null)
            return;

        var mousePos = mouseButtonEvent.Position;
        var args = new PointerCommandArgs(this, new(mousePos.X, mousePos.Y), mouseButtonEvent);
        if (PointerPressedCommand.CanExecute(args))
            PointerPressedCommand.Execute(args);
    }

    public override void _MouseDoubleClick(InputEventMouseButton mouseButtonEvent)
    {
        if (DoubleClickCommand is null)
            return;

        var mousePos = mouseButtonEvent.Position;
        var args = new PointerCommandArgs(this, new(mousePos.X, mousePos.Y), mouseButtonEvent);
        if (DoubleClickCommand.CanExecute(args))
            DoubleClickCommand.Execute(args);
    }

    public override void _MouseMoved(InputEventMouseMotion mouseMotionEvent)
    {
        var mousePos = mouseMotionEvent.Position;

        if (PointerMoveCommand is not null)
        {
            var args = new PointerCommandArgs(this, new(mousePos.X, mousePos.Y), mouseMotionEvent);
            if (PointerMoveCommand.CanExecute(args))
                PointerMoveCommand.Execute(args);
        }

        coreChart.InvokePointerMove(new(mousePos.X, mousePos.Y));
    }

    public override void _MouseUp(InputEventMouseButton mouseButtonEvent)
    {
        if (PointerReleasedCommand is null)
            return;

        var mousePos = mouseButtonEvent.Position;
        var args = new PointerCommandArgs(this, new(mousePos.X, mousePos.Y), mouseButtonEvent);
        if (PointerReleasedCommand.CanExecute(args))
            PointerReleasedCommand.Execute(args);
    }

    public override void _MouseExited()
    {
        coreChart.InvokePointerLeft();
    }
    #endregion

    #region Core events
    private void OnCoreMeasuring(IChartView<SkiaSharpDrawingContext> _)
    {
        Measuring?.Invoke(this);
    }

    private void OnCoreUpdateFinished(IChartView<SkiaSharpDrawingContext> _)
    {
        UpdateFinished?.Invoke(this);
    }

    private void OnCoreUpdateStarted(IChartView<SkiaSharpDrawingContext> _)
    {
        if (UpdateStartedCommand is not null)
        {
            var args = new ChartCommandArgs(this);
            if (UpdateStartedCommand.CanExecute(args))
                UpdateStartedCommand.Execute(args);
        }

        UpdateStarted?.Invoke(this);
    }
    #endregion
    #endregion

    #region IChartView methods
    #region Abstract methods
    /// <inheritdoc cref="IChartView{TDrawingContext}.GetPointsAt(LvcPoint, TooltipFindingStrategy)"/>
    public abstract IEnumerable<ChartPoint> GetPointsAt(LvcPoint point, TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic);

    /// <inheritdoc cref="IChartView{TDrawingContext}.GetVisualsAt(LvcPoint)"/>
    public abstract IEnumerable<VisualElement<SkiaSharpDrawingContext>> GetVisualsAt(LvcPoint point);
    #endregion

    #region Event handlers
    void IChartView.OnDataPointerDown(IEnumerable<ChartPoint> points, LvcPoint pointer)
    {
        DataPointerDown?.Invoke(this, points);
        if (DataPointerDownCommand is not null && DataPointerDownCommand.CanExecute(points))
            DataPointerDownCommand.Execute(points);

        var closest = points.FindClosestTo(pointer);
        ChartPointPointerDown?.Invoke(this, closest);
        if (ChartPointPointerDownCommand is not null && ChartPointPointerDownCommand.CanExecute(closest))
            ChartPointPointerDownCommand.Execute(closest);
    }

    void IChartView<SkiaSharpDrawingContext>.OnVisualElementPointerDown
    (
        IEnumerable<VisualElement<SkiaSharpDrawingContext>> visualElements,
        LvcPoint pointer)
    {
        var eventArgs = new VisualElementsEventArgs<SkiaSharpDrawingContext>(CoreChart, visualElements, pointer);

        VisualElementsPointerDown?.Invoke(this, eventArgs);
        if (VisualElementsPointerDownCommand is not null && VisualElementsPointerDownCommand.CanExecute(eventArgs))
            VisualElementsPointerDownCommand.Execute(eventArgs);
    }
    #endregion

    void IChartView.InvokeOnUIThread(Action action)
    {
        Callable.From(action).CallDeferred();
    }

    void IChartView.Invalidate()
    {
        CoreCanvas.Invalidate();
    }
    #endregion
}
