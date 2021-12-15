using System;
using Games;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Main.View{
    public class QuestionText : MonoBehaviour{
        private Text text;
        [Inject] private GameRPC gameRPC;

        private void Start(){
            text = GetComponent<Text>();
            
            gameRPC.QuestionStream
                   .Where(_ => !GameRPC.Player.IsMaster)
                   .Subscribe(q => {
                       text.text = q.Message;
                   })
                   .AddTo(this);
        }
    }
}
