using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultGridManager : IGridManager {

	private static uint GridObjectID = 0;

    private IGrid grid;

    public IGrid Grid
    {
        get { return grid; }
    }

	private Dictionary<uint, IGridObject> gridObjects;

	public DefaultGridManager(IGrid grid) {
        this.grid = grid;
		this.gridObjects = new Dictionary<uint, IGridObject> ();
	}

	private uint getNextObjectId() 
	{
		return GridObjectID++;
	}


	#region IGridManager implementation
	public IGridObject GetObjectByID (uint gridObjectId)
	{
		return gridObjects [gridObjectId];
	}
	
	public void AddGridObject (IGridObject gridObject)
	{
		gridObjects.Add (gridObject.GetID (), gridObject);
	}

	public uint GetNextGridObjectID ()
	{
		return getNextObjectId();
	}

    public List<uint> GetIDsOfType(GridObjectType gridObjectType)
    {
        List<uint> idList = new List<uint>();

        foreach (KeyValuePair<uint, IGridObject> entry in gridObjects)
        {
            if (entry.Value.GetGridObjectType() == gridObjectType)
            {
                idList.Add(entry.Key);
            }
        }
        return idList;
    }

    public bool IsValidPosition(GridPosition position)
    {
        return (position.X < grid.Width && position.Y < grid.Height);
    }

	#endregion







}
