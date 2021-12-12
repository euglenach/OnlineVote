using System;
using UnityEngine;
using VContainer;

namespace Main.View{
    public class StateChangeView : MonoBehaviour{
        [Inject] private IGameStateObservable gameStateObservable;

        private void Start(){
            
        }
    }
}
