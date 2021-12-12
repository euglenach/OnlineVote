using System.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using ServerShared;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;

namespace Server.StreamingHubs{
    public class MatchingHub : StreamingHubBase<IMatchingHub,IMatchingHubReceiver>, IMatchingHub{
        IGroup room;
        Player me;
        private IInMemoryStorage<Player> players;

        public async Task JoinAsync(string roomName, Player player){
            (room,players) = await Group.AddAsync(roomName,player);
            me = player;

            if(players.AllValues.Any(p => p.IsMaster)){
                await room.RemoveAsync(Context);
                throw new AlreadyMasterExistException();
            }

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
