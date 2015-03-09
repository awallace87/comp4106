using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class InitializeGameBoardCommand : Command {

    [Inject]
    public IGameManager gameManager { get; set; }

    public override void Execute ()
	{
		gameManager.GetGameBoard ().Board = new IBoardSquareModel[OthelloConstants.StandardBoardSize, OthelloConstants.StandardBoardSize];

		for (uint i = 0; i < OthelloConstants.StandardBoardSize; i++) {
			for (uint j = 0; j < OthelloConstants.StandardBoardSize; j++) {
				gameManager.GetGameBoard().Board[i,j] = injectionBinder.GetInstance<IBoardSquareModel>() as IBoardSquareModel;
				uint mobilityScore = GetMobilityScore(i,j);
                gameManager.GetGameBoard().Board[i,j].MobilityScore = mobilityScore;
                //Debug.Log("Initialize Value for + " + i + "," + j + " - " + mobilityScore);
			}
		}
	}

    private uint GetMobilityScore(uint x, uint y)
    {
        uint weightScale = gameManager.GetGameBoard().BoardSize;

        uint xDist, yDist;
        //Distance to closest x edge
        if (x <= weightScale / 2)
        {
            xDist = x;
        }
        else
        {
            xDist = weightScale - x - 1;
        }

        if (y <= weightScale / 2)
        {
            yDist = y;
        }
        else
        {
            yDist = weightScale - y - 1;
        }

        int totalDistance = (int)Math.Min(xDist, yDist);

        //Give corners extra value
        if ((x == 0 || x == (weightScale - 1)) && (y == 0 || y == (weightScale - 1))) 
        {
            totalDistance -= 2;
        }


        return (uint)Math.Pow(2, (weightScale / 2.0) - totalDistance);
    }
}
