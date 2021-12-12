using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Grpc.Core;
using MagicOnion.Client;
using ServerShared.MessagePackObjects;
using ServerShared.StreamingHubs;
using UniRx;
using Utils;
using Channel = Grpc.Core.Channel;

namespace Main{
    public class QuestionRPC : IQuestionReceiver, IDisposable{
        private Channel channel;
        private IQuestionHub questionHub;
       private readonly Subject<QuestionResult> resultStream = new Subject<QuestionResult>();
       public IObservable<QuestionResult> ResultStream => resultStream; 
       private readonly Subject<Question> questionStream = new Subject<Question>();
       public IObservable<Question> QuestionStream => questionStream;
       private readonly Subject<int> selectStream = new Subject<int>();
       public IObservable<int> SelectStream => selectStream;
        
        
        public QuestionRPC(){
            channel = new Channel(Common.URL, ChannelCredentials.Insecure);
            questionHub = StreamingHubClient.Connect<IQuestionHub, IQuestionReceiver>(channel, this);
        }

        public async UniTask SendQuestionAsync(Question question,CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await questionHub.QuestionAsync(question).AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask OptionSelectAsync(int index,CancellationToken cancellationToken){
            cancellationToken.ThrowIfCancellationRequested();
            await questionHub.SelectAsync(index).AsUniTask();
            cancellationToken.ThrowIfCancellationRequested();
        }

        public async UniTask ResultAsync(QuestionResult[] results){
            await questionHub.ResultAsync(results);
        }

        public void OnQuestion(Question question){
            questionStream.OnNext(question);
        }

        public void OnResult(QuestionResult questionResult){
            resultStream.OnNext(questionResult);
        }

        public void OnSelect(int index){
            selectStream.OnNext(index);
        }

        public void Dispose() => UniTask.Void(async () => {
            questionStream.Dispose();
            resultStream.Dispose();
            await questionHub.DisposeAsync();
            await channel.ShutdownAsync();
        });
    }
}
