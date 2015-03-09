using UnityEngine;
using System.Collections;

public class HumanPlayer : IPlayer 
{
    public PlayMethod GetPlayMethod()
    {
        return PlayMethod.UserInput;
    }
}
