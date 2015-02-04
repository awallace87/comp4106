using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class GameEndCommand : Command
{
    [Inject]
    public IUpdateManager updateManager { get; set; }

    public override void Execute()
    {
        Debug.Log("GameEndCommand::Execute");
        updateManager.Shutdown();
    }
}
