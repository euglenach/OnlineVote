using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;

namespace Server.StreamingHubs{
    public class MatchingHub : StreamingHubBase<IMatchingHub,IMatchingHubReceiver>, IMatchingHub{
        IGroup room;
        Player me;


        public async Task JoinAsync(string roomName, Player player){
            room = await Group.AddAsync(roomName);
            me = player;
            
            // ブロードキャスト
            Broadcast(room).OnJoin(player);
        }

        public async Task LeaveAsync(){
            await room.RemoveAsync(Context);
            
            // ブロードキャスト
            Broadcast(room).OnLeave(me);
        }
    }
}
