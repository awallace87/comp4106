using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;


public class Root : ContextView
{
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

    
}
