using Godot;

namespace GodotSample.VisualTest.Tabs;

public partial class View : ViewBase
{
    public View()
    {
        var tabContainer = new TabContainer();

        var tab1 = new MarginContainer { Name = "tab 1" };
        tab1.AddChild(new Bars.AutoUpdate.View());

        var tab2 = new MarginContainer { Name = "tab 2" };
        tab2.AddChild(new Lines.AutoUpdate.View());

        var tab3 = new MarginContainer { Name = "tab 3" };
        tab3.AddChild(new Scatter.AutoUpdate.View());

        AddChild(tabContainer);
    }
}
