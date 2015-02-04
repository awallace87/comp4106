using UnityEngine;
using System.Collections;

public class DefaultSnakeModel : ISnakeModel{

    private GridDirection direction;
	private GridPosition position;
	private uint id;
	
	public DefaultSnakeModel(IGridManager manager)
	{
        this.id = manager.GetNextGridObjectID();
        manager.AddGridObject(this);
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

	public void Move ()
	{
	}

	#endregion



}
