using System;
using Cysharp.Threading.Tasks;
using Games;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Main.View{
    public class StateChangeView : MonoBehaviour{
        [Inject] private IGameStateObservable gameStateObservable;
        [SerializeField] private GameObject masterSection;
        [SerializeField] private GameObject respondentSection;
        [SerializeField] private GameObject sendResultSection;
        [SerializeField] private GameObject resultSection;

        private bool isMaster => GameRPC.Player.IsMaster;

        private async UniTaskVoid Start(){
            // 最初は待ちフェーズ
            StateChange(GameState.HostWait);
            
            await UniTask.WaitUntil(() => gameStateObservable is{});
            
            gameStateObservable
                .OnChangeState
                .Subscribe(StateChange)
                .AddTo(this);
        }

        void StateChange(GameState state){
            switch(state){
                case GameState.HostWait:
                    masterSection.gameObject.SetActive(isMaster);
                    respondentSection.gameObject.SetActive(false);
                    sendResultSection.gameObject.SetActive(false);
                    resultSection.gameObject.SetActive(false);
                    break;
                case GameState.Vote:
                    masterSection.gameObject.SetActive(false);
                    respondentSection.gameObject.SetActive(!isMaster);
                    sendResultSection.gameObject.SetActive(isMaster);
                    resultSection.gameObject.SetActive(false);
                    break;
                case GameState.Result:
                    respondentSection.SetActive(false);
                    masterSection.SetActive(false);
                    sendResultSection.gameObject.SetActive(false);
                    resultSection.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
