using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.View{
    public class RestartButton : MonoBehaviour{
        public void Restart() => UniTask.Void(async ct => {
            await SceneManager.LoadSceneAsync("SampleScene");
            // await SceneManager.UnloadSceneAsync("Main");
            // await UniTask.Yield();
            await SceneManager.LoadSceneAsync("Main");
        },this.GetCancellationTokenOnDestroy());
    }
}
