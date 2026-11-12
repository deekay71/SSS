using System.Threading;
using Cysharp.Threading.Tasks;
using Tenpile.App;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace Tenpile.Boot
{
    /// <summary>
    /// Luồng khởi động. M0: tạo bàn mới rồi vào Main.
    /// Các bước consent, SDK, Remote Config, save, giờ server được thêm ở M4–M6.
    /// </summary>
    public sealed class BootFlow : IAsyncStartable
    {
        private readonly GameSession _session;
        private readonly LifetimeScope _rootScope;

        public BootFlow(GameSession session, LifetimeScope rootScope)
        {
            _session = session;
            _rootScope = rootScope;
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            _session.StartNew();

            using (LifetimeScope.EnqueueParent(_rootScope))
            {
                await SceneManager.LoadSceneAsync(SceneNames.Main).ToUniTask(cancellationToken: cancellation);
            }
        }
    }
}
