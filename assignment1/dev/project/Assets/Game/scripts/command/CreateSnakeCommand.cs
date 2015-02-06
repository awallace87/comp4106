using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CreateSnakeCommand : Command {

	[Inject]
	public IGridManager gridManager { get; set; }

    [Inject]
    public UpdateSnakeDirectionSignal updateDirectionSignal { get; set; }

	public override void Execute ()
	{
		Debug.Log ("CreateSnakeCommand::Execute");

		ISnakeModel snake = injectionBinder.GetInstance<ISnakeModel> ();
        snake.Position = GetSnakeStartingPosition();
        gridManager.AddGridObject(snake);
        //gridManager.Grid.Map[snake.Position.X, snake.Position.Y] = GridObjectType.SnakeHead;

        updateDirectionSignal.Dispatch(snake.GetID());

		//Create Snake View
		GameObject snakeObject = new GameObject("snake");
		SnakeView snakeView = snakeObject.AddComponent<SnakeView>();

		//Wire Both Together
		snakeView.ModelID = snake.GetID ();
	}

    GridPosition GetSnakeStartingPosition() 
    {
        bool foundUnoccupiedPosition = false;
        GridPosition startPosition = new GridPosition(gridManager.Grid.Width / 2, gridManager.Grid.Height / 2);
        while (!foundUnoccupiedPosition)
        {
            uint minX, maxX, minY, maxY;
            minX = (uint)(gridManager.Grid.Width * 0.25);
            maxX = (uint)(gridManager.Grid.Width * 0.75);
            minY = (uint)(gridManager.Grid.Height * 0.25);
            maxY = (uint)(gridManager.Grid.Height * 0.75);
            uint xPosition = (uint)Random.Range(minX, maxX);
            uint yPosition = (uint)Random.Range(minY, maxY);

            startPosition = new GridPosition(xPosition, yPosition);

            foundUnoccupiedPosition = gridManager.Grid.Map[xPosition, yPosition] == GridObjectType.Empty;
        }
        return startPosition;
    }
}