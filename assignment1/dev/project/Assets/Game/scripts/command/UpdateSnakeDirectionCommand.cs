using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;
using System;

public class UpdateSnakeDirectionCommand : Command
{
    [Inject]
    public uint snakeModelID { get; set; }

    [Inject]
    public IGridManager gridManager { get; set; }

	[Inject]
	public INavigationManager navManager { get; set; }

    bool[,] isVisited;
    uint[,] distance;

	delegate uint distanceHeuristic(GridPosition a, GridPosition b);
	delegate uint differenceHeuristic(GridPosition a, GridPosition b);
	delegate uint averageHeuristic(GridPosition a, GridPosition b);

    public override void Execute()
    {

        List<uint> foodIDs = gridManager.GetIDsOfType(GridObjectType.Food);
        if (foodIDs.Count < 1)
        {
            Debug.Log("No Food");
            return;
        }
        ISnakeModel snakeModel = gridManager.GetGridObject(snakeModelID) as ISnakeModel;
        IGridObject foodModel = gridManager.GetGridObject(foodIDs[0]);



        InitializeVisited();
        int StartTime = System.DateTime.UtcNow.Millisecond;
        Debug.Log("Search Started");

		NavigationMethod method = navManager.GetNavigationMethod (snakeModelID);

		GridPosition nextPosition;

		switch (method) 
		{
			case NavigationMethod.BreadthFirst:
				{
					nextPosition = BreadthFirstSearch (snakeModel.Position);
				}
				break;
			case NavigationMethod.DepthFirst:
				{
					nextPosition = DepthFirstSearch (foodModel.Position, snakeModel.Position);
				}
				break;
			case NavigationMethod.AStarEuclidean:
				{
					nextPosition = AStarSearch (snakeModel.Position, foodModel.Position, DistanceHeuristic);
				}
				break;
			case NavigationMethod.AStarManhattan:
				{
					nextPosition = AStarSearch (snakeModel.Position, foodModel.Position, DifferenceHeuristic);
				}
				break;
			case NavigationMethod.AStarAverage:
				{
					nextPosition = AStarSearch( snakeModel.Position, foodModel.Position, AverageHeuristic);
				}
				break;
			default:
				{
					nextPosition = BreadthFirstSearch (snakeModel.Position);
				}
				break;
		}

		snakeModel.NextPosition = nextPosition;

        int totalTime = System.DateTime.UtcNow.Millisecond - StartTime;
        Debug.Log("Time Completed in - " + totalTime.ToString());

    }

	void initializeDelegates() {
		//distanceHeuristic = new distanceHeuristic (DistanceHeuristic);
		//differenceHeuristic = new differenceHeuristic (DifferenceHeuristic);
		//averageHeuristic = new averageHeuristic (AverageHeuristic);
	}

    GridPosition BreadthFirstSearch(GridPosition snakePosition)
    {
        GridPosition nextPosition = snakePosition;
        Queue<SearchNode> bfsFringe = new Queue<SearchNode>();
        SearchNode startNode = new SearchNode(snakePosition);
        isVisited[snakePosition.X, snakePosition.Y] = true;
        bfsFringe.Enqueue(startNode);// Push(startNode);

        while (bfsFringe.Count > 0)
        {
            SearchNode currentNode = bfsFringe.Dequeue();//Pop();
            //Debug.Log("Checking Position - " + positionPushed.X.ToString() + "," + positionPushed.Y.ToString());
            if (gridManager.Grid.Map[currentNode.Position.X, currentNode.Position.Y] == GridObjectType.Food)
            {
                //Debug.Log("Food Found at - " + currentNode.Position.X + "," + currentNode.Position.Y);
                while (currentNode.Predecessor.Predecessor != null)
                {
                    currentNode = currentNode.Predecessor;
                }
                if (currentNode.Predecessor.Position.Equals(snakePosition))
                {
                    //Found Snake
                    //Debug.Log("Backtrack Success");
                    //Debug.Log("Current Position - " + snakeModel.Position.ToString());
                    //Debug.Log("Next Position - " + currentNode.Position.ToString());
                    nextPosition = currentNode.Position; //.GetDirectionOfPosition(snakeModel.Position);
                }
                break;
            }
            List<GridPosition> adjacentPositions = getAdjacentPositions(currentNode.Position);
            foreach (GridPosition adjacent in adjacentPositions)
            {
                if (!isVisited[adjacent.X, adjacent.Y])
                {
                    isVisited[adjacent.X, adjacent.Y] = true;
                    SearchNode adjacentNode = new SearchNode(adjacent);
                    adjacentNode.Predecessor = currentNode;
                    bfsFringe.Enqueue(adjacentNode);// Push(adjacentNode);
                }
            }
        }
        return nextPosition;
    }

    GridPosition DepthFirstSearch(GridPosition foodPosition, GridPosition snakePosition)
    {
        GridPosition nextPosition = snakePosition;

        Stack<SearchNode> dfsFringe = new Stack<SearchNode>();
        SearchNode startNode = new SearchNode(foodPosition);
        isVisited[foodPosition.X, foodPosition.Y] = true;
        dfsFringe.Push(startNode);

        while (dfsFringe.Count > 0)
        {
            SearchNode currentNode = dfsFringe.Pop();
            //Debug.Log("Checking Position - " + positionPushed.X.ToString() + "," + positionPushed.Y.ToString());
            if (currentNode.Position.Equals(snakePosition))
            {
                //Debug.Log("Snake Found at - " + currentNode.Position.X + "," + currentNode.Position.Y);

                if (currentNode.Predecessor != null)
                {
                    //Found Snake
                    //Debug.Log("Backtrack Success");
                    //Debug.Log("Next Position - " + currentNode.Predecessor.Position);
                    nextPosition = currentNode.Predecessor.Position;
                }
                break;
            }
            List<GridPosition> adjacentPositions = getAdjacentPositions(currentNode.Position);
            foreach (GridPosition adjacent in adjacentPositions)
            {
                if (!isVisited[adjacent.X, adjacent.Y])
                {
                    isVisited[adjacent.X, adjacent.Y] = true;
                    SearchNode adjacentNode = new SearchNode(adjacent);
                    adjacentNode.Predecessor = currentNode;
                    dfsFringe.Push(adjacentNode);
                }
            }
        }
        return nextPosition;
    }

	GridPosition AStarSearch(GridPosition snakePosition, GridPosition foodPosition, Func<GridPosition,GridPosition,uint> heuristic)
    {
        GridPosition nextPosition = snakePosition;

        List<GridPosition> closedSet = new List<GridPosition>();
        List<GridPosition> openSet = new List<GridPosition>();


        Dictionary<GridPosition, GridPosition> predecessor = new Dictionary<GridPosition,GridPosition>();
        Dictionary<GridPosition, uint> gScore = new Dictionary<GridPosition, uint>();
        Dictionary<GridPosition, uint> fScore = new Dictionary<GridPosition,uint>();
        
        openSet.Add(snakePosition);
        gScore[snakePosition] = 0;
        fScore[snakePosition] = gScore[snakePosition] + heuristic(snakePosition, foodPosition);
        GridPosition current = snakePosition ;
        while(openSet.Count > 0)
        {
            openSet.Sort(delegate(GridPosition a, GridPosition b)
            {
                if (a == null && b == null) return 0;
                else if (a == null) return -1;
                else if (b == null) return 1;
                else return (fScore[a]).CompareTo(fScore[b]);
            });
            current = openSet[0];

            if (current.Equals(foodPosition))
            {
                //Debug.Log("Found Food");
                break;
            }

            openSet.Remove(current);
            closedSet.Add(current);
            foreach (GridPosition adjacent in getAdjacentPositions(current))
            {
                if (!closedSet.Contains(adjacent))
                {
                    uint tempGScore = gScore[current] + 1;
                    if (!openSet.Contains(adjacent) || tempGScore < (gScore.ContainsKey(adjacent) ? gScore[adjacent] : uint.MaxValue))
                    {
                        predecessor[adjacent] = current;
                        gScore[adjacent] = tempGScore;
                        fScore[adjacent] = gScore[adjacent] + heuristic(adjacent, foodPosition);
                        if (!openSet.Contains(adjacent))
                        {
                            openSet.Add(adjacent);
                        }
                    }
                } 
            }
        }

        while (predecessor.ContainsKey(current))
        {
            nextPosition = current;
            current = predecessor[current];
        }
        return nextPosition;
    }

    public uint DistanceHeuristic(GridPosition a, GridPosition b)
    {
        return (uint)Mathf.Sqrt(Mathf.Pow((a.X - b.X), 2) + Mathf.Pow((a.Y - b.Y), 2));
    }

	public uint DifferenceHeuristic(GridPosition a, GridPosition b)
	{
		return (uint)Mathf.Abs (((int)a.X - (int)b.X) + ((int)a.Y - (int)b.Y));
	}

	public uint AverageHeuristic(GridPosition a, GridPosition b)
	{
		return (DistanceHeuristic (a, b) + DifferenceHeuristic (a, b) / 2);
	}


    List<GridPosition> getAdjacentPositions(GridPosition position)
    {
        List<GridPosition> adjacentPositions = new List<GridPosition>();
        for (int i = (int)GridDirection.ValidDirectionStart; i <= (int)GridDirection.ValidDirectionEnd; i++)
        {
            GridPosition directionPosition = position.GetPositionInDirection((GridDirection)i);
            if (isValidPosition(directionPosition))
            {
                adjacentPositions.Add(directionPosition);
            }
        }
        //Shuffle List (Stack Overflow)
        System.Random random = new System.Random();
        int n = adjacentPositions.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            GridPosition value = adjacentPositions[k];
            adjacentPositions[k] = adjacentPositions[n];
            adjacentPositions[n] = value;
        }
        return adjacentPositions;
    }

    bool isValidPosition(GridPosition position) 
    {
        if(gridManager.IsValidPosition(position)) {
           // bool isSpaceEmpty = gridManager.Grid.Map[position.X, position.Y] == GridObjectType.Empty;
            GridObjectType positionType = gridManager.Grid.Map[position.X, position.Y];
            bool isMovableSpace = positionType == GridObjectType.Empty || positionType == GridObjectType.Food || positionType == GridObjectType.SnakeHead;
            return isMovableSpace;
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
        isVisited = new bool[gridManager.Grid.Width, gridManager.Grid.Height];
        for (int i = 0; i < gridManager.Grid.Width; i++)
        {
            for (int j = 0; j < gridManager.Grid.Height; j++)
            {
                isVisited[i, j] = false;
            }
        }
    }


    private class SearchNode
    {
        public GridPosition Position;
        public SearchNode Predecessor = null;

        public SearchNode(GridPosition position)
        {
            this.Position = position;
        }
    }
}
