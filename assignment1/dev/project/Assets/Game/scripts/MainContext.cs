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

		mediationBinder.Bind<SnakeView> ().To<SnakeMediator> ();

		injectionBinder.Bind<IGridManager> ().To<DefaultGridManager> ().ToSingleton ();
		injectionBinder.Bind<IGrid> ().To<DefaultGrid> ();

		injectionBinder.Bind<ISnakeModel> ().To<DefaultSnakeModel> ();

		commandBinder.Bind<GameStartSignal> ().To<GameStartCommand> ().To<CreateSnakeCommand>().InSequence().Once ();
    }

	void mapConfigurationBindings()
	{
	}

	public override IContext Start ()
	{
		base.Start ();
		GameStartSignal startSignal = (GameStartSignal)injectionBinder.GetInstance<GameStartSignal> ();
		startSignal.Dispatch ();
		return this;
	}
}
