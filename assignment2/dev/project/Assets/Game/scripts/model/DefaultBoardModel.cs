using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DefaultBoardModel : IBoardModel 
{
	private IBoardSquareModel[,] board;
    private uint boardSize;

	public DefaultBoardModel()
	{
        boardSize = OthelloConstants.StandardBoardSize;
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

    public uint BoardSize
    {
        get
        {
            return boardSize;
        }
    }

	public IList<GridPosition> GetLegalMoves (DiscColour player)
	{
        List<GridPosition> legalMoves = new List<GridPosition>();

        for (uint i = 0; i < OthelloConstants.StandardBoardSize; i++)
        {
            for (uint j = 0; j < OthelloConstants.StandardBoardSize; j++)
            {
                if (isLegalMove(i, j, player))
                {
                    GridPosition flankingPosition = new GridPosition(i, j);
                    legalMoves.Add(flankingPosition);
                }
            }
        }
        return legalMoves;
	}

    public IList<GridPosition> GetAffectedDiscPositions(DiscColour player, GridPosition playLocation)
    {
        List<GridPosition> affectedPositions = new List<GridPosition>();
        if (isLegalMove(playLocation.X, playLocation.Y, player))
        {
            for (int i = (int)GridDirection.ValidDirectionStart; i <= (int)GridDirection.ValidDirectionEnd; i++)
            {
                affectedPositions.AddRange(getFlankedPositions(playLocation, (GridDirection)i, player));
            }
        }
        return affectedPositions;
    }
	#endregion

    bool isLegalMove(uint x, uint y, DiscColour player)
    {
        if (isPositionInBounds(x, y) && !board[x,y].ContainsDisc())
        {
            return checkFlanking(x,y, player);
        }

        return false;
    }
    
	bool isPositionInBounds(uint x, uint y)
	{
        try
        {
            bool inBounds = x < boardSize || y < boardSize;
            return inBounds;
        }
        catch (IndexOutOfRangeException e)
        {
            return false;
        }
	}

    bool checkFlanking(uint x, uint y, DiscColour player)
    {
        bool doesFlank = false;
        GridPosition currentPosition = new GridPosition(x,y);

        for (int i = (int)GridDirection.ValidDirectionStart; i <= (int)GridDirection.ValidDirectionEnd; i++)
        {
            bool isFlankingPosition = getFlankedPositions(currentPosition, (GridDirection)i, player).Count > 0;
            doesFlank = isFlankingPosition;
            if (isFlankingPosition)
            {
                break;
            }
        }
        return doesFlank;
    }

    IList<GridPosition> getFlankedPositions(GridPosition startPosition, GridDirection searchDirection, DiscColour playerColour)
    {
        List<GridPosition> affectedPositions = new List<GridPosition>();

        GridPosition bracketEnd = startPosition.GetPositionInDirection(searchDirection);

        try
        {
            if (!isPositionInBounds(bracketEnd.X, bracketEnd.Y))
                return affectedPositions;

            IBoardSquareModel bracketEndSquare = board[bracketEnd.X, bracketEnd.Y];
            if (!bracketEndSquare.ContainsDisc())
                return affectedPositions;

            bool isDiscOpponent = bracketEndSquare.Disc.Colour != playerColour;

            if (isDiscOpponent)
            {
                affectedPositions.Add(bracketEnd);
            }
            else
            {
                return affectedPositions;
            }

            while (isDiscOpponent)
            {
                bracketEnd = bracketEnd.GetPositionInDirection(searchDirection);
                if (!isPositionInBounds(bracketEnd.X, bracketEnd.Y))
                {
                    affectedPositions.Clear();
                    return affectedPositions;
                }    

                bracketEndSquare = board[bracketEnd.X, bracketEnd.Y];
                if (!bracketEndSquare.ContainsDisc())
                {
                    affectedPositions.Clear();
                    return affectedPositions;
                }

                isDiscOpponent = bracketEndSquare.Disc.Colour != playerColour;
                if (isDiscOpponent)
                {
                    affectedPositions.Add(bracketEnd);
                }
            }

            //Debug.Log("Found Legal Move for " + playerColour.ToString() + " on "
            //    + startPosition.ToString() + " in Direction - " + searchDirection.ToString());
            return affectedPositions;
        }
        catch (IndexOutOfRangeException e)
        {
            //Debug.Log("FindEndOfFlank - " + bracketEnd.ToString() + " Out of Bounds");
            return affectedPositions;
        } 
    }
}
