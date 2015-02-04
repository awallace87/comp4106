using UnityEngine;
using System.Collections;

public interface ISnakeModel : IGridObject{
    GridDirection Direction { get; set; }
	void Move();
}
