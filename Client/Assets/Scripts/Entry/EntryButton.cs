using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Entry;
using ServerShared.MessagePackObjects;
using UnityEngine;
using UnityEngine.UI;

public class EntryButton : MonoBehaviour, IEntryNotification{
    [SerializeField] private InputField nameInput;
    [SerializeField] private InputField roomInput;
    [SerializeField] private Toggle readerToggle;
    
    private UniTaskCompletionSource<EntryStatus> entryTask = new UniTaskCompletionSource<EntryStatus>();
    private CancellationTokenRegistration cancelEvent;
    private Button button;
    private Text text;

    public UniTask<EntryStatus> EntryAsync(CancellationToken cancellationToken){
        entryTask = new UniTaskCompletionSource<EntryStatus>();
        cancelEvent.Dispose();
        cancelEvent = cancellationToken.Register(() => entryTask.TrySetCanceled());
        return entryTask.Task;
    }

    private void Awake(){
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
        var token = this.GetCancellationTokenOnDestroy();

        readerToggle.OnValueChangedAsAsyncEnumerable()
                    .ForEachAsync(isOn => {
                        text.text = isOn? "部屋を作る" : "部屋に入る";
                    }, token);

        button.OnClickAsAsyncEnumerable()
              .Where(_ => !string.IsNullOrEmpty(nameInput.text) && !string.IsNullOrEmpty(roomInput.text))
              .ForEachAsync(_ => {
                  var userName = nameInput.text;
                  var room = roomInput.text;
                  var isMaster = readerToggle.isOn;
                  var player = new Player{ Name = userName, IsMaster = isMaster, };
                  
                  entryTask?.TrySetResult(new EntryStatus(player,room));
              }, token);
    }

    private void OnDestroy(){
        entryTask.TrySetCanceled();
    }
}
