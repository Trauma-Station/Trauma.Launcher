namespace SS15.Launcher.ViewModels.Login;

public abstract partial class BaseLoginViewModel : ViewModelBase, IErrorOverlayOwner
{
    [Reactive] public partial bool Busy { get; protected set; }
    [Reactive] public partial string? BusyText { get; protected set; }
    [Reactive] public ViewModelBase? _overlayControl;
    public MainWindowLoginViewModel ParentVM { get; }

    protected BaseLoginViewModel(MainWindowLoginViewModel parentVM)
    {
        ParentVM = parentVM;
    }

    public virtual void Activated()
    {
    }

    public virtual void OverlayOk()
    {
        OverlayControl = null;
    }
}
