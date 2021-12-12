using System.Threading;
using Cysharp.Threading.Tasks;
using ServerShared.MessagePackObjects;

namespace Entry{
    public interface IEntryNotification{
        UniTask<EntryStatus> EntryAsync(CancellationToken cancellationToken);
    }

    public readonly struct EntryStatus{
        public readonly Player Player;
        public readonly string RoomName;
        
        public EntryStatus(Player player, string roomName){
            Player = player;
            RoomName = roomName;
        }
    }
}