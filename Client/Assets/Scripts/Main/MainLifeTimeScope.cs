using ServerShared.StreamingHubs;
using VContainer;
using VContainer.Unity;

namespace Main{
    public class MainLifeTimeScope : LifetimeScope{
        protected override void Configure(IContainerBuilder builder){
            builder.Register<QuestionRPC>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameStateObservable>();
        }
    }
}
