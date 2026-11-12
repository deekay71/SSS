using System.IO;
using Tenpile.Boot;
using Tenpile.Core;
using Tenpile.View;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tenpile.Editor
{
    /// <summary>
    /// Sinh scene Boot/Main, build settings và player settings Android.
    /// Chạy lại được: ghi đè hai scene của dự án, không đụng asset khác.
    /// Batchmode: -executeMethod Tenpile.Editor.ProjectSetup.Run
    /// </summary>
    public static class ProjectSetup
    {
        private const string SceneDir = "Assets/_Tenpile/Scenes";
        private static readonly Color PaperColor = new Color32(0xF3, 0xF5, 0xF8, 0xFF);

        [MenuItem("Tenpile/Setup/Rebuild Scenes And Settings")]
        public static void Run()
        {
            Directory.CreateDirectory(SceneDir);
            var boot = CreateBootScene();
            var main = CreateMainScene();

            EditorBuildSettings.scenes = new[]
            {
                new EditorBuildSettingsScene(boot, true),
                new EditorBuildSettingsScene(main, true),
            };

            ConfigurePlayer();
            AssetDatabase.SaveAssets();
            Debug.Log("[ProjectSetup] Done: " + boot + ", " + main);
        }

        private static string CreateBootScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCamera();
            new GameObject("RootLifetimeScope", typeof(RootLifetimeScope));
            return Save(scene, SceneNames.Boot);
        }

        private static string CreateMainScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCamera();

            var canvasGo = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvasGo.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGo.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080, 1920);
            scaler.matchWidthOrHeight = 0f;

            var boardGo = new GameObject("Board", typeof(RectTransform), typeof(GridLayoutGroup), typeof(BoardView));
            boardGo.transform.SetParent(canvasGo.transform, false);
            var rect = (RectTransform)boardGo.transform;
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(1000, 1100);

            var grid = boardGo.GetComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(220, 320);
            grid.spacing = new Vector2(24, 24);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = Board.Columns;
            grid.childAlignment = TextAnchor.MiddleCenter;

            new GameObject("MainLifetimeScope", typeof(MainLifetimeScope));
            return Save(scene, SceneNames.Main);
        }

        private static void CreateCamera()
        {
            var go = new GameObject("Main Camera", typeof(Camera)) { tag = "MainCamera" };
            var cam = go.GetComponent<Camera>();
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = PaperColor;
            go.transform.position = new Vector3(0, 0, -10);
        }

        private static string Save(Scene scene, string name)
        {
            var path = $"{SceneDir}/{name}.unity";
            EditorSceneManager.SaveScene(scene, path);
            return path;
        }

        private static void ConfigurePlayer()
        {
            PlayerSettings.productName = "Tenpile";
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, "com.studio.tenpile");
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        }
    }
}
