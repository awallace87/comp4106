using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class FlipAffectedDiscsCommand : Command
{
    [Inject]
    public GridPosition playPosition { get; set; }

    [Inject]
    public DiscColour playColour { get; set; }

    [Inject]
    public IGameManager gameManager { get; set; }

    public override void Execute()
    {
        IList<GridPosition> affectedPositions = gameManager.GetGameBoard().GetAffectedDiscPositions(playColour, playPosition);
        foreach (GridPosition position in affectedPositions)
        {
            FlipDisc(position);
        }
    }

    private void FlipDisc(GridPosition flipPosition)
    {
        gameManager.GetGameBoard().Board[flipPosition.X, flipPosition.Y].Disc.Flip();
    }
}
