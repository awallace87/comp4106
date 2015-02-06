using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class UpdateAllSnakesCommand : Command
{
    [Inject]
    public IGridManager gridManager { get; set; }

    [Inject]
    public UpdateSnakeDirectionSignal updateSignal { get; set; }

    public override void Execute()
    {
        //Debug.Log("UpdateAllSnakesCommand::Execute");

        //Find and Move Snakes
        List<uint> snakeIds = gridManager.GetIDsOfType(GridObjectType.SnakeHead);
        foreach (uint snakeID in snakeIds)
        {
            updateSignal.Dispatch(snakeID);
        }
    }
}
