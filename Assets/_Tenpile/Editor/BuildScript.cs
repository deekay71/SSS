using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Tenpile.Editor
{
    /// <summary>
    /// Build Android từ menu hoặc batchmode:
    /// Unity -batchmode -quit -buildTarget Android -projectPath . -executeMethod Tenpile.Editor.BuildScript.BuildAndroid
    /// </summary>
    public static class BuildScript
    {
        private const string ApkPath = "Builds/Android/Tenpile.apk";

        [MenuItem("Tenpile/Build/Android APK")]
        public static void BuildAndroid()
        {
            EditorUserBuildSettings.buildAppBundle = false;

            var options = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
                locationPathName = ApkPath,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.None,
            };

            var summary = BuildPipeline.BuildPlayer(options).summary;
            Debug.Log($"[Build] {summary.result}: {ApkPath}, {summary.totalSize} bytes, {summary.totalErrors} errors, {summary.totalTime}");

            if (Application.isBatchMode)
                EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
        }
    }
}
