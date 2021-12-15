using System;
using System.Linq;
using System.Text;
using Games;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using VContainer;

namespace Main.View{
    public class ResultText : MonoBehaviour{
        [Inject] private GameRPC gameRPC;

        private void Awake(){
            var text = GetComponent<Text>();
            
            gameRPC.ResultStream
                   .Subscribe(result => {
                       var stringBuilder = new StringBuilder();

                       var total = result.Answers.Sum();
                       foreach(var ((count,op),i) in result.Answers
                                                      .Zip(result.Options,(count,option) => (count, option))
                                                      .OrderByDescending(x => x.count)
                                                      .Indexed()){
                           stringBuilder.Append($"{result.Options[i]}:{(float)count / (float)total * 100}% ...{count}人\n");
                       }

                       text.text = stringBuilder.ToString();
                   })
                   .AddTo(this);
        }
    }
}
