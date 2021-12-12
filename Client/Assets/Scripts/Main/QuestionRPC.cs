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
    public class QuestionRPC : IDisposable{
        private Channel channel;
        private IQuestionHub questionHub;
        private QuestionReceiver questionReceiver;

        public QuestionRPC(){
            channel = new Channel(Common.URL, ChannelCredentials.Insecure);
            questionReceiver = new QuestionReceiver();
            questionHub = StreamingHubClient.Connect<IQuestionHub, IQuestionReceiver>(channel, questionReceiver);
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


        public void Dispose() => UniTask.Void(async () => {
            questionReceiver.Dispose();
            await questionHub.DisposeAsync();
            await channel.ShutdownAsync();
        });

        class QuestionReceiver: IQuestionReceiver,IDisposable{
            private readonly Subject<QuestionResult> resultStream = new Subject<QuestionResult>();
            public IObservable<QuestionResult> ResultStream => resultStream;
            private readonly Subject<Question> questionStream = new Subject<Question>();
            public IObservable<Question> QuestionStream => questionStream;
            private readonly Subject<int> selectStream = new Subject<int>();
            public IObservable<int> SelectStream => selectStream;
            public void OnQuestion(Question question){
                questionStream.OnNext(question);
            }

            public void OnResult(QuestionResult questionResult){
                resultStream.OnNext(questionResult);
            }

            public void OnSelect(int index){
                selectStream.OnNext(index);
            }

            public void Dispose(){
                resultStream?.Dispose();
                questionStream?.Dispose();
                selectStream?.Dispose();
            }
        }
    }
}
