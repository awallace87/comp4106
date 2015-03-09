using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IBoardModel
{
    IBoardSquareModel[,] Board { get; set; }
    uint BoardSize { get; }

    IList<GridPosition> GetLegalMoves(DiscColour player);
    IList<GridPosition> GetAffectedDiscPositions(DiscColour player, GridPosition playLocation);
}
