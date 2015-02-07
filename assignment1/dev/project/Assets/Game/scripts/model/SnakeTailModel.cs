using UnityEngine;
using System.Collections;

public class SnakeTailModel : ISnakeModel {

    private GridDirection direction;
    private GridPosition nextPosition;
    private GridPosition position;
    private uint id;

    private ISnakeModel next = null;

   	public SnakeTailModel(IGridManager manager)
	{
        this.id = manager.GetNextGridObjectID();
        direction = GridDirection.Invalid;
	}


    public GridDirection Direction
    {
        get { return direction; }
        set { direction = value; }
    }

    public GridPosition NextPosition
    {
        get { return nextPosition; }
        set { nextPosition = value; }
    }

    public ISnakeModel Next
    {
        get { return next; }
        set { next = value; }
    }

    public GridPosition Position
    {
        get { return position; }
        set { position = value; }
    }

    public uint GetID() { return id; }

    public GridObjectType GetGridObjectType()  { return GridObjectType.SnakeTail; }
}
