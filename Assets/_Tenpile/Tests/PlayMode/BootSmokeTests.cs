using System.Collections;
using NUnit.Framework;
using Tenpile.View;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Tenpile.Tests
{
    public class BootSmokeTests
    {
        [UnityTest]
        public IEnumerator Boot_LoadsMainSceneWithStartingBoard()
        {
            SceneManager.LoadScene("Boot");

            BoardView board = null;
            var deadline = Time.realtimeSinceStartup + 15f;
            while (Time.realtimeSinceStartup < deadline)
            {
                board = Object.FindAnyObjectByType<BoardView>();
                if (board != null && board.transform.childCount > 0) break;
                yield return null;
            }

            Assert.IsNotNull(board, "Không tìm thấy BoardView sau 15 s");
            Assert.AreEqual("Main", SceneManager.GetActiveScene().name);
            Assert.AreEqual(12, board.transform.childCount);
        }
    }
}
