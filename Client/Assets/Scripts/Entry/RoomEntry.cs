using System.Threading;
using Cysharp.Threading.Tasks;
using Entry;
using Grpc.Core;
using Matching;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UnityEngine.SceneManagement;
using Utils;
using VContainer;
using VContainer.Unity;
using Channel = Grpc.Core.Channel;

public class RoomEntry : IAsyncStartable{
    [Inject] private EntryButton entryNotification;
    private IMatchingHub matchingHub;
    public Player Player{get;private set;}

    public async UniTask StartAsync(CancellationToken cancellation){
        var status = await entryNotification.EntryAsync(cancellation);
        Player = status.Player;

        var channel = new Channel(Common.URL, ChannelCredentials.Insecure);
        var matching = new MatchingRPC(channel);
        await matching.JoinAsync(status.RoomName,Player, cancellation);
        SceneManager.LoadScene("Main");
    }
}
