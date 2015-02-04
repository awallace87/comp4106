using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class SnakeView : GridObjectView {

    internal void Initialize()
	{
        InitializeGridObjectView();

		//Create Cube
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.SetParent (this.transform);

        //Set Color
        cube.renderer.material.color = Color.green;
	}
}
