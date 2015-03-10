using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class StartTurnCommand : Command
{
    [Inject]
    public DiscColour turnToPlay { get; set; }

    [Inject]
    public ContinueGameSignal continueSignal { get; set; }

    public override void Execute()
    {
        Debug.Log("Start Turn");
   		IGameManager gameManager = injectionBinder.GetInstance<IGameManager> () as IGameManager;

        if (gameManager.GetGameBoard().GetLegalMoves(turnToPlay).Count > 0)
        {
            gameManager.SkippedLastTurn = false;
            PlayMethod method = PlayMethod.UserInput;

            switch (turnToPlay)
            {
                case DiscColour.White: method = gameManager.WhitePlayer.GetPlayMethod(); break;
                case DiscColour.Black: method = gameManager.BlackPlayer.GetPlayMethod(); break;
            }

            GetNextMove(method);
        }
        else
        {
            SkipCurrentTurn(gameManager);
        }
    }

    private void GetNextMove(PlayMethod method)
    {
        switch (method)
        {
            case PlayMethod.UserInput: 
                BeginUserInputTurn(); 
                break;
            default: 
                BeginMinimaxSearchTurn(); 
                break;
        }
    }

    private void BeginUserInputTurn()
    {
        //Debug.Log("Begin User Input");
        MakeUserInputMoveSignal signal = injectionBinder.GetInstance<MakeUserInputMoveSignal>() as MakeUserInputMoveSignal;
        signal.Dispatch(turnToPlay);
    }

    private void BeginMinimaxSearchTurn()
    {
        Retain();
        continueSignal.AddListener(MakeAIMove);
    }

    private void MakeAIMove()
    {
        MakeAIMoveSignal signal = injectionBinder.GetInstance<MakeAIMoveSignal>() as MakeAIMoveSignal;
        signal.Dispatch(turnToPlay);
        continueSignal.RemoveListener(MakeAIMove);
        Release();
    }

    private void SkipCurrentTurn(IGameManager gameManager)
    {
        if (gameManager.SkippedLastTurn)
        {
            //End of Game
            Debug.Log("End");
            GameOverSignal overSignal = injectionBinder.GetInstance<GameOverSignal>() as GameOverSignal;
            overSignal.Dispatch();
        }
        else
        {
            gameManager.SkippedLastTurn = true;
            SkipTurnSignal skipSignal = injectionBinder.GetInstance<SkipTurnSignal>() as SkipTurnSignal;
            skipSignal.Dispatch();
        }
    }
}
