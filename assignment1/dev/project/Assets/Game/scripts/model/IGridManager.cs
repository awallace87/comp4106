using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGridManager {

    //Store GridObjects for View/Mediator Access
	IGridObject GetObjectByID(uint gridObjectId);
	void AddGridObject(IGridObject gridObject);
    uint GetNextGridObjectID();

    List<uint> GetIDsOfType(GridObjectType gridObjectType);

    bool IsValidPosition(GridPosition position);

    IGrid Grid { get; }
}
