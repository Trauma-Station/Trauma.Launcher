using Splat;
using Trauma.Launcher.Models;
using Trauma.Launcher.Utility;

namespace Trauma.Launcher.Views;

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
