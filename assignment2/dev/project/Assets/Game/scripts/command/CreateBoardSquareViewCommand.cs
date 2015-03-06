using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class CreateBoardSquareViewCommand : Command 
{
	[Inject]
	public GridPosition squarePosition { get; set; }

	public override void Execute ()
	{
		IResourceNameManager resourceNameManager = injectionBinder.GetInstance<IResourceNameManager> () as IResourceNameManager;
		
		//Create Board Object
		Action createBoardObjectAction = () =>
		{
			GameObject boardSquare = (GameObject) GameObject.Instantiate(Resources.Load(resourceNameManager.GetResourceName(ResourceID.BoardSquarePrefab)));
			BoardSquareView boardSquareView = boardSquare.AddComponent<BoardSquareView>();
			boardSquareView.boardSquarePosition = squarePosition;

			boardSquare.transform.parent = GameObject.Find(resourceNameManager.GetResourceName(ResourceID.BoardGO)).transform;
		};
		
		Root.RootMainThreadActions.Enqueue (createBoardObjectAction);

	}
}
