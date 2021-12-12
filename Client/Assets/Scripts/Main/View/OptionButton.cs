using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View{
    public class OptionButton : MonoBehaviour{
        private int optionIndex;
        private readonly Subject<int> onClick = new Subject<int>();
        public IObservable<int> OnClick => onClick;

        public void Init(string question,int index){
            var text = GetComponentInChildren<Text>();
            text.text = question;
            optionIndex = index;
        }

        private void Start(){
            GetComponent<Button>()
                .OnClickAsAsyncEnumerable()
                .ForEachAsync(_ => {
                    onClick.OnNext(optionIndex);
                },this.GetCancellationTokenOnDestroy());
        }
    }
}
