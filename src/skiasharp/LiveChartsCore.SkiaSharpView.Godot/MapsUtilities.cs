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

using System.IO;
using System.Reflection;
using Godot;
using LiveChartsCore.Drawing;
using LiveChartsCore.Geo;

namespace LiveChartsCore.SkiaSharpView.Godot;

/// <summary>
/// Provides additional utility methods to work with maps.
/// </summary>
/// <seealso cref="Maps"/>
public static class MapsUtilities
{
    private const string AssemblyNameContainingWorldMap =
        $"{nameof(LiveChartsCore)}.{nameof(SkiaSharpView)}.{nameof(Godot)}";

    private const string WorldMapResourceName =
        $"{AssemblyNameContainingWorldMap}.Resources.world.geojson";

    /// <summary>
    /// Gets the world map containing in <see cref="LiveChartsCore.SkiaSharpView.Godot"/> assembly.
    /// </summary>
    /// <returns>The map.</returns>
    public static CoreMap<TDrawingContext> GetWorldMap<TDrawingContext>()
        where TDrawingContext : DrawingContext
    {
        var lvcCoreAssembly = Assembly.Load(AssemblyNameContainingWorldMap);
        var worldMapResourceStream = lvcCoreAssembly.GetManifestResourceStream(WorldMapResourceName)!;
        using var worldMapResourceStreamReader = new StreamReader(worldMapResourceStream);
        return Maps.GetMapFromStreamReader<TDrawingContext>(worldMapResourceStreamReader);
    }
}
