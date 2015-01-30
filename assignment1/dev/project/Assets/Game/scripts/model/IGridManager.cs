using UnityEngine;
using System.Collections;

public interface IGridManager {

	//Store GridObjects for View/Mediator Access
	IGridObject GetObjectByID(uint gridObjectId);
	void AddGridObject(IGridObject gridObject);
	uint GetNextGridObjectID();

	uint GetGridHeight();
	uint GetGridWidth();
}
