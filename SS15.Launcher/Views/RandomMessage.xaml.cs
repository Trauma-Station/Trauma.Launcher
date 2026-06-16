using Splat;
using SS15.Launcher.Models;
using SS15.Launcher.Utility;

namespace SS15.Launcher.Views;

public sealed partial class RandomMessage : UserControl
{
    public RandomMessage()
    {
        InitializeComponent();
    }

    public void Refresh()
    {
        Text.Text = Locator.Current.GetRequiredService<LauncherInfoManager>().GetRandomMessage();
    }
}
