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
        Debug.Log("4");
        foreach (GridPosition position in affectedPositions)
        {
            Debug.Log("5");
            FlipDisc(position);
        }
    }

    private void FlipDisc(GridPosition flipPosition)
    {
        Debug.Log("2");
        gameManager.GetGameBoard().Board[flipPosition.X, flipPosition.Y].Disc.Flip();
    }
}
