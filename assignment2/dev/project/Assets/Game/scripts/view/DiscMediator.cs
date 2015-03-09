using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class DiscMediator : Mediator 
{
	[Inject]
	public DiscView view { get; set; }

	[Inject]
	public IGameManager gameManager { get; set; }

	public override void OnRegister ()
	{
		DiscColour initalDiscColour = gameManager.GetGameBoard ().Board [view.discPosition.X, view.discPosition.Y].Disc.Colour;
        gameManager.GetGameBoard().Board[view.discPosition.X, view.discPosition.Y]
            .Disc.OnDiscFlippedSignal.AddListener(OnDiscModelFlipped);
		view.Initialize (initalDiscColour);
	}

    private void OnDiscModelFlipped(DiscColour colour)
    {
        Debug.Log("Disc Model Flipped");
        view.DiscFlippedSignal.Dispatch(colour);
    }
}
