# Tenpile

Game puzzle xếp chồng và gộp lá số. Thiết kế: https://claude.ai/artifact/UPr86rf4Rr9keb8HU5L5ki

- Unity **6000.0.76f1**, URP 2D (template 2D cross-platform), Android trước.
- Bấm Play từ scene `Assets/_Tenpile/Scenes/Boot.unity`. Mở thẳng `Main` sẽ thiếu scope cha.

## Cấu trúc (`Assets/_Tenpile`)

| Thư mục | Assembly | Ghi chú |
|---|---|---|
| `Core/` | `Tenpile.Core` | C# thuần, `noEngineReferences`. Luật chơi |
| `App/` | `Tenpile.App` | Use-case, service, port. Không chứa MonoBehaviour |
| `View/` | `Tenpile.View` | MonoBehaviour, UI, animation |
| `Boot/` | `Tenpile.Boot` | Composition root: `RootLifetimeScope` (VContainer), `BootFlow`, `MainLifetimeScope` |
| `Editor/` | `Tenpile.Editor` | Sinh scene/settings, script build |
| `Tests/EditMode`, `Tests/PlayMode` | `Tenpile.Tests.*` | NUnit |

`Infra/` (adapter SDK) và `Dev/` (debug menu, chỉ dev build) được thêm ở các mốc sau.

## Lệnh

Đặt `UNITY="D:/Unity/Hub/Editor/6000.0.76f1/Editor/Unity.exe"`, chạy tại thư mục project.

Sinh lại scene Boot/Main, build settings, player settings:

```bash
"$UNITY" -batchmode -quit -projectPath . -executeMethod Tenpile.Editor.ProjectSetup.Run -logFile Logs/setup.log
```

Chạy test:

```bash
"$UNITY" -batchmode -projectPath . -runTests -testPlatform EditMode -testResults Logs/editmode.xml -logFile Logs/editmode.log
```

```bash
"$UNITY" -batchmode -projectPath . -runTests -testPlatform PlayMode -testResults Logs/playmode.xml -logFile Logs/playmode.log
```

Build APK ra `Builds/Android/Tenpile.apk`:

```bash
"$UNITY" -batchmode -quit -buildTarget Android -projectPath . -executeMethod Tenpile.Editor.BuildScript.BuildAndroid -logFile Logs/build.log
```
