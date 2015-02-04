using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

//TODO: Make interface
public class GridObjectView : View
{
    public uint ModelID { get; set; }
    
    GridPosition gridPosition;
    bool changePosition = true;
    
    protected Signal<GridPosition> modelMovedSignal;
    public Signal<GridPosition> ModelMovedSignal
    {
        get { return modelMovedSignal; }
    }

    protected void InitializeGridObjectView()
    {
        //Debug.Log("InitializeGridObjectView");
        //Setup Parent
        GameObject context = GameObject.Find("_context");	//TODO Use const for context transform
        this.transform.SetParent(context.transform);

        //Setup Listeners
        modelMovedSignal = new Signal<GridPosition>();
        ModelMovedSignal.AddListener(onModelMoved);
    }

    protected void onModelMoved(GridPosition newPosition)
    {
        gridPosition = newPosition;
        changePosition = true;
    }

    void Update()
    {
        if(changePosition)
        {
            //Debug.Log("ChangePosition");
            Vector3 updatePosition = transform.position;
            updatePosition.x = gridPosition.X;
            updatePosition.y = gridPosition.Y;
            transform.position = updatePosition;
            changePosition = false;
        }
    }
}
