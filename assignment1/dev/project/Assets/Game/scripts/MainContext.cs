using UnityEngine;
using System.Collections;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

public class MainContext : SignalContext
{
    public MainContext(MonoBehaviour view)
        : base(view)
    {
    }

    protected override void mapBindings()
    {
        Debug.Log("MapBindings Begin");

    }
}
