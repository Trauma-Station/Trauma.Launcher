using System.ComponentModel;
using Avalonia.Labs.Gif;
using Splat;
using Trauma.Launcher.Models.Data;
using Trauma.Launcher.Utility;

namespace Trauma.Launcher.Views;

public sealed partial class MainWindowContent : UserControl
{
    public const int BannerScale = 1;

    private readonly DataManager _data = Locator.Current.GetRequiredService<DataManager>();

    public MainWindowContent()
    {
        InitializeComponent();

        ServerBanner.PropertyChanged += (_, args) =>
        {
            if (args.Property == GifImage.SourceProperty && ServerBanner.Source is { } source)
            {
                ServerBanner.Width = source.Size.Width * BannerScale;
                ServerBanner.Height = source.Size.Height * BannerScale;
            }
        };
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs args)
    {
        base.OnAttachedToVisualTree(args);

        _data.GetCVarEntry(CVars.ShowBanner).PropertyChanged += OnShowBannerChanged;

        UpdateVisible();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs args)
    {
        base.OnDetachedFromVisualTree(args);

        _data.GetCVarEntry(CVars.ShowBanner).PropertyChanged -= OnShowBannerChanged;
    }

    private void OnShowBannerChanged(object? sender, PropertyChangedEventArgs e)
    {
        UpdateVisible();
    }

    private void UpdateVisible()
    {
        ServerBanner.IsVisible = _data.GetCVar(CVars.ShowBanner);
    }
}
