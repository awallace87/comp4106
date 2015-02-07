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
        mediationBinder.Bind<SnakeView>().To<SnakeMediator>();
        mediationBinder.Bind<FoodView>().To<FoodMediator>();
        mediationBinder.Bind<WallView>().To<WallMediator>();
    }

    void mapInjectionBindings()
    {
        injectionBinder.Bind<IGridManager>().To<DefaultGridManager>().ToSingleton();
        injectionBinder.Bind<IViewManager>().To<DefaultViewManager>().ToSingleton();
        injectionBinder.Bind<IUpdateManager>().To<DefaultUpdateManager>().ToSingleton();
        
        injectionBinder.Bind<IGrid>().To<DefaultGrid>();

        injectionBinder.Bind<ISnakeModel>().To<DefaultSnakeModel>().ToName(SnakeBindings.Head);
        injectionBinder.Bind<ISnakeModel>().To<SnakeTailModel>().ToName(SnakeBindings.Tail);
        injectionBinder.Bind<IFoodModel>().To<DefaultFoodModel>();
        injectionBinder.Bind<IWallModel>().To<DefaultWallModel>();
    }

    void mapCommandBindings()
    {
        commandBinder.Bind<GameStartSignal>()
            .InSequence()
            .To<GameStartCommand>()
            .To<AddInitialWallsCommand>()
            .To<AddFoodCommand>()
            .To<CreateSnakeCommand>()
            .Once();

        commandBinder.Bind<GameEndSignal>().To<GameEndCommand>().Once();

        commandBinder.Bind<CreateSnakeSignal>().To<CreateSnakeCommand>();
        commandBinder.Bind<MoveAllSnakesSignal>().To<MoveAllSnakesCommand>();
        commandBinder.Bind<MoveSnakeSignal>().To<MoveSnakeCommand>();
        commandBinder.Bind<UpdateSnakeDirectionSignal>().To<UpdateSnakeDirectionCommand>();
        commandBinder.Bind<UpdateAllSnakesSignal>().To<UpdateAllSnakesCommand>();

        commandBinder.Bind<IncrementSnakeTailSignal>().To<IncrementSnakeTailCommand>();

        commandBinder.Bind<EatFoodSignal>()
            .InSequence()
            .To<EatFoodCommand>()
            .To<AddFoodCommand>();

        commandBinder.Bind<AddWallSignal>().To<AddWallCommand>();

        commandBinder.Bind<RemoveGridObjectSignal>().To<RemoveGridObjectCommand>();
        //commandBinder.Bind<GridObjectMovedSignal>().To<CheckCollisionCommand>();
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

    public void AddSnake()
    {
        Debug.Log("AddSnake");
        CreateSnakeSignal createSnakeSignal = injectionBinder.GetInstance<CreateSnakeSignal>();
        createSnakeSignal.Dispatch();
    }
}
