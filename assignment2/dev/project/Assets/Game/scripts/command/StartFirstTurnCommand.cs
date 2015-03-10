using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class StartFirstTurnCommand : Command
{
    public override void Execute()
    {
        IGameManager gameManager = injectionBinder.GetInstance<IGameManager>() as IGameManager;

        Debug.Log("3");

        gameManager.CurrentTurn = DiscColour.White;
    }
}
