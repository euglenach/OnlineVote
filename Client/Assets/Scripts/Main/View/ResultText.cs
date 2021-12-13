using System;
using System.Text;
using Games;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Main.View{
    public class ResultText : MonoBehaviour{
        [Inject] private GameRPC gameRPC;

        private void Awake(){
            var text = GetComponent<Text>();
            
            gameRPC.ResultStream
                   .Subscribe(results => {
                       var stringBuilder = new StringBuilder();

                       foreach(var result in results){
                           stringBuilder.Append($"{result.Option}:{result.Percentage*100}%\n");
                       }

                       text.text = stringBuilder.ToString();
                   })
                   .AddTo(this);
        }
    }
}
