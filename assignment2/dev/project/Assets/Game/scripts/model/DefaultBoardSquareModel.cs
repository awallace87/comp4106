using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public class DefaultBoardSquareModel : IBoardSquareModel
{
	private uint mobilityValue;
	private IDiscModel disc;
	private bool containsDisc;
    private BoardSquareState state;
    private Signal<BoardSquareState> stateChangedSignal;

	public DefaultBoardSquareModel() 
	{
        this.mobilityValue = 0;
		this.containsDisc = false;
		this.disc = null;
        this.state = BoardSquareState.Default;
        this.stateChangedSignal = new Signal<BoardSquareState>();
	}

	#region IBoardSquareModel implementation
	public bool ContainsDisc ()
	{
		return this.containsDisc;
	}

	public uint MobilityScore {
		get {
			return this.mobilityValue;
		}
		set {
			this.mobilityValue = value;
		}
	}

	public IDiscModel Disc {
		get {
			return this.disc;
		}
		set {
			this.disc = value;
			this.containsDisc = true;
		}
	}

    public BoardSquareState State
    {
        get { return state; }
        set 
        {
            this.state = value;
            this.stateChangedSignal.Dispatch(state);
        }
    }

    public Signal<BoardSquareState> StateChangedSignal
    {
        get { return this.stateChangedSignal; }
    }

	#endregion


}
