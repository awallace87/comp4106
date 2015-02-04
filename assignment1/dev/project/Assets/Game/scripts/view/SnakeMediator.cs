using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class SnakeMediator : Mediator , IGridObjectMediator {

	[Inject]
	public SnakeView view { get; set; }

    [Inject]
    public IViewManager viewManager { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    private Signal<GridPosition> modelMovedSignal;

    public override void OnRegister ()
	{
		view.Initialize ();
        modelMovedSignal = new Signal<GridPosition>();
        modelMovedSignal.AddListener(onModelMoved);

        viewManager.AddMediatorMoveSignal(modelMovedSignal, view.ModelID);

        onModelMoved(gridManager.GetObjectByID(view.ModelID).Position);
    }

    public override void OnRemove()
    {
        viewManager.RemoveMediatorMoveSignal(view.ModelID);
    }

    private void onModelMoved(GridPosition position)
	{
        //Debug.Log("SnakeMediator::onModelMoved");
        view.ModelMovedSignal.Dispatch(position);
	}

    public Signal<GridPosition> ModelMovedSignal
    {
        get { return modelMovedSignal; }
    }
}
