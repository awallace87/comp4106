using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class InitializePlayersCommand : Command
{
    public override void Execute()
    {
        IGameManager gameManager = injectionBinder.GetInstance<IGameManager>() as IGameManager;

        gameManager.WhitePlayer = injectionBinder.GetInstance<IPlayer>(PlayerType.ComputerMobility) as IPlayer;
        gameManager.BlackPlayer = injectionBinder.GetInstance<IPlayer>(PlayerType.ComputerScore) as IPlayer;

        //gameManager.CurrentTurn = DiscColour.White;
    }
}
