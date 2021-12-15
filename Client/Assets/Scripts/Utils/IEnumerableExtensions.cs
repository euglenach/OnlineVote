using System.Collections.Generic;
using System.Linq;

namespace Utils{
    public static class IEnumerableExtensions{
        public static IEnumerable<(T,int)> Indexed<T>(this IEnumerable<T> self){
            return self.Select((t, i) => (t, i));
        }
        
    }
}
