using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class BoardSquareView : View 
{
    public GridPosition BoardSquarePosition { get; set; }

    private Signal viewSelectedSignal;
    public Signal ViewSelectedSignal
    {
        get { return viewSelectedSignal; }
    }

    private Signal<BoardSquareState> boardSquareStateChangedSignal;
    public Signal<BoardSquareState> BoardSquareStateChangedSignal
    {
        get { return boardSquareStateChangedSignal; }
    }

	internal void Initialize()
	{
		//Adjust Position
		Vector3 localPosition = transform.localPosition;
		localPosition.x = BoardSquarePosition.X;
		localPosition.y = BoardSquarePosition.Y;
		transform.localPosition = localPosition;

        //Initialize Signals
        this.viewSelectedSignal = new Signal();

        this.boardSquareStateChangedSignal = new Signal<BoardSquareState>();
        this.boardSquareStateChangedSignal.AddListener(onBoardSquareStateChanged);
	}

    private void onBoardSquareStateChanged(BoardSquareState state)
    {
        switch (state)
        {
            case BoardSquareState.Default: SetToDefault(); break;
            case BoardSquareState.Available: SetToAvailable(); break;
            case BoardSquareState.Affected: SetToAffected(); break;
            case BoardSquareState.Selected: SetToAffected(); break;
        }
    }

    void OnMouseDown()
    {
        this.ViewSelectedSignal.Dispatch();
    }

    private void SetToDefault()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    private void SetToAvailable()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    private void SetToAffected()
    {

    }

    private void SetToSelected()
    {

    }

}
