using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Games;
using UniRx;
using VContainer;
using JetBrains.Annotations;
using ServerShared.MessagePackObjects;
using VContainer.Unity;

namespace Main{
    public class QuestionManager{
        private GameRPC gameRPC;
        private int[] answers;
        private Question question;

        [Inject]
        public QuestionManager(GameRPC gameRPC){
            this.gameRPC = gameRPC;
            StartAsync(new CancellationToken()).Forget();
        }

        public async UniTask StartAsync(CancellationToken cancellation){
            if(!GameRPC.Player.IsMaster){ return;}
            
            question = await gameRPC.QuestionStream.ToUniTask(true,cancellation);

            answers = new int[question.Options.Length];

            gameRPC.SelectStream
                   .Synchronize()
                   .Subscribe(i => {
                       answers[i] += 1;
                   })
                   .AddTo(cancellation);
        }

        public QuestionResult CreateResult(){
            if(!GameRPC.Player.IsMaster){ return null;}
            
            return new QuestionResult{
                Options = question.Options,
                Answers = answers
            };
        }
    }
}
