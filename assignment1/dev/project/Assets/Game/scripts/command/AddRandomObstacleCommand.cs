using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class AddRandomObstacleCommand : Command 
{
	[Inject]
	public IGridManager gridManager { get; set; }

	public override void Execute ()
	{
		GridPosition wallPosition = GetRandomWallObstacle ();

		AddWallSignal wallSignal = injectionBinder.GetInstance<AddWallSignal> () as AddWallSignal;

		wallSignal.Dispatch (wallPosition);
	}

	GridPosition GetRandomWallObstacle()
	{
		bool foundUnoccupiedPosition = false;
		System.Random random = new System.Random();
		GridPosition startPosition = new GridPosition(gridManager.Grid.Width / 2, gridManager.Grid.Height / 2);

		while (!foundUnoccupiedPosition)
		{
			int minX, maxX, minY, maxY;
			minX = (int)(gridManager.Grid.Width * 0.1);
			maxX = (int)(gridManager.Grid.Width * 0.9);
			minY = (int)(gridManager.Grid.Height * 0.1);
			maxY = (int)(gridManager.Grid.Height * 0.9);
			uint xPosition = (uint)random.Next(minX, maxX);
			uint yPosition = (uint)random.Next(minY, maxY);
			
			startPosition = new GridPosition(xPosition, yPosition);
			
			foundUnoccupiedPosition = gridManager.Grid.Map[xPosition, yPosition] == GridObjectType.Empty;
		}

		return startPosition;
	}
}
