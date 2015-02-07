using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class AddWallCommand : Command
{
    [Inject]
    public GridPosition wallPosition { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    [Inject]
    public IViewManager viewManager { get; set; }

    public override void Execute()
    {
        if (gridManager.Grid.Map[wallPosition.X, wallPosition.Y] != GridObjectType.Empty)
        {
            Debug.Log("Attempt to create Wall at occupied position");
            return;
        }

        IWallModel wallModel = injectionBinder.GetInstance<IWallModel>();
        wallModel.Position = wallPosition;
        gridManager.AddGridObject(wallModel);

        Action createWallAction = () =>
        {
            GameObject wallObject = new GameObject("wall");
            WallView wallView = wallObject.AddComponent<WallView>();
            wallView.ModelID = wallModel.GetID();
        };

        Root.RootMainThreadActions.Enqueue(createWallAction);
        
    }
}
