using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.View{
    public class RestartButton : MonoBehaviour{
        public void Restart() => SceneManager.LoadSceneAsync("Main");
    }
}
