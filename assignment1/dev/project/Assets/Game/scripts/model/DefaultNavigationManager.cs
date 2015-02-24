using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DefaultNavigationManager : INavigationManager 
{
	private Dictionary<uint,NavigationMethod> snakeNavigation;

	#region INavigationManager implementation
	
	public DefaultNavigationManager()
	{
		this.snakeNavigation = new Dictionary<uint, NavigationMethod> ();
	}

	public NavigationMethod GetNavigationMethod (uint snakeID)
	{
		if( snakeNavigation.ContainsKey(snakeID) )
		{
			return snakeNavigation[snakeID];
		}
		throw new GridObjectNotFoundException();
	}

	public void AddSnake (uint snakeID, NavigationMethod method)
	{
		snakeNavigation.Add (snakeID, method);
	}

	public NavigationMethod GetRandomNavigationMethod ()
	{
		NavigationMethod method = NavigationMethod.DepthFirst;
		System.Random random = new System.Random();
		uint randomInt = (uint)random.Next ((int)NavigationMethod.NavigationMethodStart, (int)NavigationMethod.NavigationMethodEnd);
		if (randomInt >= (uint)NavigationMethod.NavigationMethodStart && randomInt <= (uint)NavigationMethod.NavigationMethodEnd) 
		{
			return (NavigationMethod)randomInt;
		}
		return NavigationMethod.BreadthFirst;
	}

	#endregion


}
