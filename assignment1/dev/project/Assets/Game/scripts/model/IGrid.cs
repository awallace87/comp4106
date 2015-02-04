using UnityEngine;
using System.Collections;

public interface IGrid {
    GridObjectType[,] Map { get; }
	uint Height { get; set; }
	uint Width { get; set; }
}
