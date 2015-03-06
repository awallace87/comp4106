using UnityEngine;
using System.Collections;

public class DefaultBoardSquareModel : IBoardSquareModel
{
	private uint mobilityValue;
	private IDiscModel disc;
	private bool containsDisc;

	public DefaultBoardSquareModel() 
	{
		this.containsDisc = false;
		this.disc = null;
	}

	#region IBoardSquareModel implementation
	public bool ContainsDisc ()
	{
		return containsDisc;
	}

	public uint MobilityScore {
		get {
			return mobilityValue;
		}
		set {
			mobilityValue = value;
		}
	}

	public IDiscModel Disc {
		get {
			return disc;
		}
		set {
			disc = value;
			containsDisc = true;
		}
	}
	#endregion
}
