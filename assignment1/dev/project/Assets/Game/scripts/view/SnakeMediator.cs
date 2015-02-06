using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class SnakeMediator : Mediator, IGridObjectMediator {

	[Inject]
	public SnakeView view { get; set; }

    [Inject]
    public IViewManager viewManager { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    private Signal<GridPosition> modelMovedSignal;
    private Signal modelRemovedSignal;

    public override void OnRegister ()
	{
		view.Initialize ();

        modelMovedSignal = new Signal<GridPosition>();
        modelMovedSignal.AddListener(onModelMoved);

        modelRemovedSignal = new Signal();
        modelRemovedSignal.AddListener(onModelRemoved);

        viewManager.AddMediator(this, view.ModelID);

        onModelMoved(gridManager.GetGridObject(view.ModelID).Position);
    }

    public override void OnRemove()
    {
        viewManager.RemoveMediator(view.ModelID);
    }

    private void onModelMoved(GridPosition position)
	{
        //Debug.Log("SnakeMediator::onModelMoved");
        view.ModelMovedSignal.Dispatch(position);
	}

    private void onModelRemoved()
    {
        Destroy(gameObject);
    }

    public Signal<GridPosition> ModelMovedSignal
    {
        get { return modelMovedSignal; }
    }


    public Signal ModelRemovedSignal
    {
        get { return modelRemovedSignal; }
    }
}
