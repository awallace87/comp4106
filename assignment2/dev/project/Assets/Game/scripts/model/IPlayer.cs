using UnityEngine;
using System.Collections;

public interface IPlayer 
{
    PlayMethod GetPlayMethod();
}

public enum PlayerType
{
	Human
	, ComputerScore
    , ComputerMobility
}

public enum PlayMethod
{
    UserInput,
    AlphaBetaScore,
    AlphaBetaMobility,
    MinimaxScore,
    MinimaxMobility
}
