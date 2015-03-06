using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class CreateDiscCommand : Command 
{
	[Inject]
	public GridPosition discPosition { get; set; }

	[Inject]
	public DiscColour discColour { get; set; }

	public override void Execute()
	{
		IGameManager gameManager = injectionBinder.GetInstance<IGameManager> () as IGameManager;

		IDiscModel disc = injectionBinder.GetInstance<IDiscModel> () as IDiscModel;
		disc.Colour = discColour;

		gameManager.GetGameBoard ().Board [discPosition.X, discPosition.Y].Disc = disc;
	}
}
