using System;
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
        [SerializeField] private Text resultText;

        private bool isMaster => GameRPC.Player.IsMaster;

        private void Start(){
            gameStateObservable
                .OnChangeState
                .Subscribe(StateChange)
                .AddTo(this);
        }

        void StateChange(GameState state){
            switch(state){
                case GameState.HostWait:
                    masterSection.gameObject.SetActive(isMaster);
                    break;
                case GameState.Vote:
                    respondentSection.gameObject.SetActive(!isMaster);
                    break;
                case GameState.Result:
                    respondentSection.SetActive(false);
                    masterSection.SetActive(false);
                    resultText.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
