using UnityEngine;
using System.Collections;

public class HumanPlayer : IPlayer 
{
	private DiscColour colour;

	#region IPlayer implementation
	
	public DiscColour Colour {
		get {
			return colour;
		}
		set {
			colour = value;
		}
	}

	#endregion
}
