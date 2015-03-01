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
        mapMediationBindings();
        mapInjectionBindings();
        mapCommandBindings();
    }

	void mapConfigurationBindings()
	{
	}

    void mapMediationBindings()
    {
    }

    void mapInjectionBindings()
    {

    }

    void mapCommandBindings()
    {
        commandBinder.Bind<GameStartSignal>()
            .InSequence()
            .Once();

    }

	public override IContext Start ()
	{
		base.Start ();
		GameStartSignal startSignal = (GameStartSignal)injectionBinder.GetInstance<GameStartSignal> ();
		startSignal.Dispatch ();
		return this;
	}

    public override void OnRemove()
    {
        EndGame();
        base.OnRemove();
    }

    public void PauseGame()
    {

    }

    public void EndGame() 
    {
        Debug.Log("OnRemove");
        GameEndSignal endSignal = (GameEndSignal)injectionBinder.GetInstance<GameEndSignal>();
        endSignal.Dispatch();
    }
}
