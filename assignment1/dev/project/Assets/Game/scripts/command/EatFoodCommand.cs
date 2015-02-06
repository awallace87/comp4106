using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class EatFoodCommand : Command
{
    [Inject]
    public GridPosition foodPosition { get; set; }

    [Inject]
    public uint snakeModelID { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    public override void Execute()
    {
        try
        {
            RemoveGridObjectSignal removeSignal = injectionBinder.GetInstance<RemoveGridObjectSignal>();
            removeSignal.Dispatch(gridManager.GetGridObject(foodPosition));
            Debug.Log("Eat Food Command");
            //Update Score/Snake Length
        }
        catch (GridObjectNotFoundException gridException)
        {
            Debug.Log("Food Not Found at Location");
        }
    }
}
