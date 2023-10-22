using System;
using Godot;

public partial class MainScene : Control
{
	[Export] public NodePath SampleListPath { get; set; } = null!;
	[Export] public NodePath ViewContainerPath { get; set; } = null!;

	public override void _Ready()
	{
		var sampleList = GetNode<ItemList>(SampleListPath);
		var viewContainer = GetNode<Control>(ViewContainerPath);

		var samples = ViewModelsSamples.Index.Samples;
	
		foreach (var sample in samples)
			_ = sampleList.AddItem(sample);

		Control? currentView = null;
		sampleList.ItemSelected += sampleIndex =>
		{
			if (currentView is not null)
			{
				viewContainer.RemoveChild(currentView);
				currentView.QueueFree();
			}

			var sample = samples[(int)sampleIndex];
			var sampleTypeName = $"GodotSample.{sample.Replace('/', '.')}.View";
			currentView = (Control)Activator.CreateInstance(null, sampleTypeName).Unwrap();
			viewContainer.AddChild(currentView);
		};
	}
}
