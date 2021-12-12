using System;
using System.Collections.Generic;

namespace Utils{
    public static class ChannelLocator{
        private static Dictionary<Type, IChannelLocatorManaged> dictionary =
            new Dictionary<Type, IChannelLocatorManaged>();

        public static IChannelLocatorManaged Get<T>(){
            return dictionary.TryGetValue(typeof(T), out var x)? x : default;
        }
    }

    public interface IChannelLocatorManaged : IDisposable{
        
    }
}