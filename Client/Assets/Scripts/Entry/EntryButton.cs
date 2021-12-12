using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EntryButton : MonoBehaviour, IEntryNotification{
    private UniTaskCompletionSource entryTask = new UniTaskCompletionSource();
    private CancellationTokenRegistration cancelEvent;
    private Button button;
    
    public UniTask EntryAsync(CancellationToken cancellationToken){
        entryTask = new UniTaskCompletionSource();
        cancelEvent.Dispose();
        cancelEvent = cancellationToken.Register(() => entryTask.TrySetCanceled());
        return entryTask.Task;
    }

    private void Awake(){
        button = GetComponent<Button>();

        button.OnClickAsAsyncEnumerable()
              .ForEachAsync(_ => {
                  entryTask?.TrySetResult();
              }, this.GetCancellationTokenOnDestroy());
    }

    private void OnDestroy(){
        entryTask.TrySetCanceled();
    }
}
