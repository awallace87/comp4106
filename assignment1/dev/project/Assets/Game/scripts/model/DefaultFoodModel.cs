using UnityEngine;
using System.Collections;

public class DefaultFoodModel : IFoodModel {

    private uint id;
    private GridPosition position;

    public DefaultFoodModel(IGridManager manager)
    {
        this.id = manager.GetNextGridObjectID();
        manager.AddGridObject(this);
    }

    public GridPosition Position
    {
        get { return position; }
        set 
        { 
            position = value;
        }
    }

    public uint GetID()
    {
        return id;
    }


    public GridObjectType GetGridObjectType() { return GridObjectType.Food; }
}
