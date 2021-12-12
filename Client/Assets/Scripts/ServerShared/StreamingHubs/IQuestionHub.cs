using System.Threading.Tasks;
using MagicOnion;
using ServerShared.MessagePackObjects;

namespace ServerShared.StreamingHubs{
    public interface IQuestionHub : IStreamingHub<IQuestionHub,IQuestionReceiver>{
        Task QuestionAsync(Question question);
        Task SelectAsync(int index);
        Task ResultAsync(QuestionResult[] questionResults);
    }

    public interface IQuestionReceiver{
        void OnQuestion(Question question);
        void OnResult(QuestionResult questionResult);
        void OnSelect(int index);
    }
}
