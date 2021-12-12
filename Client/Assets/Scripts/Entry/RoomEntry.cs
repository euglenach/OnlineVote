using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using Grpc.Core;
using MagicOnion.Client;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Channel = Grpc.Core.Channel;

public class RoomEntry : IAsyncStartable, IMatchingHubReceiver{
    [Inject] private IEntryNotification entryNotification;
    private IMatchingHub matchingHub;

    public async UniTask StartAsync(CancellationToken cancellation){
        await entryNotification.EntryAsync(cancellation);

        var channel = new Channel("0.0.0.0:12345", ChannelCredentials.Insecure);
        var matching = new Matching(channel);
    }

    public void OnJoin(Player player){
        
    }

    public void OnLeave(Player player){
        
    }
}
