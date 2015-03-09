using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class MakeAIMoveCommand : Command 
{
    [Inject]
    public DiscColour turnToPlay { get; set; }

    public override void Execute()
    {

    }
}