using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class GameStartCommand : Command {

	[Inject]
	public IGridManager gridManager {get; set;}

    [Inject]
    public IUpdateManager updateManager { get; set; }

	public override void Execute ()
	{
		//Setup Grid
		//Move Camera to middle
		centerCamera ();

        updateManager.Initialize();
        updateManager.StartCycle();
	}

	void centerCamera()
	{
		float cameraX = gridManager.Grid.Width / 2;
		float cameraY = gridManager.Grid.Height / 2;

		Vector3 cameraPosition = Camera.main.transform.position;
		cameraPosition.x = cameraX;
		cameraPosition.y = cameraY;

		Camera.main.transform.position = cameraPosition;
	}
}
