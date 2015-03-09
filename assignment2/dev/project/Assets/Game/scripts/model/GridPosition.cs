using UnityEngine;
using System.Collections;

public class GridPosition {

	public uint X { get; set; }
	public uint Y { get; set; }

	public GridPosition(uint x, uint y) 
	{
		this.X = x;
		this.Y = y;
	}

    public override bool Equals(object obj)
    {
        if (obj.GetType().Equals(typeof(GridPosition)))
        {
            GridPosition gridPosition = (GridPosition)obj;
            return (gridPosition.X == this.X && gridPosition.Y == this.Y);
        }
        else
        {
            return false;
        }
    }

    public override string ToString()
    {
        return string.Format("({0},{1})",this.X, this.Y);
    }

    public GridPosition GetPositionInDirection(GridDirection direction)
    {
        GridPosition directionPosition = new GridPosition(this.X, this.Y);
        switch (direction)
        {
            case GridDirection.Up: directionPosition.Y++; break;
            case GridDirection.Down: directionPosition.Y--; break;
            case GridDirection.Left: directionPosition.X--; break;
            case GridDirection.Right: directionPosition.X++; break;
            case GridDirection.UpLeft:
                {
                    directionPosition.Y++;
                    directionPosition.X--;
                }
                break;
            case GridDirection.UpRight:
                {
                    directionPosition.Y++;
                    directionPosition.X++;
                }
                break;
            case GridDirection.DownRight:
                {
                    directionPosition.Y--;
                    directionPosition.X++;
                }
                break;
            case GridDirection.DownLeft:
                {
                    directionPosition.Y--;
                    directionPosition.X--;
                }
                break;
        }

        return directionPosition;
    }

    public GridDirection GetDirectionOfPosition(GridPosition position)
    {
        GridDirection directionOfPosition = GridDirection.Invalid;
        if (this.X == position.X)
        {
            Debug.Log("Position in X");
            if (this.Y + 1 == position.Y)
            {
                directionOfPosition = GridDirection.Up;
            }
            else if (this.Y - 1 == position.Y)
            {
                directionOfPosition = GridDirection.Down;
            }
        }
        else if (this.Y == position.Y)
        {
            if (this.X + 1 == position.X)
            {
                directionOfPosition = GridDirection.Right;
            }
            else if (this.X - 1 == position.X)
            {
                directionOfPosition = GridDirection.Left;
            }
        }
        return directionOfPosition;
    }
}

public enum GridDirection 
{
	Invalid = -1,
    ValidDirectionStart,
	Up = ValidDirectionStart,
    UpLeft,
	Left,
    DownLeft,
    Down,
    DownRight,
	Right,
    UpRight,
    ValidDirectionEnd = UpRight
}
