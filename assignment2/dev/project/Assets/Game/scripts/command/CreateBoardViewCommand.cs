using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class CreateBoardViewCommand : Command 
{

	public override void Execute ()
	{
		IResourceNameManager resourceNameManager = injectionBinder.GetInstance<IResourceNameManager> () as IResourceNameManager;

		//Create Board Object
		Action createBoardObjectAction = () =>
		{
			GameObject board = new GameObject(resourceNameManager.GetResourceName(ResourceID.BoardGO));
			board.transform.parent = GameObject.Find(resourceNameManager.GetResourceName(ResourceID.ContextGO)).transform;
		};

		Root.RootMainThreadActions.Enqueue (createBoardObjectAction);

		for (uint i = 0; i < OthelloConstants.StandardBoardSize; i++) {
			for (uint j = 0; j < OthelloConstants.StandardBoardSize; j++) {
				CreateBoardSquareView (i, j);
			}
		}
	}

	private void CreateBoardSquareView(uint x, uint y)
	{
		CreateBoardSquareViewSignal signal = injectionBinder.GetInstance<CreateBoardSquareViewSignal> () as CreateBoardSquareViewSignal;
		GridPosition boardSquarePosition = new GridPosition (x, y);
		signal.Dispatch (boardSquarePosition);
	}
}
