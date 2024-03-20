using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildScript
{
    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        BuildAndroidClient();
        BuildLinuxServer();
        BuildWindowsClient();
        BuildWindowsServer();
    }

    [MenuItem("Build/Build Client (Android APK)")]
    public static void BuildAndroidClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
                "Assets/Scenes/WaveFunction.unity",
            };
        buildPlayerOptions.locationPathName = "Builds/Android/Client/Client.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    [MenuItem("Build/Build Server (Linux)")]
    public static void BuildLinuxServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Basic/SceneVR-Basic.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Common/SceneVR-Common.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Menu/SceneVR-Menu.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-UnityDemo/SceneVR-UnityDemo.unity"
            };
        buildPlayerOptions.locationPathName = "Builds/Linux/Server/Server.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.subtarget = (int)StandaloneBuildSubtarget.Server;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    [MenuItem("Build/Build Client (Windows)")]
    public static void BuildWindowsClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Basic/SceneVR-Basic.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Common/SceneVR-Common.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Menu/SceneVR-Menu.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-UnityDemo/SceneVR-UnityDemo.unity"
            };
        buildPlayerOptions.locationPathName = "Builds/Windows/Client/Client.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        // buildPlayerOptions.subtarget = (int)StandaloneBuildSubtarget.Client; //Potential subtarget required
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    [MenuItem("Build/Build Server (Windows)")]
    public static void BuildWindowsServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Basic/SceneVR-Basic.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Common/SceneVR-Common.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-Menu/SceneVR-Menu.unity",
                "Assets/MirrorExamplesVR/Scenes/SceneVR-UnityDemo/SceneVR-UnityDemo.unity"
            };
        buildPlayerOptions.locationPathName = "Builds/Windows/Server/Server.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.subtarget = (int)StandaloneBuildSubtarget.Server;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
