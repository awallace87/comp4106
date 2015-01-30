using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class SnakeMediator : Mediator {

	[Inject]
	public SnakeView view { get; set; }

	[Inject]
	public IGridManager manager { get; set; }


	public override void OnRegister ()
	{
		view.initialize ();
		uint modelID = view.ModelID;

		//TODO Handle model not present
		IGridObject model = manager.GetObjectByID (modelID);
		model.MoveSignal.AddListener (onModelMoved);
		onModelMoved (model.Position);
	}

	private void onModelMoved(GridPosition gridPosition)
	{
		view.ModelMovedSignal.Dispatch (gridPosition);
	}
}
