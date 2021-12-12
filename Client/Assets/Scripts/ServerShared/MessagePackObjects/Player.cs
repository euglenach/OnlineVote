using MessagePack;

namespace ServerShared.MessagePackObjects{
    
    [MessagePackObject]
    public class Player{
        [Key(0)]
        public string Name { get; set; }
        [Key(1)]
        public bool IsMaster{get;set;}
    }
}
