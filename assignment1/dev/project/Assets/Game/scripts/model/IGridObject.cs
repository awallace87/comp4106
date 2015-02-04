using UnityEngine;
using System.Collections;

public interface IGridObject {
	GridPosition Position { get; set;}
	uint GetID();
    GridObjectType GetGridObjectType();
}

public enum GridObjectType
{
    SnakeHead
    , SnakeTail
    , Wall
    , Food
    , Empty
}

