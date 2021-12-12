using System;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UniRx;
using VContainer.Unity;

namespace Main{
    public class GameStateObservable : IGameStateObservable, IStartable, IDisposable{
        private readonly Subject<GameState> onChangeState = new Subject<GameState>();
        public IObservable<GameState> OnChangeState => onChangeState;
        private Player me;
        private IQuestionReceiver QuestionReceiver;

        public GameStateObservable(IQuestionReceiver questionReceiver){
            QuestionReceiver = questionReceiver;
        }

        public void Start(){
            
        }


        public void Dispose(){
            onChangeState?.Dispose();
        }
    }
}
