using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;
using System.Collections.Generic;
using System;

public class Root : ContextView
{
    public static readonly Queue<Action> RootMainThreadActions = new Queue<Action>();

    public bool gameStarted = false;
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

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("1");
            MainContext mainContext = context as MainContext;
            mainContext.BeginGame();
        }

    }

    

}
