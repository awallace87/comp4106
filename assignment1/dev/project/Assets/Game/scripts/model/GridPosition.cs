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

    public GridPosition GetPositionInDirection(GridDirection direction)
    {
        GridPosition directionPosition = new GridPosition(this.X, this.Y);
        switch (direction)
        {
            case GridDirection.Up: directionPosition.Y++; break;
            case GridDirection.Down: directionPosition.Y--; break;
            case GridDirection.Left: directionPosition.X--; break;
            case GridDirection.Right: directionPosition.X++; break;
        }

        return directionPosition;
    }
}

public enum GridDirection 
{
	Invalid = -1,
    ValidDirectionStart,
	Up = ValidDirectionStart,
	Down,
	Left,
	Right,
    ValidDirectionEnd = Right
}
