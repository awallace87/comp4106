using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class IncrementSnakeTailCommand : Command
{
    [Inject]
    public uint snakeModelID { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    [Inject]
    public IViewManager viewManager { get; set; }

    public override void Execute()
    {
        Debug.Log("IncrementSnakeTailCommand::Execute()");
        ISnakeModel snakeModel = gridManager.GetGridObject(snakeModelID) as ISnakeModel;

        while (snakeModel.Next != null)
        {
            snakeModel = snakeModel.Next;
        }

        ISnakeModel snakeTail = injectionBinder.GetInstance<ISnakeModel>(SnakeBindings.Tail);
        snakeTail.Position = snakeModel.Position;
        gridManager.AddGridObject(snakeTail);
        snakeModel.Next = snakeTail;

        Action createSnakeTail = () =>
        {
            //Create Snake View
            GameObject snakeObject = new GameObject("snakeTail");
            SnakeView snakeView = snakeObject.AddComponent<SnakeView>();

            //Wire Both Together
            snakeView.ModelID = snakeTail.GetID();
        };

        Root.RootMainThreadActions.Enqueue(createSnakeTail);
    }
}
