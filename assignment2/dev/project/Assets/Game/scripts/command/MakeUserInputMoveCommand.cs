using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;

public class MakeUserInputMoveCommand : Command
{
    [Inject]
    public DiscColour turnToPlay { get; set; }

    public override void Execute()
    {
        //Set States of BoardSquare to allow moves
    }

}
