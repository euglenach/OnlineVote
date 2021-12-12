using MessagePack;

namespace ServerShared.MessagePackObjects{
    [MessagePackObject]
    public class Question{
        /// <summary>
        /// 質問文
        /// </summary>
        [Key(0)]
        public string Message{get;set;}
        /// <summary>
        /// 選択肢
        /// </summary>
        [Key(1)]
        public string[] Options{get;set;}
    }
    
    [MessagePackObject]
    public class QuestionResult{
        /// <summary>
        /// 選択肢の一個一個
        /// </summary>
        [Key(0)]
        public string Option{get;set;}
        
        /// <summary>
        /// 割合
        /// </summary>
        [Key(1)]
        public float Percentage{get;set;}
    }
}
