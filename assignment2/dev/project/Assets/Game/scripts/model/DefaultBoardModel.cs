using UnityEngine;
using System.Collections;

public class DefaultBoardModel : IBoardModel 
{
	private IBoardSquareModel[,] board;

	public DefaultBoardModel()
	{

	}
	
	#region IBoardModel implementation

	public IBoardSquareModel[,] Board {
		get {
			return board;
		}
		set {
			board = value;
		}
	}

	public System.Collections.Generic.IList<GridPosition> GetLegalMoves (DiscColour player)
	{
		throw new System.NotImplementedException ();
	}
	#endregion

	bool isLegalMove(uint x, uint y)
	{
		bool inBounds = x < OthelloConstants.StandardBoardSize || y < OthelloConstants.StandardBoardSize;
		return inBounds;
	}
}
