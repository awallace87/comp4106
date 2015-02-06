using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization;

public interface IGridManager {

    //Store GridObjects for View/Mediator Access
	IGridObject GetGridObject(uint gridObjectId);
    IGridObject GetGridObject(GridPosition position);

	void AddGridObject(IGridObject gridObject);
    void RemoveGridObject(uint gridObjectId);

    uint GetNextGridObjectID();

    List<uint> GetIDsOfType(GridObjectType gridObjectType);

    bool IsValidPosition(GridPosition position);
    
    IGrid Grid { get; }
}

public class GridObjectNotFoundException : Exception
{
    public GridObjectNotFoundException()
    {
        // Add implementation.
    }
    public GridObjectNotFoundException(string message)
    {
        // Add implementation.
    }
    public GridObjectNotFoundException(string message, Exception inner)
    {
        // Add implementation.
    }
    
    /*
    void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        throw new NotImplementedException();
    }
    */
}

public class GridPositionOutOfBoundsException : Exception
{
        public GridPositionOutOfBoundsException()
    {
        // Add implementation.
    }
    public GridPositionOutOfBoundsException(string message)
    {
        // Add implementation.
    }
    public GridPositionOutOfBoundsException(string message, Exception inner)
    {
        // Add implementation.
    }
}