using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class DefaultViewManager : IViewManager {

    private Dictionary<uint, IGridObjectMediator> gridMediators;

    public DefaultViewManager()
    {
        //Debug.Log("DefaultViewManager()");
        this.gridMediators = new Dictionary<uint, IGridObjectMediator>();
    }

    public IGridObjectMediator GetMediator(uint id)
    {
        //Debug.Log("GetMediatorByID()");

        return gridMediators[id];
    }

    public void AddMediator(IGridObjectMediator mediator, uint id)
    {
        //Debug.Log("AddMediatorByID()");

        gridMediators.Add(id, mediator);
    }

    public void RemoveMediator(uint id)
    {
        //Debug.Log("RemoveMediatorByID()");

        gridMediators.Remove(id);
    }
}
