using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class UpdateBoardForTurnCommand : Command
{
    [Inject]
    public DiscColour currentPlayer { get; set; }

    public override void Execute()
    {
        IGameManager gameManager = injectionBinder.GetInstance<IGameManager>() as IGameManager;
        IList<GridPosition> legalMoves = gameManager.GetGameBoard().GetLegalMoves(currentPlayer);

        Debug.Log("Legal Moves for " + currentPlayer.ToString() + "- " + legalMoves.Count);

        foreach (GridPosition legalPosition in legalMoves)
        {
            gameManager.GetGameBoard().Board[legalPosition.X, legalPosition.Y].State = BoardSquareState.Available;
        }
    }
}
