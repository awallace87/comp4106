using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class AddFoodCommand : Command
{
    [Inject]
    public IGridManager gridManager { get; set; }

    public override void Execute()
    {
        Debug.Log("AddFoodCommand::Execute");

        IFoodModel food = injectionBinder.GetInstance<IFoodModel>();
        food.Position = GetFoodStartingPosition();

        GameObject foodObject = new GameObject("food");
        FoodView foodView = foodObject.AddComponent<FoodView>();

        //Wire Both
        foodView.ModelID = food.GetID();        
    }

    GridPosition GetFoodStartingPosition()
    {
        bool foundUnoccupiedPosition = false;
        GridPosition startPosition = new GridPosition(gridManager.Grid.Width / 2, gridManager.Grid.Height / 2);
        while (!foundUnoccupiedPosition)
        {
            uint minX, maxX, minY, maxY;
            minX = 0;
            maxX = gridManager.Grid.Width;
            minY = 0;
            maxY = gridManager.Grid.Height;
            uint xPosition = (uint)Random.Range(minX, maxX);
            uint yPosition = (uint)Random.Range(minY, maxY);

            startPosition = new GridPosition(xPosition, yPosition);

            foundUnoccupiedPosition = gridManager.Grid.Map[xPosition, yPosition] == GridObjectType.Empty;
        }
        return startPosition;
    }
}
