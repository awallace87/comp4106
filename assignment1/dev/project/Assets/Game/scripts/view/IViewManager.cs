using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public interface IViewManager {
    Signal<GridPosition> GetMediatorMoveSignal(uint id);
    void AddMediatorMoveSignal(Signal<GridPosition> mediatorSignal, uint id);
    void RemoveMediatorMoveSignal(uint id);
}
