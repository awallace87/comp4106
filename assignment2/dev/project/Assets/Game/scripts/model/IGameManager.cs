using UnityEngine;
using System.Collections;

public interface IGameManager 
{
	IBoardModel GetGameBoard();

	IPlayer WhitePlayer{ get; set; }
	IPlayer BlackPlayer{ get; set; }
}
