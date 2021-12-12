using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IEntryNotification{
    UniTask EntryAsync(CancellationToken cancellationToken);
}
