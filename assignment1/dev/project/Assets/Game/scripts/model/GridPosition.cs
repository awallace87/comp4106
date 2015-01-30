using UnityEngine;
using System.Collections;

public class GridPosition {

	public uint X { get; set; }
	public uint Y { get; set; }

	public GridPosition(uint x, uint y) 
	{
		this.X = x;
		this.Y = y;
	}
}

public enum GridDirection 
{
	Invalid,
	Up,
	Down,
	Left,
	Right
}
