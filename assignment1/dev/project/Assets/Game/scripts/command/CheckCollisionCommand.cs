using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CheckCollisionCommand : Command
{
    [Inject]
    public IGridObject gridObject { get; set; }

    [Inject]
    public GridPosition nextPosition { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    public override void Execute()
    {
        Debug.Log("CheckCollisionCommand::Execute");
        //Check for Collisions and Move Accordingly
        GridObjectType destinationType = gridManager.Grid.Map[nextPosition.X, nextPosition.Y];

        if (destinationType != GridObjectType.Empty)
        {
            //A collision has occurred
        }
    }
}
