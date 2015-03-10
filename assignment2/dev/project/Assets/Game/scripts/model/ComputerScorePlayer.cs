using UnityEngine;
using System.Collections;

public class ComputerScorePlayer : IPlayer
{
    public PlayMethod GetPlayMethod()
    {
        return PlayMethod.AlphaBetaScore;
    }
}
