using UnityEngine;
using System.Collections;

public class DefaultGrid : IGrid{
	private const uint DEFAULT_HEIGHT = 30;
	private const uint DEFAULT_WIDTH = 40;

	private uint height;
	private uint width;

	public DefaultGrid()
		: this(DEFAULT_WIDTH, DEFAULT_HEIGHT)
	{
	}

	public DefaultGrid(uint width, uint height)
	{
		this.width = width;
		this.height = height;
	}

	#region IGrid implementation
	public uint Height {
		get { return height; }
		set { height = value; }
	}

	public uint Width {
		get { return width; }
		set { width = value; }
	}
	#endregion
}
