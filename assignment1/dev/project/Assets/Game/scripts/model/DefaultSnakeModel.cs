using UnityEngine;
using System.Collections;

public class DefaultSnakeModel : ISnakeModel{

	private GridPosition position;
	private uint id;
	
	private GridObjectMovedSignal moveSignal;

	[Inject]
	public IGridManager manager { get; set; }

	public DefaultSnakeModel()
	{
		this.moveSignal = new GridObjectMovedSignal ();
	}

	//TODO - Switch to Constructor Injection
	[PostConstruct]
	void initialize() {
		id = manager.GetNextGridObjectID ();
	}

	#region IGridObject
	public GridPosition Position {
		get {
			return position;
		}
		set {
			position = value;
			moveSignal.Dispatch (position);
		}
	}

	public GridObjectMovedSignal MoveSignal {
		get {
			return moveSignal;
		}
	}
	
	public uint GetID ()
	{
		return id;
	}

	#endregion

	#region ISnakeModel implementation

	public void Move ()
	{
		moveSignal.Dispatch (position);
	}

	#endregion
}
