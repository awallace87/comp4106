using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class AddFoodCommand : Command
{
    [Inject]
    public IGridManager gridManager { get; set; }

    public override void Execute()
    {
        Debug.Log("AddFoodCommand::Execute");

        IFoodModel food = injectionBinder.GetInstance<IFoodModel>();
        food.Position = GetFoodStartingPosition();
        gridManager.AddGridObject(food);
        //gridManager.Grid.Map[food.Position.X, food.Position.Y] = GridObjectType.Food;

        Action createFoodAction = () =>
        {
            GameObject foodObject = new GameObject("food");
            FoodView foodView = foodObject.AddComponent<FoodView>();
            foodView.ModelID = food.GetID();

        };

        Root.RootMainThreadActions.Enqueue(createFoodAction);
    }

    GridPosition GetFoodStartingPosition()
    {
        bool foundUnoccupiedPosition = false;
        System.Random random = new System.Random();
        GridPosition startPosition = new GridPosition(gridManager.Grid.Width / 2, gridManager.Grid.Height / 2);
        while (!foundUnoccupiedPosition)
        {
            int minX, maxX, minY, maxY;
            minX = 0;
            maxX = (int)gridManager.Grid.Width;
            minY = 0;
            maxY = (int)gridManager.Grid.Height;
            uint xPosition = (uint)random.Next(minX, maxX);
            uint yPosition = (uint)random.Next(minY, maxY);

            startPosition = new GridPosition(xPosition, yPosition);

            foundUnoccupiedPosition = gridManager.Grid.Map[xPosition, yPosition] == GridObjectType.Empty;
        }
        return startPosition;
    }
}
