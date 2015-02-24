using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using System.Collections.Generic;
using System;

public class Root : ContextView
{
    public static readonly Queue<Action> RootMainThreadActions = new Queue<Action>();

    void Awake()
    {
        context = new MainContext(this);
    }

    //Done to call Shutdown in Editor
    void OnApplicationQuit()
    {
        MainContext mainContext = context as MainContext;
        mainContext.EndGame();
    }

    void Update()
    {
        if (RootMainThreadActions.Count > 0)
        {
            RootMainThreadActions.Dequeue().Invoke();
        }

		if (Input.GetKeyDown(KeyCode.A))
		{
			//Add Snake
			MainContext mainContext = context as MainContext;
			mainContext.AddSnake(NavigationMethod.AStarAverage);
		}

        if (Input.GetKeyDown(KeyCode.D))
        {
            //Add Snake
            MainContext mainContext = context as MainContext;
            mainContext.AddSnake(NavigationMethod.DepthFirst);
        }

		if (Input.GetKeyDown(KeyCode.B))
		{
			//Add Snake
			MainContext mainContext = context as MainContext;
			mainContext.AddSnake(NavigationMethod.BreadthFirst);
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			//Add Snake
			MainContext mainContext = context as MainContext;
			mainContext.AddSnake(NavigationMethod.AStarEuclidean);
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			//Add Snake
			MainContext mainContext = context as MainContext;
			mainContext.AddSnake(NavigationMethod.AStarManhattan);
		}

		if (Input.GetKeyDown (KeyCode.W))
		{
			MainContext mainContext = context as MainContext;
			mainContext.AddObstacle();
		}


    }
    

}
