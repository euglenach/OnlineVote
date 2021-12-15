using System;
using Cysharp.Threading.Tasks;
using Games;
using Main;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StateText : MonoBehaviour{
    [Inject]private IGameStateObservable gameStateObservable;
    private Text text;
    private bool isMaster => GameRPC.Player.IsMaster;
    
    private async UniTaskVoid Start(){
        text = GetComponent<Text>();
        await UniTask.WaitUntil(() => gameStateObservable is{});
        
        gameStateObservable
            .OnChangeState
            .Subscribe(ChangeState)
            .AddTo(this);
    }

    void ChangeState(GameState state){
        var str = state switch{
            GameState.HostWait => isMaster ? "質問を入力してください" : "出題者を待っています",
            GameState.Vote => isMaster ? "投票を待っています。" : null,
            GameState.Result => "結果発表",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

        if(string.IsNullOrEmpty(str)){ return;}
        text.text = str;
    }
}
