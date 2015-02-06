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
        GridPosition nextPosition = snakeModel.Position.GetPositionInDirection(snakeModel.Direction);

        if (!gridManager.IsValidPosition(nextPosition))
        {
            //Out of Bounds
            //TODO - Handle
            return;
        }
        //Check if a collision will occur
        GridObjectType destinationType = gridManager.Grid.Map[nextPosition.X, nextPosition.Y];

        switch (destinationType)
        {
            case GridObjectType.Food:
                {
                    //Eat Food
                    EatFood(nextPosition);
                    MoveSnake(nextPosition);
                }
                break;
            case GridObjectType.Empty:
                {
                    //Move
                    MoveSnake(nextPosition);
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
        //Debug.Log("Move Snake Position");
        gridManager.Grid.Map[snakeModel.Position.X, snakeModel.Position.Y] = GridObjectType.Empty;
        snakeModel.Position = position;

        gridManager.Grid.Map[snakeModel.Position.X, snakeModel.Position.Y] = snakeModel.GetGridObjectType();
        //TODO Check non-existent mediator
        viewManager.GetMediator(modelID).ModelMovedSignal.Dispatch(position);
    }

    void EatFood(GridPosition foodPosition)
    {
        injectionBinder.GetInstance<EatFoodSignal>().Dispatch(foodPosition, modelID);
    }
}
