using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class SnakeView : GridObjectView {

	private Signal<GridPosition> modelMovedSignal;
	public Signal<GridPosition> ModelMovedSignal { 
		get { return modelMovedSignal; }
	}

	internal void initialize()
	{

		//Setup Parent
		GameObject context = GameObject.Find ("_context");	//TODO Use const for context transform
		this.transform.SetParent (context.transform);
		//Create Cube
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.SetParent (this.transform);

		//Setup Listeners
		modelMovedSignal = new Signal<GridPosition> ();
		ModelMovedSignal.AddListener (onModelMoved);
	}

	void onModelMoved(GridPosition newPosition)
	{
		Vector3 objectPosition = gameObject.transform.position;
		objectPosition.x = newPosition.X;
		objectPosition.y = newPosition.Y;

		gameObject.transform.position = objectPosition;
	}
}
