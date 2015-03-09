using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class DiscView : View 
{
	private const float BlackXRotation = 0.0f;
	private const float WhiteXRotation = 180.0f;

    public GridPosition discPosition { get; set; }

    private Signal<DiscColour> discFlippedSignal;
    public Signal<DiscColour> DiscFlippedSignal 
    {
        get { return discFlippedSignal; }
    }


	internal void Initialize(DiscColour initialColour)
	{
		//Adjust Position
		Vector3 localPosition = transform.localPosition;
		localPosition.x = discPosition.X;
		localPosition.y = discPosition.Y;
		transform.localPosition = localPosition;

		//Set Rotation
		if (initialColour == DiscColour.White) {
			Vector3 initalRotation = new Vector3(WhiteXRotation, 0.0f, 0.0f);
			transform.Rotate(initalRotation);
		}

        discFlippedSignal = new Signal<DiscColour>();
        discFlippedSignal.AddListener(OnDiscFlipped);
	}

    private void OnDiscFlipped(DiscColour colour)
    {
        Vector3 initalRotation = new Vector3(WhiteXRotation, 0.0f, 0.0f);
        transform.Rotate(initalRotation);
    }


}
