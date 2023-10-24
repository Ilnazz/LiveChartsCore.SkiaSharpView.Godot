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
using LiveChartsCore.Geo;
using LiveChartsCore.Kernel;
using LiveChartsCore.Measure;
using LiveChartsCore.Motion;
using LiveChartsCore.SkiaSharpView.Drawing;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <inheritdoc cref="IGeoMapView{TDrawingContext}"/>
public partial class GeoMap : ChartAndMapBase, IGeoMapView<SkiaSharpDrawingContext>
{
    #region Properties
    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.SyncContext" />
    public object SyncContext
    {
        get => canvas.CoreCanvas.Sync;
        set
        {
            canvas.CoreCanvas.Sync = value;

            _coreMap.Update();
        }
    }

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.ActiveMap"/>
    public CoreMap<SkiaSharpDrawingContext> ActiveMap
    {
        get => _activeMap;
        set
        {
            _activeMap = value;

            _coreMap.Update();
        }
    }

    // <inheritdoc cref="IGeoMapView{TDrawingContext}.MapProjection"/>
    public MapProjection MapProjection
    {
        get => _mapProjection;
        set
        {
            _mapProjection = value;

            _coreMap.Update();
        }
    }

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.Stroke"/>
    public IPaint<SkiaSharpDrawingContext>? Stroke
    {
        get => _stroke;
        set
        {
            _stroke = value;
            if (_stroke is not null)
                _stroke.IsStroke = true;

            _coreMap.Update();
        }
    }

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.Fill"/>
    public IPaint<SkiaSharpDrawingContext>? Fill
    {
        get => _fill;
        set
        {
            _fill = value;
            if (_fill is not null)
                _fill.IsFill = true;

            _coreMap.Update();
        }
    }

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.Series"/>
    public IEnumerable<IGeoSeries> Series
    {
        get => _series;
        set
        {
            _seriesObserver.Dispose(_series);

            _series = value;
            _seriesObserver.Initialize(_series);

            _coreMap.Update();
        }
    }

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.Canvas"/>
    public MotionCanvas<SkiaSharpDrawingContext> Canvas => canvas.CoreCanvas;

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.AutoUpdateEnabled" />
    public bool AutoUpdateEnabled { get; set; } = true;
    #endregion

    #region IGeoMapView properties
    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.DesignerMode" />
    bool IGeoMapView<SkiaSharpDrawingContext>.DesignerMode => false;

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.Width"/>
    float IGeoMapView<SkiaSharpDrawingContext>.Width => Size.X;

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.Height"/>
    float IGeoMapView<SkiaSharpDrawingContext>.Height => Size.Y;
    #endregion

    /// <inheritdoc cref="IGeoMapView{TDrawingContext}.ViewCommand" />
    public object? ViewCommand
    {
        get => _viewCommand;
        set
        {
            _viewCommand = value;
            _coreMap.ViewTo(_viewCommand);
        }
    }

    #region Fields
    private readonly GeoMap<SkiaSharpDrawingContext> _coreMap;

    private IEnumerable<IGeoSeries> _series = null!;
    private readonly CollectionDeepObserver<IGeoSeries> _seriesObserver;

    private CoreMap<SkiaSharpDrawingContext> _activeMap;
    private MapProjection _mapProjection;

    private IPaint<SkiaSharpDrawingContext>? _stroke;
    private IPaint<SkiaSharpDrawingContext>? _fill;

    private object? _viewCommand;
    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GeoMap"/> class.
    /// </summary>
    public GeoMap() : base()
    {
        _coreMap = new GeoMap<SkiaSharpDrawingContext>(this);

        _seriesObserver = new CollectionDeepObserver<IGeoSeries>
        (
            OnDeepCollectionChanged,
            OnDeepCollectionPropertyChanged,
            true
        );

        _activeMap = Maps.GetWorldMap<SkiaSharpDrawingContext>();

        _stroke = new SolidColorPaint(new SKColor(255, 255, 255, 255)) { IsStroke = true };
        _fill = new SolidColorPaint(new SKColor(240, 240, 240, 255)) { IsFill = true };

        Series = Enumerable.Empty<IGeoSeries>();
    }

    #region Event handlers
    private void OnDeepCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        _coreMap.Update();
    }
    private void OnDeepCollectionPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        _coreMap.Update();
    }

    #region Mouse event handlers
    public override void _MouseDown(InputEventMouseButton mouseButtonEvent)
    {
        base._MouseDown(mouseButtonEvent);

        var mousePos = mouseButtonEvent.Position;
        _coreMap.InvokePointerDown(new LvcPoint(mousePos.X, mousePos.Y));
    }

    public override void _MouseUp(InputEventMouseButton mouseButtonEvent)
    {
        base._MouseUp(mouseButtonEvent);

        var mousePos = mouseButtonEvent.Position;
        _coreMap.InvokePointerUp(new LvcPoint(mousePos.X, mousePos.Y));
    }

    public override void _MouseMoved(InputEventMouseMotion mouseMotionEvent)
    {
        base._MouseMoved(mouseMotionEvent);

        var mousePos = mouseMotionEvent.Position;
        _coreMap.InvokePointerMove(new LvcPoint(mousePos.X, mousePos.Y));
    }

    public override void _MouseWheel(InputEventMouseButton mouseButtonEvent)
    {
        var mousePos = mouseButtonEvent.Position;
        var zoomDirection = mouseButtonEvent.ButtonIndex == MouseButton.WheelUp
            ? ZoomDirection.ZoomIn
            : ZoomDirection.ZoomOut;

        _coreMap.ViewTo(new ZoomOnPointerView(new LvcPoint(mousePos.X, mousePos.Y), zoomDirection));
    }

    public override void _MouseExited()
    {
        _coreMap.InvokePointerLeft();
    }
    #endregion

    public override void _TreeExited()
    {
        _coreMap.Unload();
    }

    public override void _Resized()
    {
        _coreMap.Update();
    }
    #endregion

    void IGeoMapView<SkiaSharpDrawingContext>.InvokeOnUIThread(Action action)
    {
        DeferringHelper.Instance.DeferActionInvocation(action);
    }
}
