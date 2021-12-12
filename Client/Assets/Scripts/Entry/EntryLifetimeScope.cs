using VContainer;
using VContainer.Unity;

namespace DefaultNamespace.Entry{
    public class EntryLifetimeScope : LifetimeScope{
        protected override void Configure(IContainerBuilder builder){
            builder.RegisterEntryPoint<RoomEntry>();
            builder.RegisterComponentInHierarchy<IEntryNotification>();
        }
    }
}
