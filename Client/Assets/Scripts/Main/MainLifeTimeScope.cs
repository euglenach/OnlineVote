using Main.View;
using VContainer;
using VContainer.Unity;

namespace Main{
    public class MainLifeTimeScope : LifetimeScope{
        protected override void Configure(IContainerBuilder builder){
            builder.Register<QuestionManager>(Lifetime.Singleton);
            builder.RegisterEntryPoint<GameStateObservable>();
        }
    }
}
