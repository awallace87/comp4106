using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class GameOverCommand : Command
{
    public override void Execute()
    {
        IGameManager gameManager = injectionBinder.GetInstance<IGameManager>() as IGameManager;

        DiscColour colour = GetWinner(gameManager.GetGameBoard());

        if (colour == DiscColour.White)
        {
            Debug.Log("White Won");
        }
        else
        {
            Debug.Log("Black Won");
        }
    }

    private DiscColour GetWinner(IBoardModel board)
    {
        DiscColour winner;
        uint blackDiscs = 0;
        for (uint i = 0; i < board.BoardSize; i++)
        {
            for (uint j = 0; j < board.BoardSize; j++)
            {
                IBoardSquareModel square = board.Board[i, j];
                if (!square.ContainsDisc() && square.Disc.Colour == DiscColour.Black)
                {
                    blackDiscs++;
                }
            }
        }

        bool didBlackWin = blackDiscs > ((board.BoardSize * board.BoardSize) / 2.0);

        if (didBlackWin)
        {
            return DiscColour.Black;
        }
        else
        {
            return DiscColour.White;
        }
    }
}
