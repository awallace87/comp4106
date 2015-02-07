using UnityEngine;
using System.Collections;

public class DefaultSnakeModel : ISnakeModel{

    private GridDirection direction;
    private GridPosition nextPosition;
	private GridPosition position;
	private uint id;

    private ISnakeModel next = null;
	
	public DefaultSnakeModel(IGridManager manager)
	{
        this.id = manager.GetNextGridObjectID();
        direction = GridDirection.Invalid;
	}

	#region IGridObject
	public GridPosition Position {
		get { return position; }
		set 
        { 
            position = value;
        }
	}

    public GridDirection Direction
    {
        get { return direction; }
        set { direction = value; }
    }
	
	public uint GetID ()
	{
		return id;
	}

    public GridObjectType GetGridObjectType() { return GridObjectType.SnakeHead; }

	#endregion

	#region ISnakeModel implementation

	public ISnakeModel Next
	{
        get { return next; }
        set { next = value; }
	}



    public GridPosition NextPosition
    {
        get { return nextPosition;  }
        set { nextPosition = value; }
    }


    #endregion
}
