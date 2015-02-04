using UnityEngine;
using System.Collections;

public class FoodView : GridObjectView {
    
    internal void Initialize()
    {
        InitializeGridObjectView();

        //Create Cube
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.SetParent(this.transform);

        //Set Color
        cube.renderer.material.color = Color.red;
    }
}
