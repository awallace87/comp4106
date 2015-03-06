using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

public class BoardSquareMediator : Mediator 
{
	[Inject]
	public BoardSquareView view { get; set; }

	public override void OnRegister ()
	{
		view.Initialize ();
	}

	//TODO Create Callbacks
}
