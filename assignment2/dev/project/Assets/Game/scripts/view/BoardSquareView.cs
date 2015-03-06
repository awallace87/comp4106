using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class BoardSquareView : View 
{
	public GridPosition boardSquarePosition { get; set; }

	internal void Initialize()
	{
		//Adjust Position
		Vector3 localPosition = transform.localPosition;
		localPosition.x = boardSquarePosition.X;
		localPosition.y = boardSquarePosition.Y;
		transform.localPosition = localPosition;
	}
}
