using Tenpile.App;
using Tenpile.Core;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Tenpile.View
{
    /// <summary>Vẽ lưới khay từ GameSession. Art tạm: mỗi khay là một ô màu theo trạng thái.</summary>
    [RequireComponent(typeof(GridLayoutGroup))]
    public sealed class BoardView : MonoBehaviour
    {
        [SerializeField] private Color openColor = new Color32(0xFF, 0xFF, 0xFF, 0xFF);
        [SerializeField] private Color spareColor = new Color32(0xFB, 0xEF, 0xD9, 0xFF);
        [SerializeField] private Color lockedColor = new Color32(0xD9, 0xDF, 0xE8, 0xFF);
        [SerializeField] private Color labelColor = new Color32(0x5A, 0x64, 0x77, 0xFF);

        private GameSession _session;

        [Inject]
        public void Construct(GameSession session) => _session = session;

        private void Start()
        {
            var board = _session.Board;
            for (var i = 0; i < board.TrayCount; i++)
                CreateTray(i, board.GetState(i));
        }

        private void CreateTray(int index, TrayState state)
        {
            var tray = new GameObject($"Tray{index:00}", typeof(RectTransform), typeof(Image));
            tray.transform.SetParent(transform, false);
            tray.GetComponent<Image>().color = state switch
            {
                TrayState.Open => openColor,
                TrayState.Spare => spareColor,
                _ => lockedColor,
            };

            var label = new GameObject("Label", typeof(RectTransform), typeof(Text));
            label.transform.SetParent(tray.transform, false);
            var rect = (RectTransform)label.transform;
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = rect.offsetMax = Vector2.zero;

            var text = label.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 36;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = labelColor;
            text.text = state.ToString();
        }
    }
}
