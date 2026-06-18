# Space Station 15 Launcher

This is the launcher you should be using to connect to Space Station 15.
It currently supports:
- managing different engines
- multiple auth servers so you can play on your favourite servers no matter what
- epic server banner at the top of the launcher!

Nix flake is currently broken, unlucky.

# Development

Useful environment variable for development:
* `SS14_LAUNCHER_APPDATA_NAME=launcherTest` to change the user data directories the launcher stores its data in. This can be useful to avoid breaking your "normal" TS14 launcher data while developing something.

# License

Space Station 15's Launcher is licensed under the [GNU GPLv3](/LICENSE.txt).
It is derived from Space Station 14's launcher which is licensed under the [MIT License](/LICENSE-SS14.txt).
A single file `ViewLocator.cs` is derived from an avalonia sample licensed under the [MIT License](/LICENSE-AVALONIA.txt).

The terms of each license must be followed when redistributing, etc.
