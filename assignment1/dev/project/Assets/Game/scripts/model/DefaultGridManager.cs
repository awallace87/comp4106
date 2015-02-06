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
	public IGridObject GetGridObject (uint gridObjectId)
	{
        if (gridObjects.ContainsKey(gridObjectId))
        {
            return gridObjects[gridObjectId];
        }
        throw new GridObjectNotFoundException();
	}
	
	public void AddGridObject (IGridObject gridObject)
	{
		gridObjects.Add (gridObject.GetID (), gridObject);
        //TODO: throw out of bounds exception
        //TODO: check for collision
        Grid.Map[gridObject.Position.X, gridObject.Position.Y] = gridObject.GetGridObjectType();
	}

    public void RemoveGridObject(uint gridObjectId)
    {
        if (gridObjects.ContainsKey(gridObjectId))
        {
            IGridObject gridObject = gridObjects[gridObjectId];
            Grid.Map[gridObject.Position.X, gridObject.Position.Y] = GridObjectType.Empty;
            gridObjects.Remove(gridObjectId);
        }
        else
        {
            throw new GridObjectNotFoundException();
        }
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

    public IGridObject GetGridObject(GridPosition position)
    {
        foreach (KeyValuePair<uint, IGridObject> entry in gridObjects)
        {
            if (entry.Value.Position.Equals(position))
            {
                return entry.Value;
            }
        }
        throw new GridObjectNotFoundException();
    }
	#endregion

}
