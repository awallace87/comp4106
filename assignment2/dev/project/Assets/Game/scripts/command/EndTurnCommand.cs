using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class EndTurnCommand : Command
{
    [Inject]
    public IGameManager gameManager { get; set; }

    public override void Execute()
    {
        ResetBoard();
        AlternatePlayer();
    }

    private void ResetBoard()
    {
        for (int i = 0; i < gameManager.GetGameBoard().BoardSize; i++)
        {
            for (int j = 0; j < gameManager.GetGameBoard().BoardSize; j++)
            {
                gameManager.GetGameBoard().Board[i, j].State = BoardSquareState.Default;
            }
        }
    }

    private void AlternatePlayer()
    {
        DiscColour currentTurn = gameManager.CurrentTurn;

        if (currentTurn == DiscColour.Black)
        {
            gameManager.CurrentTurn = DiscColour.White;
        }
        else
        {
            gameManager.CurrentTurn = DiscColour.Black;
        }
    }
}
