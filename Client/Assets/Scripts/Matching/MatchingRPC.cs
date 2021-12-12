using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UniRx;
using Utils;
using Channel = Grpc.Core.Channel;

namespace Matching{
    public class MatchingRPC : IDisposable{
        private Channel channel;
        private IMatchingHub matchingHub;
        private MatchingHubReceiver matchingHubReceiver;
        public IObservable<Player> JoinStream => matchingHubReceiver.JoinStream;
        public IObservable<Player> LeaveStream => matchingHubReceiver.LeaveStream;
        
        public MatchingRPC(){
            channel = new Channel(Common.URL, ChannelCredentials.Insecure);
            this.matchingHubReceiver = new MatchingHubReceiver();
            this.matchingHub = StreamingHubClient.Connect<IMatchingHub, IMatchingHubReceiver>(channel,matchingHubReceiver);
        }

        public void Dispose() => UniTask.Void(async () => {
            await matchingHub.LeaveAsync();
            await matchingHub.DisposeAsync();
            await channel.ShutdownAsync();
        });

        public async UniTask JoinAsync(string roomName,Player player,CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await matchingHub.JoinAsync(roomName,player).AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }
        
        public async UniTask LeaveAsync(CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await matchingHub.LeaveAsync().AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }

        class MatchingHubReceiver : IMatchingHubReceiver, IDisposable{
            private readonly Subject<Player> onjoin = new Subject<Player>();
            public IObservable<Player> JoinStream => onjoin;

            private readonly Subject<Player> onLeave = new Subject<Player>();
            public IObservable<Player> LeaveStream => onLeave;
            public void OnJoin(Player player){
                onjoin.OnNext(player);
            }

            public void OnLeave(Player player){
                onLeave.OnNext(player);
            }

            public void Dispose(){
                onjoin?.Dispose();
                onLeave?.Dispose();
            }
        }
    }
}
