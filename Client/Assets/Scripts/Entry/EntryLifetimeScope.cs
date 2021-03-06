using Games;
using VContainer;
using VContainer.Unity;

namespace DefaultNamespace.Entry{
    public class EntryLifetimeScope : LifetimeScope{
        // シングルトン
        private static EntryLifetimeScope entryLifetimeScope;
        protected override void Configure(IContainerBuilder builder){
            if(entryLifetimeScope is{}){
                Destroy(gameObject);
                return;
            }
            
            entryLifetimeScope = this;
            builder.Register<GameRPC>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<EntryButton>();
            builder.RegisterEntryPoint<RoomEntry>();
            DontDestroyOnLoad(gameObject);
        }
    }
}
