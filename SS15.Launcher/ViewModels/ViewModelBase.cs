namespace Trauma.Launcher.ViewModels;

public abstract class ViewModelBase : ReactiveObject, IViewModelBase;

/// <summary>
/// Signifies to <see cref="ViewLocator"/> that this viewmodel can be automatically located.
/// </summary>
public interface IViewModelBase;
