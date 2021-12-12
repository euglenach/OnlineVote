using MessagePack;

namespace ServerShared.MessagePackObjects{
    [MessagePackObject]
    public class Question{
        /// <summary>
        /// 質問文
        /// </summary>
        public string Message{get;set;}
        /// <summary>
        /// 選択肢
        /// </summary>
        public string[] Options{get;set;}
    }
    
    [MessagePackObject]
    public class QuestionResult{
        /// <summary>
        /// 選択肢の一個一個
        /// </summary>
        public string Option{get;set;}
        
        /// <summary>
        /// 割合
        /// </summary>
        public float Percentage{get;set;}
    }
}
