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
    }
    

}
