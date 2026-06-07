# Trauma Station Launcher

This is the launcher you should be using to connect to Trauma Station.

# Development

Useful environment variables for development:
* `SS14_LAUNCHER_APPDATA_NAME=launcherTest` to change the user data directories the launcher stores its data in. This can be useful to avoid breaking your "normal" TS14 launcher data while developing something.
* `SS14_LAUNCHER_OVERRIDE_AUTH=https://.../` to change the auth API URL to test against a local dev version of the API.

# License

Trauma Station's Launcher is licensed under the [GNU GPLv3](/LICENSE.txt).
It is derived from Space Station 14's launcher which is licensed under the [MIT License](/LICENSE-SS14.txt).
A single file `ViewLocator.cs` is derived from an avalonia sample licensed under the [MIT License](/LICENSE-AVALONIA.txt).

The terms of each license must be followed when redistributing, etc.
