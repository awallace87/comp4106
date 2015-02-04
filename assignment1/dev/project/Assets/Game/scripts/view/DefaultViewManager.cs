using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.signal.impl;

public class DefaultViewManager : IViewManager {

    private Dictionary<uint, Signal<GridPosition>> gridMediators;

    public DefaultViewManager()
    {
        //Debug.Log("DefaultViewManager()");
        this.gridMediators = new Dictionary<uint, Signal<GridPosition>>();
    }

    public Signal<GridPosition> GetMediatorMoveSignal(uint id)
    {
        //Debug.Log("GetMediatorByID()");

        return gridMediators[id];
    }

    public void AddMediatorMoveSignal(Signal<GridPosition> mediatorSignal, uint id)
    {
        //Debug.Log("AddMediatorByID()");

        gridMediators.Add(id, mediatorSignal);
    }

    public void RemoveMediatorMoveSignal(uint id)
    {
        //Debug.Log("RemoveMediatorByID()");

        gridMediators.Remove(id);
    }
}
