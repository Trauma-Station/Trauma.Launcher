#!/usr/bin/env python3

import argparse
import os
import stat
import subprocess
import shutil
import glob

from download_net_runtime import update_netcore_runtime, PLATFORM_WINDOWS, PLATFORM_WINDOWS_ARM64, PLATFORM_LINUX, PLATFORM_LINUX_ARM64, PLATFORM_MACOS, PLATFORM_MACOS_ARM64
from exe_set_subsystem import set_subsystem

TFM = "net10.0"

p = os.path.join

def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("platform", nargs="*")
    parser.add_argument("--x64-only", action="store_true")

    args = parser.parse_args()
    platforms: list[str] = args.platform
    x64_only: bool = args.x64_only

    script_path = os.path.dirname(os.path.realpath(__file__))
    os.chdir(script_path)

    if "windows" in platforms:
        publish_windows(x64_only)
    if "linux" in platforms:
        publish_linux(x64_only)
    if "osx" in platforms:
        publish_osx()


def publish_windows(x64_only: bool):
    update_netcore_runtime([PLATFORM_WINDOWS])
    if not x64_only:
        update_netcore_runtime([PLATFORM_WINDOWS_ARM64])

    clear_prev_publish("Windows")

    dotnet_publish("Trauma.Launcher/Trauma.Launcher.csproj", "win-x64", False, "/p:FullRelease=True", "/p:RobustILLink=true")
    dotnet_publish("Trauma.Loader/Trauma.Loader.csproj", "win-x64", False, "/p:FullRelease=True", "/p:RobustILLink=true")
    if os.name == 'nt':
        dotnet_publish("Trauma.Launcher.Bootstrap/Trauma.Launcher.Bootstrap.csproj", "win-x64", True, "/p:FullRelease=True", "/p:RobustILLink=true")

    safe_set_subsystem(f"Trauma.Launcher/bin/Release/{TFM}/win-x64/publish/Trauma.Launcher.exe")
    safe_set_subsystem(f"Trauma.Loader/bin/Release/{TFM}/win-x64/publish/Trauma.Loader.exe")

    if not x64_only:
        dotnet_publish("Trauma.Launcher/Trauma.Launcher.csproj", "win-arm64", False, "/p:FullRelease=True", "/p:RobustILLink=true")
        dotnet_publish("Trauma.Loader/Trauma.Loader.csproj", "win-arm64", False, "/p:FullRelease=True", "/p:RobustILLink=true")
        safe_set_subsystem(f"Trauma.Launcher/bin/Release/{TFM}/win-arm64/publish/Trauma.Launcher.exe")
        safe_set_subsystem(f"Trauma.Loader/bin/Release/{TFM}/win-arm64/publish/Trauma.Loader.exe")

    os.makedirs("bin/publish/Windows/bin_x64/loader", exist_ok=True)
    os.makedirs("bin/publish/Windows/dotnet_x64", exist_ok=True)
    if not x64_only:
        os.makedirs("bin/publish/Windows/bin_arm64/loader", exist_ok=True)
        os.makedirs("bin/publish/Windows/dotnet_arm64", exist_ok=True)

    bootstrap_path = f"Trauma.Launcher.Bootstrap/bin/Release/{TFM}-windows/win-x64/publish/Trauma Station Launcher.exe"
    # Natively compiled copy we need to get from a separate worker.
    if os.path.isfile("Trauma Station Launcher.exe"):
        bootstrap_path = "Trauma Station Launcher.exe"
    shutil.copyfile(bootstrap_path, "bin/publish/Windows/Trauma Station Launcher.exe")
    shutil.copyfile("Trauma.Launcher.Bootstrap/console.bat", "bin/publish/Windows/console.bat")

    shutil.copytree("Dependencies/dotnet/windows", "bin/publish/Windows/dotnet_x64", dirs_exist_ok=True)
    shutil.copytree(f"Trauma.Launcher/bin/Release/{TFM}/win-x64/publish", "bin/publish/Windows/bin_x64", dirs_exist_ok=True)
    shutil.copytree(f"Trauma.Loader/bin/Release/{TFM}/win-x64/publish", "bin/publish/Windows/bin_x64/loader", dirs_exist_ok=True)

    if not x64_only:
        shutil.copytree("Dependencies/dotnet/windows-arm64", "bin/publish/Windows/dotnet_arm64", dirs_exist_ok=True)
        shutil.copytree(f"Trauma.Launcher/bin/Release/{TFM}/win-arm64/publish", "bin/publish/Windows/bin_arm64", dirs_exist_ok=True)
        shutil.copytree(f"Trauma.Loader/bin/Release/{TFM}/win-arm64/publish", "bin/publish/Windows/bin_arm64/loader", dirs_exist_ok=True)

    shutil.make_archive("Trauma.Launcher_Windows", "zip", "bin/publish/Windows")

def publish_linux(x64_only: bool):
    update_netcore_runtime([PLATFORM_LINUX])
    if not x64_only:
        update_netcore_runtime([PLATFORM_LINUX_ARM64])

    clear_prev_publish("Linux")

    os.makedirs("bin/publish/Linux", exist_ok=True)

    dotnet_publish("Trauma.Launcher/Trauma.Launcher.csproj", "linux-x64", False, "/p:FullRelease=True", "/p:RobustILLink=true")
    dotnet_publish("Trauma.Loader/Trauma.Loader.csproj", "linux-x64", False, "/p:FullRelease=True", "/p:RobustILLink=true")

    if not x64_only:
        dotnet_publish("Trauma.Launcher/Trauma.Launcher.csproj", "linux-arm64", False, "/p:FullRelease=True", "/p:RobustILLink=true")
        dotnet_publish("Trauma.Loader/Trauma.Loader.csproj", "linux-arm64", False, "/p:FullRelease=True", "/p:RobustILLink=true")

    os.makedirs("bin/publish/Linux/bin_x64/loader", exist_ok=True)
    os.makedirs("bin/publish/Linux/dotnet_x64", exist_ok=True)
    if not x64_only:
        os.makedirs("bin/publish/Linux/bin_arm64/loader", exist_ok=True)
        os.makedirs("bin/publish/Linux/dotnet_arm64", exist_ok=True)

    shutil.copytree("Dependencies/dotnet/linux", "bin/publish/Linux/dotnet_x64", dirs_exist_ok=True)
    shutil.copytree(f"Trauma.Launcher/bin/Release/{TFM}/linux-x64/publish", "bin/publish/Linux/bin_x64", dirs_exist_ok=True)
    shutil.copytree(f"Trauma.Loader/bin/Release/{TFM}/linux-x64/publish", "bin/publish/Linux/bin_x64/loader", dirs_exist_ok=True)

    if not x64_only:
        shutil.copytree("Dependencies/dotnet/linux-arm64", "bin/publish/Linux/dotnet_arm64", dirs_exist_ok=True)
        shutil.copytree(f"Trauma.Launcher/bin/Release/{TFM}/linux-arm64/publish", "bin/publish/Linux/bin_arm64", dirs_exist_ok=True)
        shutil.copytree(f"Trauma.Loader/bin/Release/{TFM}/linux-arm64/publish", "bin/publish/Linux/bin_arm64/loader", dirs_exist_ok=True)

    # Copy and make these files executable
    for executable in ["Trauma.Launcher", "Trauma Station.desktop"]:
        src = "PublishFiles/" + executable
        dest = "bin/publish/Linux/" + executable
        shutil.copyfile(src, dest)
        st = os.stat(dest)
        os.chmod(dest, st.st_mode | stat.S_IEXEC)

    shutil.make_archive("Trauma.Launcher_Linux", "zip", "bin/publish/Linux")


def publish_osx():
    update_netcore_runtime([PLATFORM_MACOS, PLATFORM_MACOS_ARM64])

    clear_prev_publish("macOS")

    os.makedirs("bin/publish/macOS", exist_ok=True)
    shutil.copytree("PublishFiles/Trauma Station Launcher.app", "bin/publish/macOS/Trauma Station Launcher.app")

    res_root = "bin/publish/macOS/Trauma Station Launcher.app/Contents/Resources"

    loader_res_root = f"{res_root}/Space Station 15.app/Contents/Resources"

    for arch in ["x64", "arm64"]:
        full_arch_name = "x86_64" if arch == "x64" else arch
        dotnet_publish("Trauma.Launcher/Trauma.Launcher.csproj", f"osx-{arch}", False, "/p:FullRelease=True", "/p:RobustILLink=true")
        dotnet_publish("Trauma.Loader/Trauma.Loader.csproj", f"osx-{arch}", False, "/p:FullRelease=True", "/p:RobustILLink=true")

        shutil.copytree(f"Trauma.Launcher/bin/Release/{TFM}/osx-{arch}/publish", f"{res_root}/{full_arch_name}/bin", dirs_exist_ok=True)
        shutil.copytree(f"Trauma.Loader/bin/Release/{TFM}/osx-{arch}/publish", f"{loader_res_root}/{full_arch_name}/bin", dirs_exist_ok=True)

    shutil.copytree("Dependencies/dotnet/mac", f"{res_root}/x86_64/dotnet")
    shutil.copytree("Dependencies/dotnet/mac-arm64", f"{res_root}/arm64/dotnet")

    shutil.make_archive("Trauma.Launcher_macOS", "zip", "bin/publish/macOS/")

def clear_prev_publish(publish_dir: str):
    shutil.rmtree(f"bin/publish/{publish_dir}", ignore_errors=True)

    for path in glob.glob("**/bin"):
        shutil.rmtree(path)


def run(*args: str):
    subprocess.run(args, shell=False, check=True)

def safe_set_subsystem(exe: str):
    if os.name != 'nt':
        set_subsystem(exe, 2)

def dotnet_publish(proj: str, rid: str, self_contained: bool, *args: str):
    run(
        "dotnet",
        "publish",
        proj,
        "--runtime",
        rid,
        "--self-contained" if self_contained else "--no-self-contained",
        "--configuration",
        "Release",
        "/nologo",
        *args)

if __name__ == "__main__":
    main()
