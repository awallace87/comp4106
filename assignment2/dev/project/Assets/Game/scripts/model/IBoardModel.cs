using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBoardModel
{
	IBoardSquareModel[,] Board { get; set; }

	IList<GridPosition> GetLegalMoves(DiscColour player);
}
