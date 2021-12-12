using DefaultNamespace.Entry;
using ServerShared.StreamingHubs;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace Main{
    public class MainLifeTimeScope : LifetimeScope{
        protected override void Configure(IContainerBuilder builder){
            builder.RegisterEntryPoint<GameStateObservable>();
        }
    }
}
