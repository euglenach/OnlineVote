using System.Threading.Tasks;
using Grpc.Core;

namespace Utils{
    public class Common{
        private const string URL = "localhost:12345";
        private const int port = 12345;
        private static Channel channel;

        public static Channel GetChannel(){
            if(channel is null){
                return channel = new Channel(URL, ChannelCredentials.Insecure);
            }

            return channel;
        }

        public static Task ChannelShutdownAsync(){
            return channel?.ShutdownAsync();
        }
    }
}
