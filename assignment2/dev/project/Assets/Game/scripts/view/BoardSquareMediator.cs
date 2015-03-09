using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class BoardSquareMediator : Mediator 
{
	[Inject]
	public BoardSquareView view { get; set; }

    [Inject]
    public IGameManager gameManager { get; set; }

    [Inject]
    public OnMediatorRegisteredSignal mediatorRegisteredSignal { get; set; }

    [Inject]
    public BoardSquarePressedSignal boardSquarePressedSignal { get; set; }

	public override void OnRegister ()
	{
		view.Initialize ();
        view.ViewSelectedSignal.AddListener(onViewSelected);

        gameManager.GetGameBoard().Board[view.BoardSquarePosition.X, view.BoardSquarePosition.Y].StateChangedSignal.AddListener(onModelStateChanged);
        mediatorRegisteredSignal.Dispatch();
	}

    private void onViewSelected()
    {
        //Debug.Log("BoardSquare Selected - " + view.BoardSquarePosition.ToString());
        //BoardSquareState modelState = gameManager.GetGameBoard().Board[view.BoardSquarePosition.X, view.BoardSquarePosition.Y].State;

        boardSquarePressedSignal.Dispatch(view.BoardSquarePosition);
    }

    private void onModelStateChanged(BoardSquareState state)
    {
        //Debug.Log("ModelStateChanged");
        view.BoardSquareStateChangedSignal.Dispatch(state);
    }
}
