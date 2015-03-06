using UnityEngine;
using System.Collections;

public interface IPlayer 
{
	DiscColour Colour {get; set;}
}

public enum PlayerType
{
	Human
	, Computer
}
