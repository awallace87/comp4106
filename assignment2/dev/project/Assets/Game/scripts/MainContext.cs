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
		mediationBinder.Bind<BoardSquareView> ().To<BoardSquareMediator> ();
		mediationBinder.Bind<DiscView> ().To<DiscMediator> ();
    }

    void mapInjectionBindings()
    {
		injectionBinder.Bind<IPlayer> ().To<HumanPlayer> ().ToName (PlayerType.Human);
		injectionBinder.Bind<IPlayer> ().To<AIPlayer> ().ToName (PlayerType.Computer);

		injectionBinder.Bind<IBoardModel> ().To<DefaultBoardModel> ();
		injectionBinder.Bind<IDiscModel> ().To<DefaultDiscModel> ();
		injectionBinder.Bind<IBoardSquareModel> ().To<DefaultBoardSquareModel> ();

		injectionBinder.Bind<IGameManager> ().To<DefaultGameManager> ().ToSingleton ();
		injectionBinder.Bind<IResourceNameManager> ().To<DefaultResourceNameManager> ().ToSingleton ();
    }

    void mapCommandBindings()
    {
        commandBinder.Bind<GameStartSignal>()
			.To<InitializeGameBoardCommand>()
			.To<CreateBoardViewCommand>()
			.To<AddInitialDiscsCommand>()
            .InSequence()
            .Once();

		commandBinder.Bind<CreateBoardSquareViewSignal> ().To<CreateBoardSquareViewCommand> ();
		commandBinder.Bind<CreateDiscSignal> ()
			.To<CreateDiscCommand> ()
			.To<CreateDiscViewCommand> ()
			.InSequence ();

		commandBinder.Bind<GameEndSignal> ();

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
