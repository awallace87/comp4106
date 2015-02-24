using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class SnakeView : GridObjectView {

    internal void Initialize(NavigationMethod method)
	{
        InitializeGridObjectView();

		//Create Cube
		GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
		cube.transform.SetParent (this.transform);

		cube.renderer.material.color = GetSnakeColor(method);
	}

	Color GetSnakeColor(NavigationMethod method)
	{
		switch (method) {
		case NavigationMethod.AStarEuclidean:
				{
						return Color.blue;
				}
				break;
		case NavigationMethod.AStarManhattan:
				{
						return Color.cyan;
				}
				break;
		case NavigationMethod.BreadthFirst:
				{
						return Color.gray;
				}
				break;
		case NavigationMethod.DepthFirst:
				{
						return Color.yellow;
				}
				break;
		case NavigationMethod.AStarAverage:
				{
						return Color.magenta;
				}
		}

		return Color.green;
	}
}
