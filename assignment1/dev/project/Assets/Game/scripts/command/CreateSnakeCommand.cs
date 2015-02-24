using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class CreateSnakeCommand : Command {

	[Inject]
	public IGridManager gridManager { get; set; }

	[Inject]
	public INavigationManager navigationManager { get; set; }

    [Inject]
    public UpdateSnakeDirectionSignal updateDirectionSignal { get; set; }

    [Inject]
    public IncrementSnakeTailSignal incrementSnakeSignal { get; set; }

	[Inject]
	public NavigationMethod navigationMethod { get; set; }

	public override void Execute ()
	{
		Debug.Log ("CreateSnakeCommand::Execute");

		ISnakeModel snake = injectionBinder.GetInstance<ISnakeModel> (SnakeBindings.Head);
        snake.Position = GetSnakeStartingPosition();
        gridManager.AddGridObject(snake);

		navigationManager.AddSnake (snake.GetID (), navigationMethod);

        updateDirectionSignal.Dispatch(snake.GetID());

        Action createSnakeAction = () =>
        {
            //Create Snake View
            GameObject snakeObject = new GameObject("snake");
            SnakeView snakeView = snakeObject.AddComponent<SnakeView>();

            //Wire Both Together
            snakeView.ModelID = snake.GetID();
        };

        Root.RootMainThreadActions.Enqueue(createSnakeAction);

        for (int i = 0; i < 7; i++)
        {
            incrementSnakeSignal.Dispatch(snake.GetID());
        }
	}

    GridPosition GetSnakeStartingPosition() 
    {
        bool foundUnoccupiedPosition = false;
        System.Random random = new System.Random();
        GridPosition startPosition = new GridPosition(gridManager.Grid.Width / 2, gridManager.Grid.Height / 2);
        while (!foundUnoccupiedPosition)
        {
            int minX, maxX, minY, maxY;
            minX = (int)(gridManager.Grid.Width * 0.25);
            maxX = (int)(gridManager.Grid.Width * 0.75);
            minY = (int)(gridManager.Grid.Height * 0.25);
            maxY = (int)(gridManager.Grid.Height * 0.75);
            uint xPosition = (uint)random.Next(minX, maxX);
            uint yPosition = (uint)random.Next(minY, maxY);

            startPosition = new GridPosition(xPosition, yPosition);

            foundUnoccupiedPosition = gridManager.Grid.Map[xPosition, yPosition] == GridObjectType.Empty;
        }
        return startPosition;
    }
}