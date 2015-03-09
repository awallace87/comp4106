using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class BoardSquareInputCommand : Command
{
    [Inject]
    public GridPosition inputPosition { get; set; }

    [Inject]
    public IGameManager gameManager { get; set; }

    public override void Execute()
    {
        IBoardSquareModel boardSquare = gameManager.GetGameBoard().Board[inputPosition.X, inputPosition.Y];

        switch (boardSquare.State)
        {
            case BoardSquareState.Available: SelectSquare(); break;
            case BoardSquareState.Selected: PlayTurnAtSquare(); break;
        }
    }

    private void SelectSquare()
    {
        BoardSquareSelectedSignal selectSignal = injectionBinder.GetInstance<BoardSquareSelectedSignal>() as BoardSquareSelectedSignal;
        selectSignal.Dispatch(inputPosition);
    }

    private void PlayTurnAtSquare()
    {
        PlayTurnSignal playSignal = injectionBinder.GetInstance<PlayTurnSignal>() as PlayTurnSignal;
        playSignal.Dispatch(inputPosition, gameManager.CurrentTurn);
    }
}
