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
using Godot;

namespace LiveChartsCore.SkiaSharpView.Godot;

// TODO: Concurrency safe
public partial class DeferringHelper : Node
{
    private static DeferringHelper s_instance;
    public static DeferringHelper Instance { get; } = s_instance ??= new();

    private readonly IList<Action> _deferredActions = new List<Action>();

    private bool _isCallDeferred;

    private readonly object _lockObject = new();

    public void DeferActionInvocation(Action action)
    {
        lock (_lockObject)
        {
            _deferredActions.Add(action);

            if (_isCallDeferred == false)
            {
                _ = CallDeferred("InvokeDeferredActions");
                _isCallDeferred = true;
            }
        }
    }

    private void InvokeDeferredActions()
    {
        lock (_lockObject)
        {
            foreach (var action in _deferredActions)
                action.Invoke();

            _deferredActions.Clear();

            _isCallDeferred = false;
        }
    }
}
