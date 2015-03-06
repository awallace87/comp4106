using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class AddInitialDiscsCommand : Command
{
	public override void Execute ()
	{
		Dictionary<GridPosition, DiscColour> initalPlacements = GetInitialDiscPlacements ();
		foreach (KeyValuePair<GridPosition,DiscColour> entry in initalPlacements) 
		{
			CreateDiscSignal signal = injectionBinder.GetInstance<CreateDiscSignal>() as CreateDiscSignal;
			signal.Dispatch(entry.Key, entry.Value);
		}
	}

	Dictionary<GridPosition, DiscColour> GetInitialDiscPlacements()
	{
		Dictionary<GridPosition,DiscColour> initalPlacements = new Dictionary<GridPosition, DiscColour> ();

		initalPlacements.Add (new GridPosition (3, 3), DiscColour.Black);
		initalPlacements.Add (new GridPosition (4, 4), DiscColour.Black);
		initalPlacements.Add (new GridPosition (3, 4), DiscColour.White);
		initalPlacements.Add (new GridPosition (4, 3), DiscColour.White);

		return initalPlacements;
	}
}
