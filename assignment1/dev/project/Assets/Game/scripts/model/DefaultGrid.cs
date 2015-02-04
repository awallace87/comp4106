using UnityEngine;
using System.Collections;

public class DefaultGrid : IGrid{
	private const uint DEFAULT_HEIGHT = 30;
	private const uint DEFAULT_WIDTH = 40;

	private uint height;
	private uint width;
    private GridObjectType[,] map;

	public DefaultGrid()
		: this(DEFAULT_WIDTH, DEFAULT_HEIGHT)
	{
	}

	public DefaultGrid(uint width, uint height)
	{
        this.width = width;
		this.height = height;
        InitializeMap();
	}

    void InitializeMap()
    {
        map = new GridObjectType[this.width, this.height];
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                map[i, j] = GridObjectType.Empty;
            }
        }
    }

	#region IGrid implementation
    public GridObjectType[,] Map
    {
        get { return map; }
    }

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
