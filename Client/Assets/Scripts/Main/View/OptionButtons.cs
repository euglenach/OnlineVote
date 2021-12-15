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

            gameRPC.QuestionStream.Subscribe(Init).AddTo(this);
        }

        public void Init(Question question){
            // if(GameRPC.Player.IsMaster){ return;}
            
            buttons = gameObject.GetComponentsInChildren<OptionButton>(true);
            disposableOnClicks?.Dispose();
            
            foreach(var (button,i) in buttons.Select((b,i) => (b , i))){
                if(question.Options.Length <= i){
                    button.gameObject.SetActive(false);
                    continue;
                }
                button.gameObject.SetActive(true);
                button.Init(question.Options[i],i);
                button.Interactable(true);
            }

            disposableOnClicks = 
                buttons.Select(b => b.OnClick)
                       .Merge()
                       .First()
                       .Subscribe(i=> {
                           foreach(var button in buttons){
                               button.Interactable(false);
                           }
                           gameRPC.OptionSelectAsync(i, cancellationToken).Forget();
                       }).AddTo(cancellationToken);
        }
    }
}
