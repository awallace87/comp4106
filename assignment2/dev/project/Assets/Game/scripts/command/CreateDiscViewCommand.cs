using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System;

public class CreateDiscViewCommand : Command 
{
	[Inject]
	public GridPosition discPosition { get; set; }

	public override void Execute ()
	{
		IResourceNameManager resourceNameManager = injectionBinder.GetInstance<IResourceNameManager> () as IResourceNameManager;
		
		//Create Board Object
		Action createBoardObjectAction = () =>
		{
			GameObject discObject = (GameObject) GameObject.Instantiate(Resources.Load(resourceNameManager.GetResourceName(ResourceID.OthelloDiscPrefab)));
			DiscView discView = discObject.AddComponent<DiscView>();
			discView.discPosition = discPosition;
			
			discObject.transform.parent = GameObject.Find(resourceNameManager.GetResourceName(ResourceID.BoardGO)).transform;
		};
		
		Root.RootMainThreadActions.Enqueue (createBoardObjectAction);

	}
}
