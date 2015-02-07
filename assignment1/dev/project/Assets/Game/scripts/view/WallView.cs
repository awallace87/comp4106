using UnityEngine;
using System.Collections;

public class WallView : GridObjectView
{

    internal void Initialize()
    {
        InitializeGridObjectView();

        //Create cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(this.transform);

        //Set Color
        cube.renderer.material.color = Color.black;
    }
}
