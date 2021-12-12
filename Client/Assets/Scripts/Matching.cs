using System;
using Cysharp.Threading.Tasks;
using MagicOnion.Client;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using Utils;
using Channel = Grpc.Core.Channel;

namespace DefaultNamespace{
    public class Matching : IMatchingHubReceiver , IChannelLocatorManaged{
        private Channel channel;
        private IMatchingHub matchingHub;

        public Matching(Channel channel){
            this.channel = channel;
            this.matchingHub = StreamingHubClient.Connect<IMatchingHub, IMatchingHubReceiver>(channel,this);
        }

        public void Dispose() => UniTask.Void(async () => {
            await matchingHub.DisposeAsync();
        });

        public void OnJoin(Player player){
            
        }

        public void OnLeave(Player player){
            
        }
    }
}
