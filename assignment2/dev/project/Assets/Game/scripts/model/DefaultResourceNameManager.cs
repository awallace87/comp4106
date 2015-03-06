using UnityEngine;
using System.Collections;

public class DefaultResourceNameManager : IResourceNameManager 
{
	#region IResourceNameManager implementation

	public string GetResourceName (ResourceID id)
	{
		string resourceName = "";
		switch (id) 
		{
		case ResourceID.BoardGO: resourceName = ResourceNames.BoardGameObject; break;
		case ResourceID.ContextGO: resourceName = ResourceNames.ContextGameObject; break;
		case ResourceID.BoardSquarePrefab: resourceName = ResourceNames.BoardSquarePrefab; break;
		case ResourceID.OthelloDiscPrefab: resourceName = ResourceNames.OthelloDiscPrefab; break;
		}

		return resourceName;
	}

	#endregion
}
