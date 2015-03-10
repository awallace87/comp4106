using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class CreateBoardViewCommand : Command
{
    [Inject]
    public OnMediatorRegisteredSignal mediatorRegisteredSignal { get; set; }

    private int viewsPendingRegistrationCount = 0;

	public override void Execute ()
	{
		IResourceNameManager resourceNameManager = injectionBinder.GetInstance<IResourceNameManager> () as IResourceNameManager;

		//Create Board Object
		Action createBoardObjectAction = () =>
		{
			GameObject board = new GameObject(resourceNameManager.GetResourceName(ResourceID.BoardGO));
			board.transform.parent = GameObject.Find(resourceNameManager.GetResourceName(ResourceID.ContextGO)).transform;
		};

		Root.RootMainThreadActions.Enqueue (createBoardObjectAction);

		for (uint i = 0; i < OthelloConstants.StandardBoardSize; i++) {
			for (uint j = 0; j < OthelloConstants.StandardBoardSize; j++) {
				CreateBoardSquareView (i, j);
			}
		}

        mediatorRegisteredSignal.AddListener(onMediatorRegistered);
        Retain();
	}

	private void CreateBoardSquareView(uint x, uint y)
	{
		CreateBoardSquareViewSignal signal = injectionBinder.GetInstance<CreateBoardSquareViewSignal> () as CreateBoardSquareViewSignal;
		GridPosition boardSquarePosition = new GridPosition (x, y);
		signal.Dispatch (boardSquarePosition);
        viewsPendingRegistrationCount++;
	}

    private void onMediatorRegistered()
    {
        Debug.Log("Mediator Registered - Current Count : " + viewsPendingRegistrationCount);
        viewsPendingRegistrationCount--;
        if (viewsPendingRegistrationCount <= 0)
        {
            mediatorRegisteredSignal.RemoveListener(onMediatorRegistered);
            Release();
        }
    }
}
