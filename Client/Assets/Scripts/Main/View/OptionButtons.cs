using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Games;
using ServerShared.MessagePackObjects;
using UniRx;
using UnityEngine;
using VContainer;

namespace Main.View{
    public class OptionButtons : MonoBehaviour{
        [Inject] private GameRPC gameRPC;
        private OptionButton[] buttons;
        private IDisposable disposableOnClicks;
        private CancellationToken cancellationToken;

        private void Awake(){
            cancellationToken = this.GetCancellationTokenOnDestroy();

            gameRPC.QuestionStream.Subscribe(Init);
        }

        public void Init(Question question){
            buttons = GetComponentsInChildren<OptionButton>();
            disposableOnClicks?.Dispose();
            
            foreach(var button in buttons){
                button.gameObject.SetActive(false);
            }
            
            foreach(var i in Enumerable.Range(0,question.Options.Length)){
                var button = buttons.FirstOrDefault(b => !b.gameObject.activeInHierarchy);
                button?.gameObject.SetActive(true);
                button.Init(question.Options[i],i);
            }

            disposableOnClicks = 
                buttons.Select(b => b.OnClick)
                       .Merge()
                       .First()
                       .ToUniTaskAsyncEnumerable()
                       .SubscribeAwait(async (i, ct) => {
                           await gameRPC.OptionSelectAsync(i, ct);
                       }).AddTo(cancellationToken);
        }
    }
}
