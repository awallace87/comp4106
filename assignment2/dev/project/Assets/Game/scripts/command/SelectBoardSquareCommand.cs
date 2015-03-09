using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;
using System;

public class SelectBoardSquareCommand : Command
{
    [Inject]
    public GridPosition selectPosition { get; set; }

    [Inject]
    public IGameManager gameManager { get; set; }

    public override void Execute()
    {
        IBoardSquareModel selectedSquare = gameManager.GetGameBoard().Board[selectPosition.X, selectPosition.Y];
        selectedSquare.State = BoardSquareState.Selected;
        UpdateAffectedBoardSquares();
    }

    private void UpdateAffectedBoardSquares()
    {
        IList<GridPosition> affectedPositions = gameManager.GetGameBoard().GetAffectedDiscPositions(gameManager.CurrentTurn, selectPosition);
        Debug.Log("Number of Affected Discs - " + affectedPositions.Count);

        foreach (GridPosition position in affectedPositions)
        {
            gameManager.GetGameBoard().Board[position.X, position.Y].State = BoardSquareState.Affected;
        }
    }
}
