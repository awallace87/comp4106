using UnityEngine;
using System.Collections;

public interface IGridObject {
	GridObjectMovedSignal MoveSignal { get; }
	GridPosition Position { get; set;}
	uint GetID();
}

