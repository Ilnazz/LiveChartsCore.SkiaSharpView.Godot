﻿// The MIT License(MIT)
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
using Godot;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <summary>
/// Provides base implementation for charts and maps.
/// </summary>
public abstract partial class ChartAndMapBase : Panel
{
    #region Properties
    public Color Background
    {
        get => StyleBox.BgColor;
        set => StyleBox.BgColor = value;
    }

    public StyleBoxFlat StyleBox
    {
        get => (StyleBoxFlat)GetThemeStylebox("panel");
        set => AddThemeStyleboxOverride("panel", value);
    }
    #endregion

    #region Events
    /// <summary>
    /// Occurs when the mouse button is pressed.
    /// </summary>
    public event Action<InputEventMouseButton>? MouseDown;

    /// <summary>
    /// Occurs when the mouse button is pressed twice.
    /// </summary>
    public event Action<InputEventMouseButton>? MouseDoubleClick;

    /// <summary>
    /// Occurs when the mouse button is released.
    /// </summary>
    public event Action<InputEventMouseButton>? MouseUp;

    /// <summary>
    /// Occurs when the mouse wheel is moved.
    /// </summary>
    public event Action<InputEventMouseButton>? MouseWheel;

    /// <summary>
    /// Occurs when the mouse is moved.
    /// </summary>
    public event Action<InputEventMouseMotion>? MouseMoved;
    #endregion

    /// <summary>
    /// Gets the canvas.
    /// </summary>
    protected MotionCanvas canvas { get; private set; }

    protected ChartAndMapBase()
    {
        LiveCharts.Configure(config => config.UseDefaults());

        Background = default;

        LayoutMode = 1;
        AnchorsPreset = (int)LayoutPreset.FullRect;

        SizeFlagsHorizontal = SizeFlags.ExpandFill;
        SizeFlagsVertical = SizeFlags.ExpandFill;

        FocusMode = FocusModeEnum.Click;

        canvas = new MotionCanvas
        {
            LayoutMode = 1,
            AnchorsPreset = (int)LayoutPreset.FullRect,
            MouseFilter = MouseFilterEnum.Ignore,
            FocusMode = FocusModeEnum.None
        };
        AddChild(canvas);

        GuiInput += OnGuiInput;

        TreeEntered += _TreeEntered;
        TreeExiting += _TreeExiting;
        TreeExited += _TreeExited;

        Resized += _Resized;

        MouseEntered += _MouseEntered;
        MouseExited += _MouseExited;
        MouseMoved += _MouseMoved;
        MouseDown += _MouseDown;
        MouseDoubleClick += _MouseDoubleClick;
        MouseUp += _MouseUp;
        MouseWheel += _MouseWheel;
    }

    #region Event handlers
    public virtual void _TreeEntered() { }
    public virtual void _TreeExiting() { }
    public virtual void _TreeExited() { }

    public virtual void _Resized() { }

    public virtual void _MouseEntered() { }
    public virtual void _MouseExited() { }
    public virtual void _MouseMoved(InputEventMouseMotion mouseMotionEvent) { }
    public virtual void _MouseDown(InputEventMouseButton mouseButtonEvent) { }
    public virtual void _MouseDoubleClick(InputEventMouseButton mouseButtonEvent) { }
    public virtual void _MouseUp(InputEventMouseButton mouseButtonEvent) { }
    public virtual void _MouseWheel(InputEventMouseButton mouseButtonEvent) { }
    #endregion

    private void OnGuiInput(InputEvent @event)
    {
        switch (@event)
        {
            case InputEventMouseMotion mouseMotionEvent:
                MouseMoved?.Invoke(mouseMotionEvent);
                break;

            case InputEventMouseButton { ButtonIndex: MouseButton.WheelUp or MouseButton.WheelDown } mouseButtonEvent:
                MouseWheel?.Invoke(mouseButtonEvent);
                break;

            case InputEventMouseButton { Pressed: true } mouseButtonEvent:
                MouseDown?.Invoke(mouseButtonEvent);
                break;

            case InputEventMouseButton { DoubleClick: true } mouseButtonEvent:
                MouseDoubleClick?.Invoke(mouseButtonEvent);
                break;

            case InputEventMouseButton mouseButtonEvent:
                MouseUp?.Invoke(mouseButtonEvent);
                break;
        }
    }
}
