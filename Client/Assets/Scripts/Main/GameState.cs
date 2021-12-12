using System;

namespace Main{
    public enum GameState{
        HostWait,Vote,Result
    }

    public interface IGameStateObservable{
        IObservable<GameState> OnChangeState{get;}
    }
}
