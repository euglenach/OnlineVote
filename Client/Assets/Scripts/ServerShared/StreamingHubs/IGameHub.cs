using System.Threading.Tasks;
using MagicOnion;
using ServerShared.MessagePackObjects;

namespace ServerShared.StreamingHubs{
    public interface IGameHub : IStreamingHub<IGameHub,IGameReceiver>{
        /// <summary>
        /// ゲームに接続することをサーバに伝える
        /// </summary>
        Task JoinAsync(string roomName,Player player);
        /// <summary>
        /// ゲームから切断することをサーバに伝える
        /// </summary>
        Task LeaveAsync();
        
        Task QuestionAsync(Question question,Player player);
        Task SelectAsync(int index,Player player);
        Task ResultAsync(QuestionResult questionResults,Player player);
    }

    public interface IGameReceiver{
        /// <summary>
        /// 誰かがゲームに接続したことをクライアントに伝える
        /// </summary>
        void OnJoin(Player player);
        
        /// <summary>
        /// 誰かがゲームから切断したことをクライアントに伝える
        /// </summary>
        void OnLeave(Player player);
        void OnQuestion(Question question);
        void OnResult(QuestionResult questionResults);
        void OnSelect(int index);
    }
}
