using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public interface IDiscModel 
{
	DiscColour Colour { get; set; }
	void Flip();
	Signal<DiscColour> OnDiscFlippedSignal { get; }
}

public enum DiscColour
{
	Black
	, White
}
