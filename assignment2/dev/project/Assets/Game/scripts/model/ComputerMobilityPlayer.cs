using UnityEngine;
using System.Collections;

public class ComputerMobilityPlayer : IPlayer 
{
    public PlayMethod GetPlayMethod()
    {
        return PlayMethod.AlphaBetaMobility;
    }
}
