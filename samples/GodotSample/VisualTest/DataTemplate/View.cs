﻿using Godot;

namespace GodotSample.VisualTest.DataTemplate;

public partial class View : ViewBase
{
    public View()
    {
        AddChild(new Label
        {
            Text = "Data templates are not supported in Godot."
        });
    }
}
