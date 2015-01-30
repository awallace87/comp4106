using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CreateSnakeCommand : Command {

	[Inject]
	public IGridManager gridManager { get; set; }


	public override void Execute ()
	{
		Debug.Log ("CreateSnakeCommand::Execute");

		ISnakeModel snake = injectionBinder.GetInstance<ISnakeModel> ();
		snake.Position = getSnakeStartPosition ();

		gridManager.AddGridObject (snake);

		//Create Snake View
		GameObject snakeObject = new GameObject("snake");
		SnakeView snakeView = snakeObject.AddComponent<SnakeView>();

		//Wire Both Together
		snakeView.ModelID = snake.GetID ();
	}

	GridPosition getSnakeStartPosition() {
		//Start Snake in middle of grid
		uint x = gridManager.GetGridWidth () / 2;
		uint y = gridManager.GetGridHeight () / 2;

		GridPosition center = new GridPosition (x, y);
		return center;
	}

}