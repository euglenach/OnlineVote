using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Matching;
using ServerShared;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class RoomEntry : IAsyncStartable{
    [Inject] private EntryButton entryNotification;
    [Inject] private MatchingRPC matchingRPC;
    private IMatchingHub matchingHub;
    public Player Player{get;private set;}

    public async UniTask StartAsync(CancellationToken cancellation){
        var status = await entryNotification.EntryAsync(cancellation);
        Player = status.Player;

        try{
            await matchingRPC.JoinAsync(status.RoomName,Player, cancellation);
        } catch(AlreadyMasterExistException){
            SceneManager.LoadScene("Entry");
            return;
        } 
        SceneManager.LoadScene("Main");
    }
}
