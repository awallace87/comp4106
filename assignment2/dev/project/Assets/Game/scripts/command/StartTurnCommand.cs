using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class StartTurnCommand : Command
{
    [Inject]
    public DiscColour turnToPlay { get; set; }

    public override void Execute()
    {
        Debug.Log("Start Turn");
   		IGameManager gameManager = injectionBinder.GetInstance<IGameManager> () as IGameManager;

        PlayMethod method = PlayMethod.UserInput;

        switch (turnToPlay)
        {
            case DiscColour.White: method = gameManager.WhitePlayer.GetPlayMethod(); break;
            case DiscColour.Black: method = gameManager.BlackPlayer.GetPlayMethod(); break;
        }

        GetNextMove(method);
    }

    private void GetNextMove(PlayMethod method)
    {
        switch (method)
        {
            case PlayMethod.UserInput: BeginUserInputTurn(); break;
            case PlayMethod.MinimaxSearch: BeginMinimaxSearchTurn(); break;
        }
    }

    private void BeginUserInputTurn()
    {
        Debug.Log("Begin User Input");
        MakeUserInputMoveSignal signal = injectionBinder.GetInstance<MakeUserInputMoveSignal>() as MakeUserInputMoveSignal;
        signal.Dispatch(turnToPlay);
    }

    private void BeginMinimaxSearchTurn()
    {
        MakeAIMoveSignal signal = injectionBinder.GetInstance<MakeAIMoveSignal>() as MakeAIMoveSignal;
        signal.Dispatch(turnToPlay);
    }
}
