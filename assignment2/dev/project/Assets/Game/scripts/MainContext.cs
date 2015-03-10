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
		injectionBinder.Bind<IPlayer> ().To<ComputerScorePlayer> ().ToName (PlayerType.ComputerScore);
        injectionBinder.Bind<IPlayer>().To<ComputerMobilityPlayer>().ToName(PlayerType.ComputerMobility);

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
            .To<InitializePlayersCommand>()
            .InSequence()
            .Once();

		commandBinder.Bind<CreateBoardSquareViewSignal> ().To<CreateBoardSquareViewCommand> ();
		commandBinder.Bind<CreateDiscSignal> ()
			.To<CreateDiscCommand> ()
			.To<CreateDiscViewCommand> ()
			.InSequence ();

        commandBinder.Bind<ContinueGameSignal>().To<StartFirstTurnCommand>().Once();
        commandBinder.Bind<ContinueGameSignal>();

        commandBinder.Bind<GameOverSignal>().To<GameOverCommand>();

        commandBinder.Bind<StartTurnSignal>().To<StartTurnCommand>();
        commandBinder.Bind<BoardSquarePressedSignal>().To<BoardSquareInputCommand>();
        commandBinder.Bind<BoardSquareSelectedSignal>().To<SelectBoardSquareCommand>();
        
        commandBinder.Bind<PlayTurnSignal>()
            .To<FlipAffectedDiscsCommand>()
            .To<CreateDiscCommand>()
            .To<CreateDiscViewCommand>()
            .To<EndTurnCommand>()
            .InSequence();

        commandBinder.Bind<SkipTurnSignal>()
            .To<EndTurnCommand>();

        commandBinder.Bind<MakeUserInputMoveSignal>()
            .To<EnableUserInputCommand>()
            .To<UpdateBoardForTurnCommand>()
            .To<MakeUserInputMoveCommand>()
            .InSequence();

        commandBinder.Bind<MakeAIMoveSignal>()
            .To<DisableUserInputCommand>()
            .To<UpdateBoardForTurnCommand>()
            .To<MakeAIMoveCommand>()
            .InSequence();

		commandBinder.Bind<GameEndSignal> ();

        commandBinder.Bind<OnMediatorRegisteredSignal>();

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

    public void BeginGame()
    {
        Debug.Log("2");
        ContinueGameSignal firstTurnSignal = injectionBinder.GetInstance<ContinueGameSignal>() as ContinueGameSignal;
        firstTurnSignal.Dispatch();
    }

    public void EndGame() 
    {
        Debug.Log("OnRemove");
        GameEndSignal endSignal = (GameEndSignal)injectionBinder.GetInstance<GameEndSignal>();
        endSignal.Dispatch();
    }
}
