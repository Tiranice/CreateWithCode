using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Lab.Editor
{
    public class BuildTool : UnityEditor.Editor
    {
        [MenuItem("Build/Build with Version")]
        public static void BuildWithVersion()
        {
            PlayerSettings.bundleVersion = GitUtilities.Git.BuildVersion;

            var scenes = new List<string>();
            foreach (var scene in EditorBuildSettings.scenes)
                scenes.Add(scene.path);

            var buildPlayerOptions = new BuildPlayerOptions()
            {
                scenes = scenes.ToArray(),
                locationPathName = $"Builds/Windows_x86_{PlayerSettings.bundleVersion.Replace(".", "_")}/{PlayerSettings.productName}.exe",
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None,
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
                Debug.Log($"Build succeeded:  {summary.totalSize} bytes");
            if (summary.result == BuildResult.Failed)
                Debug.LogError("Build failed");
        }
    }
}