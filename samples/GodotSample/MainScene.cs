using System;
using System.Reflection;
using Godot;
using LiveChartsCore;
using LiveChartsCore.Geo;
using LiveChartsCore.SkiaSharpView.Godot;

public partial class MainScene : Control
{
    [Export] public NodePath SampleListPath { get; set; } = null!;

    [Export] public NodePath ViewContainerPath { get; set; } = null!;

	public MainScene()
	{
		LiveCharts.Configure(config => // mark
			 config // mark
			// you can override the theme
			// .AddDarkTheme() // mark

			// In case you need a non-Latin based font, you must register a typeface for SkiaSharp
			//.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('汉')) // <- Chinese // mark
			//.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('あ')) // <- Japanese // mark
			//.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('헬')) // <- Korean // mark
			//.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('Ж'))  // <- Russian // mark

			//.HasGlobalSKTypeface(SKFontManager.Default.MatchCharacter('أ'))  // <- Arabic // mark
			//.UseRightToLeftSettings() // Enables right to left tooltips // mark

			// finally register your own mappers
			// you can learn more about mappers at:
			// https://livecharts.dev/docs/{{ platform }}/{{ version }}/Overview.Mappers
			.HasMap<City>((city, point) => // mark
			{ // mark
				// here we use the index as X, and the population as Y // mark
				point.Coordinate = new(point.Index, city.Population); // mark
			}) // mark
				// .HasMap<Foo>( .... ) // mark
				// .HasMap<Bar>( .... ) // mark
		); // mark
	}

	public override void _Ready()
    {
		var sampleList = GetNode<ItemList>(SampleListPath);
		var viewContainer = GetNode<Control>(ViewContainerPath);

		var samples = ViewModelsSamples.Index.Samples;

		foreach (var sample in samples)
			_ = sampleList.AddItem(sample);

		Control currentView = null;

        ShowSample(0);

        sampleList.ItemSelected += longSampleIndex => ShowSample((int)longSampleIndex);
        return;

        void ShowSample(int sampleIndex)
        {
            if (currentView is not null)
            {
                viewContainer.RemoveChild(currentView);
                currentView.QueueFree();
            }

            var sample = samples[sampleIndex];
            var sampleTypeName = $"GodotSample.{sample.Replace('/', '.')}.View";
            currentView = (Control)Activator.CreateInstance(null, sampleTypeName)!.Unwrap();
            viewContainer.AddChild(currentView);
        }
    }

    public record City(string Name, double Population);
}
