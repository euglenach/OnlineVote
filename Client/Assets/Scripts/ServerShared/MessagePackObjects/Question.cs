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
        [Key(0)]
        public string[] Options{get;set;}
        
        /// <summary>
        /// 回答(Optionsとインデックスで紐づけ！)
        /// </summary>
        [Key(1)]
        public int[] Answers{get;set;}
    }
}
