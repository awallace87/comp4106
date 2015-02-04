using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class MoveAllSnakesCommand : Command
{
    [Inject]
    public IGridManager gridManager { get; set; }

    [Inject]
    public MoveSnakeSignal moveSignal { get; set; }
    public override void Execute()
    {
        Debug.Log("MoveAllSnakesCommand::Execute");

        //Find and Move Snakes
        List<uint> snakeIds = gridManager.GetIDsOfType(GridObjectType.SnakeHead);
        foreach (uint snakeID in snakeIds)
        {
            moveSignal.Dispatch(snakeID);
        }
    }
}
