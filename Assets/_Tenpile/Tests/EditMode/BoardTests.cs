using System;
using System.Linq;
using NUnit.Framework;
using Tenpile.App;
using Tenpile.Core;

namespace Tenpile.Tests
{
    public class BoardTests
    {
        [Test]
        public void StartingBoard_Has12TraysIn4Columns()
        {
            var board = Board.CreateStarting();

            Assert.AreEqual(12, board.TrayCount);
            Assert.AreEqual(0, board.TrayCount % Board.Columns);
        }

        [Test]
        public void StartingBoard_Has4Open1Spare7Locked()
        {
            var board = Board.CreateStarting();
            var states = Enumerable.Range(0, board.TrayCount).Select(board.GetState).ToList();

            Assert.AreEqual(4, states.Count(s => s == TrayState.Open));
            Assert.AreEqual(1, states.Count(s => s == TrayState.Spare));
            Assert.AreEqual(7, states.Count(s => s == TrayState.Locked));
        }

        [Test]
        public void Constructor_RejectsTrayCountNotMultipleOfColumns()
        {
            Assert.Throws<ArgumentException>(() => new Board(new TrayState[5]));
            Assert.Throws<ArgumentException>(() => new Board(Array.Empty<TrayState>()));
        }

        [Test]
        public void GameSession_StartNew_CreatesStartingBoard()
        {
            var session = new GameSession();
            session.StartNew();

            Assert.IsNotNull(session.Board);
            Assert.AreEqual(12, session.Board.TrayCount);
        }
    }

    public class ArchitectureTests
    {
        [Test]
        public void Core_DoesNotReferenceUnityEngine()
        {
            var unityRefs = typeof(Board).Assembly.GetReferencedAssemblies()
                .Select(a => a.Name)
                .Where(n => n.StartsWith("UnityEngine") || n.StartsWith("UnityEditor"))
                .ToList();

            Assert.IsEmpty(unityRefs, "Tenpile.Core phải là C# thuần: " + string.Join(", ", unityRefs));
        }
    }
}
