namespace SS15.Launcher.Models;

public class UpdateException(string message) : Exception(message);
public sealed class NoEngineForPlatformException(string message) : UpdateException(message);
public sealed class NoModuleForPlatformException(string message) : UpdateException(message);
