using System.Threading.Tasks;
using MagicOnion;
using ServerShared.MessagePackObjects;

namespace ServerShared.StreamingHubs{
    public interface IMatchingHub : IStreamingHub<IMatchingHub,IMatchingHubReceiver>{
        /// <summary>
        /// ゲームに接続することをサーバに伝える
        /// </summary>
        Task JoinAsync(string roomName,Player player);
        /// <summary>
        /// ゲームから切断することをサーバに伝える
        /// </summary>
        Task LeaveAsync();
    }

    public interface IMatchingHubReceiver{
        /// <summary>
        /// 誰かがゲームに接続したことをクライアントに伝える
        /// </summary>
        void OnJoin(Player player);
        
        /// <summary>
        /// 誰かがゲームから切断したことをクライアントに伝える
        /// </summary>
        void OnLeave(Player player);
    }
}
