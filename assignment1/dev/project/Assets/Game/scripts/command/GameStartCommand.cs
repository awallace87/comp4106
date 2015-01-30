using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class GameStartCommand : Command {

	[Inject]
	public IGridManager gridManager {get; set;}

	public override void Execute ()
	{
		//Setup Grid
		//Move Camera to middle
		centerCamera ();

		//Add Food/Walls

		//Create Snake
	}

	void centerCamera()
	{
		float cameraX = gridManager.GetGridWidth () / 2;
		float cameraY = gridManager.GetGridHeight () / 2;

		Vector3 cameraPosition = Camera.main.transform.position;
		cameraPosition.x = cameraX;
		cameraPosition.y = cameraY;

		Camera.main.transform.position = cameraPosition;
	}
}
