using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Games;
using ServerShared.MessagePackObjects;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class RoomEntry : IAsyncStartable{
    [Inject] private EntryButton entryNotification;
    [Inject] private GameRPC gameRPC;

    public async UniTask StartAsync(CancellationToken cancellation){
        var status = await entryNotification.EntryAsync(cancellation);

        try{
            await gameRPC.JoinAsync(status.RoomName,status.Player, cancellation);
        } catch(Exception e){
            SceneManager.LoadScene("Entry");
            return;
        } 
        SceneManager.LoadScene("Main");
    }
}
