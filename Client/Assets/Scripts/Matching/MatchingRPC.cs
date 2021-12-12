using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MagicOnion.Client;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UniRx;
using Utils;
using Channel = Grpc.Core.Channel;

namespace Matching{
    public class MatchingRPC : IMatchingHubReceiver , IDisposable{
        private Channel channel;
        private IMatchingHub matchingHub;
        private readonly Subject<Player> onjoin = new Subject<Player>();
        public IObservable<Player> JoinStream => onjoin;

        private readonly Subject<Player> onLeave = new Subject<Player>();
        public IObservable<Player> LeaveStream => onLeave;
        
        public MatchingRPC(Channel channel){
            this.channel = channel;
            this.matchingHub = StreamingHubClient.Connect<IMatchingHub, IMatchingHubReceiver>(channel,this);
        }

        public void Dispose() => UniTask.Void(async () => {
            onjoin.Dispose();
            onLeave.Dispose();
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

        public void OnJoin(Player player){
            onjoin.OnNext(player);
        }

        public void OnLeave(Player player){
            onLeave.OnNext(player);
        }
    }
}
