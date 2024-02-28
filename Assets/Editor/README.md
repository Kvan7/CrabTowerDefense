# Unity Build Automation

## Build Options

### Unity Editor

Select `Build` from the Unity Editor Menu and choose your build option. Make sure that `BuildScript.cs` is in the directory `Assets/Editor`.

### Python Script

In the root of the project folder run the following command:

```sh
python build.py --platform <android, linux, windows> --deployment_type <client, server>
```

To run the build script, use the following parameters:
- `-p, --platform`: Specify the target platform (`android`, `linux`, `windows`).
- `-d, --deployment_type`: Specify the deployment type (`client`, `server`).
- `-u, --unity_path`: Specify the complete path to the directory containing `Unity.exe`.

The default `Unity.exe` editor path is `C:\Program Files\Unity\Hub\Editor\2022.3.17f1\Editor`.

To build all supported targets, run `python build.py` without any parameters for target platform or deployment type.

## Required Modules

| Build Type     | Module Name                            |
|----------------|----------------------------------------|
| Android Client (Oculus Headset) | Android Build Support |
| Linux Server   | Linux Dedicated Server Build Support   |
| Windows Client | None (Default Install)                 |
| Windows Server | Windows Dedicated Server Build Support |

## Build Folder Structure

- Builds
  - Android
    - Client
    - Server
  - Linux
    - Client
    - Server
  - Windows
    - Client
    - Server

## Potential Error Messages

### Error building player because build target was unsupported:

- Add module to Unity install (ex. Android, Linux).

### Error building Player: Dedicated Server support for Win is not installed:

- Add Windows Server module to Unity Install (see above).


### Another Unity instance is running with this project open:

- Multiple Unity instances cannot open the same project.
- Either close the Unity window and rerun `build.py`, or run the build from the Unity Editor Menu.