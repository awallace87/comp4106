using UnityEngine;
using System.Collections;

public class DefaultWallModel : IWallModel {
    private uint id;
    private GridPosition position;

    public DefaultWallModel(IGridManager manager)
    {
        id = manager.GetNextGridObjectID();
    }

    public GridPosition Position
    {
        get { return position; }
        set { position = value; }
    }

    public uint GetID() { return id; }

    public GridObjectType GetGridObjectType()
    { return GridObjectType.Wall; }
}
