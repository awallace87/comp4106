using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class DefaultDiscModel : IDiscModel 
{
	private DiscColour colour;
	private Signal<DiscColour> discFlippedSignal;

	public DefaultDiscModel()
	{
		discFlippedSignal = new Signal<DiscColour> ();
	}

	#region IDiscModel implementation

	public DiscColour Colour {
		get {
			return colour;
		}
		set {
			colour = value;
		}
	}

	public void Flip ()
	{
		if (colour == DiscColour.Black) {
			colour = DiscColour.White;
		}
		else {
			colour = DiscColour.Black;
		}
		OnDiscFlippedSignal.Dispatch (colour);
	}

	public Signal<DiscColour> OnDiscFlippedSignal {
		get {
			return discFlippedSignal;
		}
	}

	#endregion


}
