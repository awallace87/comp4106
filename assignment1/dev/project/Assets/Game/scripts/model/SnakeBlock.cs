using UnityEngine;
using System.Collections;

public class SnakeBlock : IBoardObject {

    private BoardPosition position;
    public BoardPosition IBoardObject.Position
    {
        get { return position; }
        set { position = value; }
    }

    public BoardDirection Facing { get; set; }

    public SnakeBlock(uint x, uint y)
    {
        position = new BoardPosition(x,y);
        Facing = BoardDirection.Invalid;
    }

    public void Move(BoardDirection nextDirection)
    {
        if(Facing != BoardDirection.Invalid)
        {
            //TODO Move Self According to Current Direction
        }

        Facing = nextDirection;
    }
}
