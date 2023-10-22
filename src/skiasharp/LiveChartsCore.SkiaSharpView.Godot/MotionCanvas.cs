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
using System.Threading;
using System.Threading.Tasks;
using Godot;
using LiveChartsCore.Drawing;
using LiveChartsCore.Kernel;
using LiveChartsCore.Motion;
using LiveChartsCore.SkiaSharpView.Drawing;
using SkiaSharp.Views.Desktop;
using SkiaSharp.Views.Godot;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <summary>
/// The motion canvas control for Godot, <see cref="MotionCanvas{TDrawingContext}"/>.
/// </summary>
public partial class MotionCanvas : SKControl
{
    #region Properties
    /// <summary>
    /// Gets the core canvas.
    /// </summary>
    public MotionCanvas<SkiaSharpDrawingContext> CoreCanvas { get; }

    /// <summary>
    /// Gets or sets the paint tasks.
    /// </summary>
    private IList<PaintSchedule<SkiaSharpDrawingContext>> _paintTasks;
    public IList<PaintSchedule<SkiaSharpDrawingContext>> PaintTasks
    {
        get => _paintTasks;
        set
        {
            _paintTasks = value;
            OnPaintTasksChanged();
        }
    }

    /// <summary>
    /// Gets or sets the frames per second.
    /// </summary>
    public double MaxFps { get; set; }
    #endregion

    private bool _isDrawingLoopRunning;

    /// <summary>
    /// Initializes a new instance of the <see cref="MotionCanvas"/> class.
    /// </summary>
    public MotionCanvas() : base()
    {
        _paintTasks = new List<PaintSchedule<SkiaSharpDrawingContext>>();

        CoreCanvas = new MotionCanvas<SkiaSharpDrawingContext>();
        MaxFps = 60;
    }

    #region Event handlers
    public override void _EnterTree()
    {
        CoreCanvas.Invalidated += OnCanvasCoreInvalidated;
    }

    public override void _ExitTree()
    {
        CoreCanvas.Invalidated -= OnCanvasCoreInvalidated;
        CoreCanvas.Dispose();
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        CoreCanvas.DrawFrame(new SkiaSharpDrawingContext(CoreCanvas, e.Info, e.Surface, e.Surface.Canvas));
    }

    private void OnCanvasCoreInvalidated(MotionCanvas<SkiaSharpDrawingContext> _)
    {
        RunDrawingLoop();
    }
    #endregion

    #region Private methods
    private async void RunDrawingLoop()
    {
        if (_isDrawingLoopRunning)
            return;

        _isDrawingLoopRunning = true;

        var delayTimeSpan = TimeSpan.FromSeconds(1 / MaxFps);
        while (!CoreCanvas.IsValid)
        {
            QueueRedraw();
            await Task.Delay(delayTimeSpan);
        }

        _isDrawingLoopRunning = false;
    }

    private void OnPaintTasksChanged()
    {
        var tasks = new HashSet<IPaint<SkiaSharpDrawingContext>>();

        foreach (var item in PaintTasks)
        {
            item.PaintTask.SetGeometries(CoreCanvas, item.Geometries);
            _ = tasks.Add(item.PaintTask);
        }

        CoreCanvas.SetPaintTasks(tasks);
    }
    #endregion
}
