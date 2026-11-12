using Tenpile.Core;

namespace Tenpile.App
{
    /// <summary>Phiên chơi hiện tại. Use-case Pick/Drop/Deal được thêm ở M1–M2.</summary>
    public sealed class GameSession
    {
        public Board Board { get; private set; }

        public void StartNew() => Board = Board.CreateStarting();
    }
}
