using UnityEngine;
using System.Collections;

public class AIPlayer : IPlayer
{
    public PlayMethod GetPlayMethod()
    {
        return PlayMethod.MinimaxSearch;
    }
}
