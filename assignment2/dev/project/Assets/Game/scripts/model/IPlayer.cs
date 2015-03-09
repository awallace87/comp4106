using UnityEngine;
using System.Collections;

public interface IPlayer 
{
    PlayMethod GetPlayMethod();
}

public enum PlayerType
{
	Human
	, Computer
}

public enum PlayMethod
{
    UserInput,
    MinimaxSearch
}
