using System;
using System.Collections.Generic;
using System.Threading;
using Games;
using UniRx;
using VContainer;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using ServerShared.MessagePackObjects;
using VContainer.Unity;

namespace Main{
    public class QuestionManager{
        private GameRPC gameRPC;
        private readonly Dictionary<int,int> answers = new Dictionary<int, int>();
        private Question question;

        [Inject]
        public QuestionManager(GameRPC gameRPC){
            this.gameRPC = gameRPC;
            StartAsync(new CancellationToken()).Forget();
        }

        public async UniTask StartAsync(CancellationToken cancellation){
            if(!GameRPC.Player.IsMaster){ return;}
            
            question = await gameRPC.QuestionStream.ToUniTask(true,cancellation);

            gameRPC.SelectStream
                   .Subscribe(i => {
                       if(answers.TryGetValue(i, out _)){
                           answers[i] += 1;
                       } else{
                           answers.Add(i,1);
                       }
                   })
                   .AddTo(cancellation);
        }

        public IEnumerable<QuestionResult> CreateResult(){
            if(GameRPC.Player.IsMaster){ yield break;}
            
            var count = answers.Count;
            foreach(var answer in answers){
                yield return new QuestionResult{
                    Option = question.Options[answer.Key], Percentage = (float)answer.Value / (float)count
                };
            }
        }
    }
}
