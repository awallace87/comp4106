using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using System.Collections.Generic;
using System;

//TODO: Make interface
public class GridObjectView : View
{
    public uint ModelID { get; set; }

    protected readonly Queue<Action> MainThreadActions = new Queue<Action>();

    
    GridPosition gridPosition;

    bool changePosition = true;
    bool toBeRemoved = false;
    
    protected Signal<GridPosition> modelMovedSignal;
    public Signal<GridPosition> ModelMovedSignal
    {
        get { return modelMovedSignal; }
    }

    protected Signal modelRemovedSignal;
    public Signal ModelRemovedSignal
    {
        get { return modelRemovedSignal; }
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

        modelRemovedSignal = new Signal();
        ModelRemovedSignal.AddListener(onModelRemoved);
    }

    protected void onModelMoved(GridPosition newPosition)
    {
        gridPosition = newPosition;
        MainThreadActions.Enqueue(() =>
        {
            Vector3 updatePosition = transform.position;
            updatePosition.x = gridPosition.X;
            updatePosition.y = gridPosition.Y;
            transform.position = updatePosition;
        });

        //changePosition = true;
    }

    protected void onModelRemoved()
    {
        Debug.Log("onModelRemoved");
        MainThreadActions.Enqueue(() =>
        {
            Destroy(gameObject);
        });

        //toBeRemoved = true;
    }

    void removeModel()
    {
        Destroy(gameObject);
    }

    void updatePosition()
    {
        Vector3 updatePosition = transform.position;
        updatePosition.x = gridPosition.X;
        updatePosition.y = gridPosition.Y;
        transform.position = updatePosition;
    }

    void Update()
    {
        while (MainThreadActions.Count > 0)
        {
            MainThreadActions.Dequeue().Invoke();
        }
        /*
        if (toBeRemoved)
        {
            Debug.Log("DestroyFood");
            return;
        }

        if(changePosition)
        {
            //Debug.Log("ChangePosition");
            Vector3 updatePosition = transform.position;
            updatePosition.x = gridPosition.X;
            updatePosition.y = gridPosition.Y;
            transform.position = updatePosition;
            changePosition = false;
        }*/
    }
}
