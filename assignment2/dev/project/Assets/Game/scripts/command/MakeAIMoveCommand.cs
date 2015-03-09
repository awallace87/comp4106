using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;

public class MakeAIMoveCommand : Command 
{
    [Inject]
    public DiscColour turnToPlay { get; set; }

    private uint numOfNodes = 0;
 
    public override void Execute()
    {
        IGameManager gameManager = injectionBinder.GetInstance<IGameManager>() as IGameManager;

        //Debug.Log("Total Mobility for " + turnToPlay.ToString() + " is " + CalculateTotalMobility(gameManager.GetGameBoard(), turnToPlay));
        MoveInformationTuple searchMove = MinimaxSearch(turnToPlay, gameManager.GetGameBoard(), 3);
        Debug.Log("Next Move Found for " + turnToPlay.ToString() + " @ " + searchMove.PlayPosition.ToString());
        Debug.Log("Move found with " + numOfNodes + " nodes searched");
        PlayMove(searchMove.PlayPosition);
    }

    private void PlayMove(GridPosition foundPosition)
    {
        PlayTurnSignal playSignal = injectionBinder.GetInstance<PlayTurnSignal>() as PlayTurnSignal;
        playSignal.Dispatch(foundPosition, turnToPlay);
    }

    
    private MoveInformationTuple MinimaxSearch(DiscColour player, IBoardModel board, uint depth)
    {
        numOfNodes++;

        MoveInformationTuple bestMove = new MoveInformationTuple();
        IList<GridPosition> availableMoves = board.GetLegalMoves(player);

        if(depth == 0)
        {
            bestMove.MobilityDifference = GetMobilityDifference(board);
            return bestMove;
        }

        int initialSetting;
        if( player == turnToPlay)
        {
            initialSetting = int.MinValue;
        }
        else
        {
            initialSetting = int.MaxValue;
        }

        bestMove.MobilityDifference = initialSetting;


        foreach (GridPosition legalMove in availableMoves)
        {
            IBoardModel moveBoard = makePlay(copy(board), legalMove, player);
            MoveInformationTuple nextMove = MinimaxSearch(GetOpponent(player), moveBoard, depth - 1);
            if (player == turnToPlay)
            {
                if (nextMove.MobilityDifference > bestMove.MobilityDifference)
                {
                    bestMove.PlayPosition = legalMove;
                    bestMove.MobilityDifference = nextMove.MobilityDifference;
                }
            }
            else
            {
                if (nextMove.MobilityDifference < bestMove.MobilityDifference)
                {
                    bestMove.PlayPosition = legalMove;
                    bestMove.MobilityDifference = nextMove.MobilityDifference;
                }
            }
        }

        return bestMove;
    }

    private IBoardModel makePlay(IBoardModel originalBoard, GridPosition playLocation, DiscColour player)
    {
        IList<GridPosition> affectedPositions = originalBoard.GetAffectedDiscPositions(player, playLocation);

        foreach (GridPosition affectedPosition in affectedPositions)
        {
            originalBoard.Board[affectedPosition.X, affectedPosition.Y].Disc.Colour = player;
        }

        IDiscModel disc = injectionBinder.GetInstance<IDiscModel>() as IDiscModel;
        disc.Colour = player;
        originalBoard.Board[playLocation.X, playLocation.Y].Disc = disc;

        return originalBoard;
    }

    private int GetMobilityDifference(IBoardModel board)
    {
        uint maximizePlayer = CalculateTotalMobility(board, turnToPlay);
        uint minimizePlayer = CalculateTotalMobility(board, GetOpponent(turnToPlay));
        return (int)maximizePlayer - (int)minimizePlayer;
    }

    private uint CalculateTotalMobility(IBoardModel board, DiscColour player)
    {
        uint totalMobility = 0;
        foreach (GridPosition playPosition in board.GetLegalMoves(player))
        {
            totalMobility += board.Board[playPosition.X, playPosition.Y].MobilityScore;
        }
        return totalMobility;
    }

    private DiscColour GetOpponent(DiscColour player)
    {
        if(player == DiscColour.Black) { return DiscColour.White; }

        return DiscColour.Black;
    }

    private IBoardModel copy(IBoardModel original)
    {
        IBoardModel duplicate = injectionBinder.GetInstance<IBoardModel>() as IBoardModel;

        for (int i = 0; i < original.BoardSize; i++)
        {
            for (int j = 0; j < original.BoardSize; j++)
            {
                duplicate.Board[i, j] = copy(original.Board[i, j]);
            }
        }

        return duplicate;
    }

    private IBoardSquareModel copy(IBoardSquareModel original)
    {
        IBoardSquareModel duplicate = injectionBinder.GetInstance<IBoardSquareModel>() as IBoardSquareModel;
        duplicate.MobilityScore = original.MobilityScore;
		if (original.Disc != null) { duplicate.Disc = copy (original.Disc); }
        duplicate.State = original.State;

        return duplicate;
    }

	private IDiscModel copy(IDiscModel original)
	{
		IDiscModel duplicate = injectionBinder.GetInstance<IDiscModel> () as IDiscModel;
		duplicate.Colour = original.Colour;
		return duplicate;
	}

    private class MoveInformationTuple
    {
        public GridPosition PlayPosition;
        public int MobilityDifference;
    }

}