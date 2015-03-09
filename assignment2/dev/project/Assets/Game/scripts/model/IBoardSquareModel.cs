using UnityEngine;
using System.Collections;
using strange.extensions.signal.impl;

public interface IBoardSquareModel
{
    uint MobilityScore { get; set; }

    BoardSquareState State { get; set; }
    Signal<BoardSquareState> StateChangedSignal { get; }

	bool ContainsDisc();
	IDiscModel Disc { get; set; }
}

public enum BoardSquareState
{
    Default,
    Available,
    Selected,
    Affected
}
