using System;
using System.Collections.Generic;

namespace Tenpile.Core
{
    public enum TrayState
    {
        Open,
        Spare,
        Locked,
        Sealed,
        Padlocked,
    }

    /// <summary>Bàn chơi: danh sách khay và trạng thái. Lá bài được thêm ở M1.</summary>
    public sealed class Board
    {
        public const int Columns = 4;
        public const int TrayCapacity = 10;

        private readonly TrayState[] _trays;

        public Board(IReadOnlyList<TrayState> trays)
        {
            if (trays == null || trays.Count == 0 || trays.Count % Columns != 0)
                throw new ArgumentException($"Số khay phải là bội số dương của {Columns}.", nameof(trays));

            _trays = new TrayState[trays.Count];
            for (var i = 0; i < trays.Count; i++) _trays[i] = trays[i];
        }

        public int TrayCount => _trays.Length;

        public TrayState GetState(int tray) => _trays[tray];

        /// <summary>Bàn đầu ván: lưới 4×3, 4 khay mở, 1 khay dự phòng, 7 khay khoá.</summary>
        public static Board CreateStarting()
        {
            var trays = new TrayState[12];
            for (var i = 0; i < trays.Length; i++)
                trays[i] = i < 4 ? TrayState.Open : i == 4 ? TrayState.Spare : TrayState.Locked;
            return new Board(trays);
        }
    }
}
