using UnityEngine;
using System.Collections;

public interface IGameManager 
{
    IBoardModel GetGameBoard();

    DiscColour CurrentTurn { get; set; }

	IPlayer WhitePlayer{ get; set; }
	IPlayer BlackPlayer{ get; set; }
}
