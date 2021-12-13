using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicOnion.Client;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UniRx;
using Utils;
using Channel = Grpc.Core.Channel;

namespace Games{
    public class GameRPC : IDisposable{
        private Channel channel;
        private IGameHub gameHub;
        private GameReceiver gameReceiver;
        public IObservable<QuestionResult[]> ResultStream => gameReceiver.ResultStream;
        public IObservable<Question> QuestionStream => gameReceiver.QuestionStream;
        public IObservable<int> SelectStream => gameReceiver.SelectStream;
        public IObservable<Player> JoinStream => gameReceiver.JoinStream;
        public IObservable<Player> LeaveStream => gameReceiver.LeaveStream;
        public static Player Player{get;private set;}

        public GameRPC(){
            channel = Common.GetChannel();
            gameReceiver = new GameReceiver();
            gameHub = StreamingHubClient.Connect<IGameHub, IGameReceiver>(channel,gameReceiver);
        }

        public async UniTask JoinAsync(string roomName,Player player,CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await gameHub.JoinAsync(roomName,player);
            cancellationToken.ThrowIfCancellationRequested();
            Player = player;
        }
        
        public async UniTask LeaveAsync(CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await gameHub.LeaveAsync().AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }
        
        public async UniTask SendQuestionAsync(Question question,CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await gameHub.QuestionAsync(question,Player).AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask OptionSelectAsync(int index,CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await gameHub.SelectAsync(index,Player).AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask ResultAsync(QuestionResult[] results,CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await gameHub.ResultAsync(results,Player).AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async void Dispose(){
            gameReceiver.Dispose();
            await gameHub.DisposeAsync();
            await Common.ChannelShutdownAsync();
        }
        
        class GameReceiver : IGameReceiver, IDisposable{
            private readonly Subject<Player> onjoin = new Subject<Player>();
            public IObservable<Player> JoinStream => onjoin;
            private readonly Subject<Player> onLeave = new Subject<Player>();
            public IObservable<Player> LeaveStream => onLeave;
            private readonly Subject<QuestionResult[]> resultStream = new Subject<QuestionResult[]>();
            public IObservable<QuestionResult[]> ResultStream => resultStream;
            private readonly Subject<Question> questionStream = new Subject<Question>();
            public IObservable<Question> QuestionStream => questionStream;
            private readonly Subject<int> selectStream = new Subject<int>();
            public IObservable<int> SelectStream => selectStream;
            
            public void OnJoin(Player player){
                onjoin.OnNext(player);
            }

            public void OnLeave(Player player){
                onLeave.OnNext(player);
            }
            
            public void OnQuestion(Question question){
                questionStream.OnNext(question);
            }

            public void OnResult(QuestionResult[] questionResults){
                resultStream.OnNext(questionResults);
            }

            public void OnSelect(int index){
                selectStream.OnNext(index);
            }

            public void Dispose(){
                onjoin?.Dispose();
                onLeave?.Dispose();
                resultStream?.Dispose();
                questionStream?.Dispose();
                selectStream?.Dispose();
            }
        }
    }
}