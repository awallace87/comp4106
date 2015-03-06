using UnityEngine;
using System.Collections;

public class DefaultGameManager : IGameManager 
{
	IBoardModel board;
	IPlayer whitePlayer;
	IPlayer blackPlayer;

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
