using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class DefaultGameManager : IGameManager 
{
	IBoardModel board;

    DiscColour currentTurn;

	IPlayer whitePlayer;
	IPlayer blackPlayer;

    [Inject]
    public StartTurnSignal playTurnSignal { get; set; }

	public DefaultGameManager(IBoardModel board)
	{
		Debug.Log ("Initialize Board");
		this.board = board;
	}

	#region IGameManager implementation
	public IBoardModel GetGameBoard ()
	{
		return board;
	}

    public DiscColour CurrentTurn
    {
        get
        {
            return currentTurn;
        }
        set
        {
            currentTurn = value;
            playTurnSignal.Dispatch(currentTurn);
        }
    }

	public IPlayer WhitePlayer {
		get {
			return whitePlayer;
		}
		set {
			whitePlayer = value;
		}
	}

	public IPlayer BlackPlayer {
		get {
			return blackPlayer;
		}
		set {
			blackPlayer = value;
		}
	}
	#endregion
}
