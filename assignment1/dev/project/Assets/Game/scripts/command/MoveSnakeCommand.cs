using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class MoveSnakeCommand : Command
{
    [Inject]
    public uint modelID { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    [Inject]
    public IViewManager viewManager { get; set; }

    ISnakeModel snakeModel;
    public override void Execute()
    {
        //TODO: Add Exception for Invalid Move
        //Get Updated Position
        //Debug.Log("MoveSnakeCommand::Execute");
        snakeModel = gridManager.GetGridObject(modelID) as ISnakeModel;

        if (!gridManager.IsValidPosition(snakeModel.NextPosition))
        {
            //Out of Bounds
            //TODO - Handle
            return;
        }
        //Check if a collision will occur
        GridObjectType destinationType = gridManager.Grid.Map[snakeModel.NextPosition.X, snakeModel.NextPosition.Y];

        switch (destinationType)
        {
            case GridObjectType.Food:
                {
                    //Eat Food
                    EatFood(snakeModel.NextPosition);
                    MoveSnake(snakeModel.NextPosition);
                }
                break;
            case GridObjectType.Empty:
                {
                    //Move
                    MoveSnake(snakeModel.NextPosition);
                }
                break;
            default:
                {
                    //Death
                }
                break;
        }
        //Debug.Log("MoveSnakeCommand::Execute End");
    }

    void MoveSnake(GridPosition position)
    {
        GridPosition snakePriorPosition = snakeModel.Position;
        //Debug.Log("Move Snake Position");
        gridManager.Grid.Map[snakeModel.Position.X, snakeModel.Position.Y] = GridObjectType.Empty;
        snakeModel.Position = position;

        gridManager.Grid.Map[snakeModel.Position.X, snakeModel.Position.Y] = snakeModel.GetGridObjectType();
        //TODO Check non-existent mediator
        viewManager.GetMediator(modelID).ModelMovedSignal.Dispatch(snakeModel.Position);

        while (snakeModel.Next != null)
        {
            snakeModel = snakeModel.Next;
            if (!snakeModel.Position.Equals(snakePriorPosition))
            {
                GridPosition oldPosition = snakeModel.Position;
                gridManager.Grid.Map[snakeModel.Position.X, snakeModel.Position.Y] = GridObjectType.Empty;
                snakeModel.Position = snakePriorPosition;
                gridManager.Grid.Map[snakeModel.Position.X, snakeModel.Position.Y] = snakeModel.GetGridObjectType();
                viewManager.GetMediator(snakeModel.GetID()).ModelMovedSignal.Dispatch(snakeModel.Position);
                snakePriorPosition = oldPosition;
            }
            else
            {
                break;
            }
        }
    }

    void EatFood(GridPosition foodPosition)
    {
        injectionBinder.GetInstance<EatFoodSignal>().Dispatch(foodPosition, modelID);
    }
}
