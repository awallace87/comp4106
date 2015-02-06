using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public interface IViewManager {
    IGridObjectMediator GetMediator(uint id);
    void AddMediator(IGridObjectMediator mediator, uint id);
    void RemoveMediator(uint id);
}
