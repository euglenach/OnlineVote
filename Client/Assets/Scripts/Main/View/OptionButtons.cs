using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using ServerShared.MessagePackObjects;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Main.View{
    public class OptionButtons : MonoBehaviour{
        [Inject] private QuestionRPC questionRPC;
        private OptionButton[] buttons;
        private IDisposable disposableOnClicks;
        private CancellationToken cancellationToken;

        private void Awake(){
            cancellationToken = this.GetCancellationTokenOnDestroy();
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
                       .ToUniTaskAsyncEnumerable()
                       .SubscribeAwait(async (i, ct) => {
                           await questionRPC.OptionSelectAsync(i, ct);
                       }).AddTo(cancellationToken);
        }
    }
}
