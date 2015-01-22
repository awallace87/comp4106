using UnityEngine;
using System.Collections;
using strange.extensions.context.impl;


public class Root : ContextView
{
    void Awake()
    {
        context = new MainContext(this);    
    }
}
