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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <inheritdoc cref="IPieChartView{TDrawingContext}" />
public partial class PieChart : ChartBase, IPieChartView<SkiaSharpDrawingContext>
{
    #region Properties
    /// <inheritdoc cref="IPieChartView{TDrawingContext}.IsClockwise" />
    public bool IsClockwise
    {
        get => _isClockwise;
        set
        {
            _isClockwise = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPieChartView{TDrawingContext}.InitialRotation" />
    public double InitialRotation
    {
        get => _initialRotation;
        set
        {
            _initialRotation = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPieChartView{TDrawingContext}.MaxAngle" />
    public double MaxAngle
    {
        get => _maxAngle;
        set
        {
            _maxAngle = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPieChartView{TDrawingContext}.Total" />
    [Obsolete($"Use {nameof(MaxValue)} instead.")]
    public double? Total
    {
        get => MaxValue;
        set => MaxValue = value;
    }

    /// <inheritdoc cref="IPieChartView{TDrawingContext}.MaxValue" />
    public double? MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPieChartView{TDrawingContext}.MinValue" />
    public double MinValue
    {
        get => _minValue;
        set
        {
            _minValue = value;

            coreChart.Update();
        }
    }

    /// <inheritdoc cref="IPieChartView{TDrawingContext}.Series" />
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
    #endregion

    PieChart<SkiaSharpDrawingContext> IPieChartView<SkiaSharpDrawingContext>.Core => (PieChart<SkiaSharpDrawingContext>)coreChart;

    #region Fields
    private readonly CollectionDeepObserver<ISeries> _seriesObserver;
    private IEnumerable<ISeries> _series = null!;

    private bool _isClockwise;
    private double _initialRotation;
    private double _maxAngle;
    private double? _maxValue;
    private double _minValue;
    #endregion

    #region Setting up
    /// <summary>
    /// Initializes a new instance of the <see cref="PieChart"/> class.
    /// </summary>
    public PieChart()
    {
        _seriesObserver = new CollectionDeepObserver<ISeries>
        (
            OnDeepCollectionChanged,
            OnDeepCollectionPropertyChanged,
            true
        );

        _isClockwise = true;
        _maxAngle = 360;

        Series = new ObservableCollection<ISeries>();

        VisualElements = new ObservableCollection<ChartElement<SkiaSharpDrawingContext>>();
    }

    protected override void InitializeCoreChart()
    {
        coreChart = new PieChart<SkiaSharpDrawingContext>(this, config => config.UseDefaults(), CoreCanvas);
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
    #endregion

    #region Methods
    /// <inheritdoc cref="IChartView{TDrawingContext}.GetPointsAt(LvcPoint, TooltipFindingStrategy)"/>
    public override IEnumerable<ChartPoint> GetPointsAt(LvcPoint point, TooltipFindingStrategy strategy = TooltipFindingStrategy.Automatic)
    {
        var pieChart = (PieChart<SkiaSharpDrawingContext>)coreChart;

        if (strategy == TooltipFindingStrategy.Automatic)
            strategy = pieChart.Series.GetTooltipFindingStrategy();

        return pieChart.Series.SelectMany(series => series.FindHitPoints(pieChart, point, strategy));
    }

    /// <inheritdoc cref="IChartView{TDrawingContext}.GetVisualsAt(LvcPoint)"/>
    public override IEnumerable<VisualElement<SkiaSharpDrawingContext>> GetVisualsAt(LvcPoint point)
    {
        var pieChart = (PieChart<SkiaSharpDrawingContext>)coreChart;

        return pieChart.VisualElements.SelectMany(visualElement =>
            ((VisualElement<SkiaSharpDrawingContext>)visualElement).IsHitBy(coreChart, point));
    }
    #endregion
}
