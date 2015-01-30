using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DefaultGridManager : IGridManager {

	private static uint GridObjectID = 0;

	[Inject]
	public IGrid Grid { get; set; }

	private Dictionary<uint, IGridObject> gridObjects;

	public DefaultGridManager() {
		gridObjects = new Dictionary<uint, IGridObject> ();
	}

	[PostConstruct]
	void initializeGrid()
	{
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
	

	public GridPosition GetSnakeStartPosition ()
	{
		GridPosition startPosition = new GridPosition (0, 0);
		return startPosition;
	}
	
	void IGridManager.AddGridObject (IGridObject gridObject)
	{
		gridObjects.Add (gridObject.GetID (), gridObject);
	}

	public uint GetNextGridObjectID ()
	{
		return getNextObjectId();
	}
	
	public uint GetGridHeight ()
	{
		return Grid.Height;
	}

	public uint GetGridWidth ()
	{
		return Grid.Width;
	}

	#endregion

}
