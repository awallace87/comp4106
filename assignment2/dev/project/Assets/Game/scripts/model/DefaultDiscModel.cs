using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class DefaultDiscModel : IDiscModel 
{
	private DiscColour colour;
	private Signal<DiscColour> discFlippedSignal;

	public DefaultDiscModel()
	{
		this.discFlippedSignal = new Signal<DiscColour> ();
	}

	#region IDiscModel implementation

	public DiscColour Colour {
		get {
			return this.colour;
		}
		set {
            this.colour = value;
		}
	}

	public void Flip ()
	{
        if (this.colour == DiscColour.Black)
        {
            this.colour = DiscColour.White;
		}
		else {
            this.colour = DiscColour.Black;
		}
        Debug.Log("1");
        this.discFlippedSignal.Dispatch(colour);
	}

	public Signal<DiscColour> OnDiscFlippedSignal {
		get {
			return discFlippedSignal;
		}
	}

	#endregion


}
