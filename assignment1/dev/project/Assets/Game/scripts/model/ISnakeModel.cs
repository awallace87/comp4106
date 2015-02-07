using UnityEngine;
using System.Collections;

public interface ISnakeModel : IGridObject{
    GridDirection Direction { get; set; }
    GridPosition NextPosition { get; set; }
    ISnakeModel Next { get; set; }
}

public enum SnakeBindings
{
    Head
    ,Tail
}
