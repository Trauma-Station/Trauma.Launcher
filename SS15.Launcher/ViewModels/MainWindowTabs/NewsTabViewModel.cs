using System.Collections.ObjectModel;
using CodeHollow.FeedReader;
using SS15.Launcher.Localization;

namespace SS15.Launcher.ViewModels.MainWindowTabs;

public sealed class NewsTabViewModel : MainWindowTabViewModel
{
    private bool _startedPullingNews;
    private bool _newsPulled;

    public NewsTabViewModel()
    {
        NewsEntries = new ObservableCollection<NewsEntryViewModel>(new List<NewsEntryViewModel>());

        this.WhenAnyValue(x => x.NewsPulled)
            .Subscribe(_ => this.RaisePropertyChanged(nameof(NewsNotPulled)));
    }

    public bool NewsPulled
    {
        get => _newsPulled;
        set => this.RaiseAndSetIfChanged(ref _newsPulled, value);
    }

    public bool NewsNotPulled => !NewsPulled;

    public override void Selected()
    {
        base.Selected();

        PullNews();
    }

    private async void PullNews()
    {
        if (_startedPullingNews)
        {
            return;
        }

        _startedPullingNews = true;
        var feed = await FeedReader.ReadAsync(ConfigConstants.NewsFeedUrl);

        foreach (var feedItem in feed.Items)
        {
            NewsEntries.Add(new NewsEntryViewModel(feedItem.Title, new Uri(feedItem.Link)));
        }

        NewsPulled = true;
    }

    public ObservableCollection<NewsEntryViewModel> NewsEntries { get; }

    public override string Name => LocalizationManager.Instance.GetString("tab-news-title");
}
