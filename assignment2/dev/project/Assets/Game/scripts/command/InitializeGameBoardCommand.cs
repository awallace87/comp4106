using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class InitializeGameBoardCommand : Command {

	public override void Execute ()
	{
		IGameManager gameManager = injectionBinder.GetInstance<IGameManager> () as IGameManager;

		gameManager.GetGameBoard ().Board = new IBoardSquareModel[OthelloConstants.StandardBoardSize, OthelloConstants.StandardBoardSize];

		for (uint i = 0; i < OthelloConstants.StandardBoardSize; i++) {
			for (uint j = 0; j < OthelloConstants.StandardBoardSize; j++) {
				gameManager.GetGameBoard().Board[i,j] = injectionBinder.GetInstance<IBoardSquareModel>() as IBoardSquareModel;
				gameManager.GetGameBoard().Board[i,j].MobilityScore = 1;
			}
		}

	}
}
