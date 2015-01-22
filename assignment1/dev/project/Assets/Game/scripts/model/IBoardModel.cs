using UnityEngine;
using System.Collections;

public interface IBoardModel
{
    float BoardHeight { get; set; }
    float BoardWidth { get; set; }

    BoardObjectType GetObjectTypeAt(uint col, uint row);
    void SetObjectTypeAt(uint col, uint row, BoardObjectType objectType);
}

public enum BoardObjectType
{
    Invalid,
    Empty,
    Food,
    Wall,
    SnakeHead,
    SnakeTail
};

public enum BoardDirection
{
    Invalid = -1,
    Up,
    Down,
    Left,
    Right
}

public struct BoardPosition
{
    public uint X;
    public uint Y;

    public BoardPosition(uint x, uint y)
    {
        this.X = x;
        this.Y = y;
    }
}


