using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class RemoveGridObjectCommand : Command
{
    [Inject]
    public IGridObject objectToRemove { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    [Inject]
    public IViewManager viewManager { get; set; }

    public override void Execute()
    {
        //Get/Remove GridObject
        gridManager.RemoveGridObject(objectToRemove.GetID());

        //Get/Remove View GameObject
        IGridObjectMediator mediator = viewManager.GetMediator(objectToRemove.GetID());
        mediator.ModelRemovedSignal.Dispatch();

        viewManager.RemoveMediator(objectToRemove.GetID());
    }
}
