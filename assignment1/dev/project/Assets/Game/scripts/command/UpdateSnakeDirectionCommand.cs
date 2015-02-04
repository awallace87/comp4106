using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class UpdateSnakeDirectionCommand : Command
{
    [Inject]
    public uint snakeModelID { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

    bool[,] isVisited;
    uint[,] distance;

    public override void Execute()
    {
        ISnakeModel snakeModel = gridManager.GetObjectByID(snakeModelID) as ISnakeModel;
        GridPosition foodPosition;

        List<uint> foodIDs = gridManager.GetIDsOfType(GridObjectType.Food);
        if (foodIDs.Count < 1)
        {
            return;
        }

        foodPosition = gridManager.GetObjectByID(foodIDs[0]).Position;

        InitializeVisited();
        InitializeDistance();
        int StartTime = System.DateTime.UtcNow.Millisecond;
        Debug.Log("Search Started");

        Stack dfsStack = new Stack();
        isVisited[foodPosition.X, foodPosition.Y] = true;
        distance[foodPosition.X, foodPosition.Y] = 0;
        dfsStack.Push(foodPosition);

        while (dfsStack.Count > 0)
        {
            GridPosition positionPushed = dfsStack.Pop() as GridPosition;
            //Debug.Log("Checking Position - " + positionPushed.X.ToString() + "," + positionPushed.Y.ToString());
            if (positionPushed.Equals(snakeModel.Position))
            {
                Debug.Log("Snake Found");
                break;
            }
            List<GridPosition> adjacentPositions = getAdjacentPositions(positionPushed);
            foreach (GridPosition adjacent in adjacentPositions)
            {
                if (!isVisited[adjacent.X, adjacent.Y])
                {
                    isVisited[adjacent.X, adjacent.Y] = true;
                    if (gridManager.Grid.Map[adjacent.X, adjacent.Y] == GridObjectType.Empty)
                    {
                        distance[adjacent.X, adjacent.Y] = distance[positionPushed.X, positionPushed.Y] + 1;
                    }
                    dfsStack.Push(adjacent);
                }
            }
        }
        GridDirection bestSnakeDirection = GetBestDirection(snakeModel.Position);
        Debug.Log("BestDirection For Snake - " + bestSnakeDirection.ToString());
        snakeModel.Direction = bestSnakeDirection;
        int totalTime = System.DateTime.UtcNow.Millisecond - StartTime;
        Debug.Log("Time Completed in - " + totalTime.ToString());
    }

    GridDirection GetBestDirection(GridPosition position)
    {
        uint minDistance = uint.MaxValue;
        GridDirection minDirection = GridDirection.Invalid;
        for (int i = (int)GridDirection.ValidDirectionStart; i <= (int)GridDirection.ValidDirectionEnd; i++)
        {
            GridPosition directPosition = position.GetPositionInDirection((GridDirection)i);
            uint positionDistance = distance[directPosition.X, directPosition.Y];
            if (positionDistance < minDistance)
            {
                minDistance = positionDistance;
                minDirection = (GridDirection)i;
            }
        }
        return minDirection;
    }

    List<GridPosition> getAdjacentPositions(GridPosition position)
    {
        List<GridPosition> adjacentPositions = new List<GridPosition>();
        for (int i = (int)GridDirection.ValidDirectionStart; i <= (int)GridDirection.ValidDirectionEnd; i++)
        {
            //Debug.Log("adjacent position loop - " + i.ToString());
            GridPosition directionPosition = position.GetPositionInDirection((GridDirection)i);
            if (gridManager.IsValidPosition(directionPosition))
            {
                adjacentPositions.Add(directionPosition);
            }
        }
        //Debug.Log("Adjacent Positions Count - " + adjacentPositions.Count.ToString());
        return adjacentPositions;
    }

    bool isValidPosition(GridPosition position) 
    {
        if(gridManager.IsValidPosition(position)) {
           // bool isSpaceEmpty = gridManager.Grid.Map[position.X, position.Y] == GridObjectType.Empty;
            bool isSpaceVisited = isVisited[position.X, position.Y];
            return !isSpaceVisited;
        }
        else
        {
            return false;
        }
    }

    void InitializeVisited()
    {
        isVisited = new bool[gridManager.Grid.Width, gridManager.Grid.Height];
        for (int i = 0; i < gridManager.Grid.Width; i++)
        {
            for (int j = 0; j < gridManager.Grid.Height; j++)
            {
                isVisited[i, j] = false;
            }
        }
    }

    void InitializeDistance()
    {
        distance = new uint[gridManager.Grid.Width, gridManager.Grid.Height];
        for (int i = 0; i < gridManager.Grid.Width; i++)
        {
            for (int j = 0; j < gridManager.Grid.Height; j++)
            {
                distance[i, j] = uint.MaxValue;
            }
        }
    }
}
