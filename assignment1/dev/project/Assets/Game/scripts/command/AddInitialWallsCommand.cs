using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class AddInitialWallsCommand : Command {

    [Inject]
    public AddWallSignal addWallSignal { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    public override void Execute()
    {
        for (uint i = 0; i < gridManager.Grid.Width; i++)
        {
            GridPosition bottomBorder = new GridPosition(i, 0);
            GridPosition topBorder = new GridPosition(i, gridManager.Grid.Height - 1);

            addWallSignal.Dispatch(bottomBorder);
            addWallSignal.Dispatch(topBorder);
        }

        for (uint i = 1; i < gridManager.Grid.Height - 1; i++)
        {
            GridPosition leftBorder = new GridPosition(0, i);
            GridPosition rightBorder = new GridPosition(gridManager.Grid.Width - 1, i);

            addWallSignal.Dispatch(leftBorder);
            addWallSignal.Dispatch(rightBorder);
        }
    }
}
