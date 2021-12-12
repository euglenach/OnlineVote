using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Games;
using Main;
using ServerShared.MessagePackObjects;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class QuestionSendButton : MonoBehaviour
{
    [SerializeField] private InputField question;
    [SerializeField] private InputField options;
    [Inject]private GameRPC gameRPC;
    private Button button;
    
    void Start(){
        button = GetComponent<Button>();

        button.OnClickAsAsyncEnumerable()
              .ForEachAwaitWithCancellationAsync(async (_,ct) => {
                  var q = question.text;
                  var ops = options.text.Split(',');
                  if(4 < ops.Length){ ops = ops.Take(4).ToArray();}
                  
                  await gameRPC.SendQuestionAsync(new Question{
                      Message = q,Options = ops
                  },ct);
              },this.GetCancellationTokenOnDestroy());
    }
}
