using UnityEngine;
using System.Collections;

public class DefaultBoardModel : IBoardModel 
{
    private uint boardWidth;
    public uint IBoardModel.BoardWidth
    {
        get { return boardWidth; }
        set { boardWidth = value; }
    }

    private uint boardHeight;
    public uint IBoardModel.BoardHeight
    {
        get { return boardHeight; }
        set { boardHeight = value; }
    }

    private BoardObjectType[,] boardMap;

    public DefaultBoardModel(uint rows, uint col)
    {
        this.boardHeight = rows;
        this.boardWidth = col;
    }

    void InitializeBoard()
    {
        this.boardMap = new BoardObjectType[this.boardWidth,this.boardHeight];
    }

    public BoardObjectType GetObjectTypeAt(uint row, uint col)
    {
        BoardObjectType objectType = BoardObjectType.Invalid;
        if (row <= this.boardHeight && col <= this.boardWidth)
        {
            objectType = boardMap[col, row];
        }
        return objectType;
    }

    public void SetObjectTypeAt(uint row, uint col, ObjectType typeToSet)
    {
        if (row <= this.boardHeight && col <= this.boardWidth)
        {
            boardMap[col, row] = typeToSet;
        }
        else
        {
            Debug.Log("DefaultBoardModel::SetObjectTypeAt - Out of Bounds");
        }
    }
}
