using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Games;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace Main{
    public class GameStateObservable : IGameStateObservable, IStartable, IDisposable{
        private readonly Subject<GameState> onChangeState = new Subject<GameState>();
        public IObservable<GameState> OnChangeState => onChangeState;
        private GameRPC gameRPC;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        [Inject] 
        public GameStateObservable(GameRPC gameRPC){
            this.gameRPC = gameRPC;
        }

        public void Start(){
            cancellationTokenSource = new CancellationTokenSource();
            
            gameRPC
                .QuestionStream
                .Subscribe(_ => {
                    onChangeState.OnNext(GameState.Vote);
                }).AddTo(cancellationTokenSource.Token);
            
            gameRPC
                .ResultStream
                .Subscribe(_ => {
                    onChangeState.OnNext(GameState.Result);
                }).AddTo(cancellationTokenSource.Token);
        }


        public void Dispose(){
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            onChangeState?.Dispose();
        }
    }
}
