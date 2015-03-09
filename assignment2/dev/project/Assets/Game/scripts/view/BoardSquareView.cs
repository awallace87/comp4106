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

    [Inject]
    public IResourceNameManager resourceManager { get; set; }

    private Sprite[] boardSquareSprites;

	internal void Initialize()
	{
        boardSquareSprites = Resources.LoadAll<Sprite>(resourceManager.GetResourceName(ResourceID.BoardSquareSprite));
        SetToDefault();

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
        GetComponent<SpriteRenderer>().sprite = boardSquareSprites[0];
    }

    private void SetToAvailable()
    {
        GetComponent<SpriteRenderer>().sprite = boardSquareSprites[1];
    }

    private void SetToAffected()
    {
        GetComponent<SpriteRenderer>().sprite = boardSquareSprites[2];
    }

    private void SetToSelected()
    {
        GetComponent<SpriteRenderer>().sprite = boardSquareSprites[2];
    }

}
