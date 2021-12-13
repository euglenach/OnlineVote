using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Games;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Main.View{
    public class SendResultButton : MonoBehaviour{
        [Inject] private GameRPC gameRPC;
        [Inject] private QuestionManager questionManager;
        private async UniTaskVoid Start(){
            await UniTask.WaitUntil(() => questionManager is{});
            
            GetComponent<Button>()
                .OnClickAsAsyncEnumerable()
                .ForEachAwaitWithCancellationAsync(async (_, ct) => {
                    await gameRPC.ResultAsync(questionManager.CreateResult().ToArray(), ct);
                },this.GetCancellationTokenOnDestroy()).Forget();
        }
    }
}
