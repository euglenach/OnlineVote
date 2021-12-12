using System;
using System.Linq;
using System.Threading.Tasks;
using MagicOnion.Server.Hubs;
using ServerShared;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;

namespace Server.StreamingHubs{
    public class MatchingHub : StreamingHubBase<IGameHub,IGameReceiver>, IGameHub{
        IGroup room;
        Player me;
        private IInMemoryStorage<Player> players;

        public async Task JoinAsync(string roomName, Player player){
            (room,players) = await Group.AddAsync(roomName,player);
            me = player;

            if(players.AllValues.Any(p => p.IsMaster)){
                await room.RemoveAsync(Context);
                return;
                // throw new AlreadyMasterExistException();
            }

            // ブロードキャスト
            Broadcast(room).OnJoin(player);
        }

        public async Task LeaveAsync(){
            await room.RemoveAsync(Context);
            
            // ブロードキャスト
            Broadcast(room).OnLeave(me);
        }
        
        public Task QuestionAsync(Question question,Player player){
            
            Broadcast(room).OnQuestion(question);
            return Task.CompletedTask;
        }

        public Task SelectAsync(int index,Player player){
            Broadcast(room).OnSelect(index);
            return Task.CompletedTask;
        }

        public Task ResultAsync(QuestionResult[] questionResults,Player player){
            Broadcast(room).OnResult(questionResults);
            return Task.CompletedTask;
        }
        
        protected override ValueTask OnConnecting()
        {
            // handle connection if needed.
            Console.WriteLine($"client connected {this.Context.ContextId}");
            return CompletedTask;
        }

        protected override ValueTask OnDisconnected()
        {
            // handle disconnection if needed.
            // on disconnecting, if automatically removed this connection from group.
            Console.WriteLine($"client disconnected {this.Context.ContextId}");
            return CompletedTask;
        }
        
    }
}
