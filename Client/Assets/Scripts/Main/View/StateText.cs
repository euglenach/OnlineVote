using System;
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
    
    private void Start(){
        text = GetComponent<Text>();
        
        gameStateObservable
            .OnChangeState
            .Subscribe(ChangeState)
            .AddTo(this);
    }

    void ChangeState(GameState state){
        var str = state switch{
            GameState.HostWait => isMaster ? "質問を入力してください" : "出題者を待っています",
            GameState.Vote => isMaster ? "投票を待っています。" : "投票してください",
            GameState.Result => "結果発表",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

        text.text = str;
    }
}
