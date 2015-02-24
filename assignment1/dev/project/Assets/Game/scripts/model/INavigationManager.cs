using UnityEngine;
using System.Collections;

public interface INavigationManager 
{
	NavigationMethod GetNavigationMethod(uint snakeID);
	void AddSnake(uint snakeID, NavigationMethod method);

	NavigationMethod GetRandomNavigationMethod();
}

public enum NavigationMethod
{
	NavigationMethodStart = 0,
	DepthFirst = NavigationMethodStart,
	AStarEuclidean,
	AStarManhattan,
	AStarAverage,
	BreadthFirst,
	NavigationMethodEnd
}