using UnityEngine;
using System.Collections;
using strange.extensions.command.impl;
using System.Collections.Generic;
using System;

public class MakeAIMoveCommand : Command 
{
    [Inject]
    public DiscColour turnToPlay { get; set; }

    private uint numOfNodes = 0;
    private uint numOfBreaks = 0;

    public enum Heuristic
    {
        Score
        , Mobility
    }
 
    public override void Execute()
    {
        IGameManager gameManager = injectionBinder.GetInstance<IGameManager>() as IGameManager;

        IPlayer currentPlayer;
        if(turnToPlay == DiscColour.White)
        {
            currentPlayer = gameManager.WhitePlayer;
        }
        else
        {
            currentPlayer = gameManager.BlackPlayer;
        }

        GridPosition playPosition = GetBestPlay(turnToPlay, gameManager.GetGameBoard(), currentPlayer.GetPlayMethod()); 

        Debug.Log("Move found with " + numOfNodes + " nodes searched and " + numOfBreaks + " breaks");
        PlayMove(playPosition);
    }

    private GridPosition GetBestPlay(DiscColour player, IBoardModel board, PlayMethod search)
    {
        switch (search)
        {
            case PlayMethod.MinimaxMobility:
            case PlayMethod.MinimaxScore:
                return BeginMinimaxSearch(player, board, GetHeuristic(search)); 
            case PlayMethod.AlphaBetaMobility:
            case PlayMethod.AlphaBetaScore:
                return BeginAlphaBetaSearch(player, board, GetHeuristic(search)); 
        }
        throw new System.NotImplementedException();
    }

    private Heuristic GetHeuristic(PlayMethod search)
    {
        Heuristic heuristic = Heuristic.Score;
        switch (search)
        {
            case PlayMethod.AlphaBetaMobility:
            case PlayMethod.MinimaxMobility:
                heuristic = Heuristic.Mobility;
                break;
            case PlayMethod.AlphaBetaScore:
            case PlayMethod.MinimaxScore:
                heuristic = Heuristic.Score;
                break;
        }
        return heuristic;
    }

    private GridPosition BeginMinimaxSearch(DiscColour player, IBoardModel board, Heuristic heuristic)
    {
        Debug.Log("Begin Minimax");
        SearchNode root;
        if (heuristic == Heuristic.Score)
        {
            root = MinimaxSearch(player, board, 3, GetWeightedScoreDifference);
        }
        else
        {
            root = MinimaxSearch(player, board, 3, GetMobilityDifference);
        }

        return root.bestMove;
    }

    private SearchNode MinimaxSearch(DiscColour player, IBoardModel board, uint depth, Func<IBoardModel,int> heuristic)
    {
        SearchNode node = new SearchNode();
        numOfNodes++;

        node.board = board;
        node.boardValue = heuristic(board);
        //node.boardValue = GetMobilityDifference(board);
        IList<GridPosition> availableMoves = node.board.GetLegalMoves(player);

        if (depth == 0 || availableMoves.Count <= 0)
        {
            return node;
        }

        int initialSetting;
        if (player == turnToPlay)
        {
            initialSetting = int.MinValue;
        }
        else
        {
            initialSetting = int.MaxValue;
        }

        node.boardValue = initialSetting;


        foreach (GridPosition legalMove in availableMoves)
        {
            IBoardModel moveBoard = makePlay(copy(board), legalMove, player);
            SearchNode nextMove = MinimaxSearch(GetOpponent(player), moveBoard, depth - 1, heuristic);
            if (player == turnToPlay)
            {
                if (nextMove.boardValue > node.boardValue)
                {
                    node.bestMove = legalMove;
                    node.boardValue = nextMove.boardValue;
                }
            }
            else
            {
                if (nextMove.boardValue < node.boardValue)
                {
                    node.bestMove = legalMove;
                    node.boardValue = nextMove.boardValue;
                }
            }
            node.children.Add(legalMove, nextMove);
        }

        return node;
    }

    private GridPosition BeginAlphaBetaSearch(DiscColour player, IBoardModel board, Heuristic heuristic)
    {
        Debug.Log("Begin AlphaBeta");
        SearchNode alphaBetaRoot;
        if (heuristic == Heuristic.Score)
        {
             alphaBetaRoot = AlphaBetaSearch(player, board, 4, int.MinValue, int.MaxValue, GetWeightedScoreDifference);
        }
        else
        {
            alphaBetaRoot = AlphaBetaSearch(player, board, 4, int.MinValue, int.MaxValue, GetMobilityDifference);
        }
        return alphaBetaRoot.bestMove;
    }

    private SearchNode AlphaBetaSearch(DiscColour player, IBoardModel board, uint depth, int alpha, int beta, Func<IBoardModel, int> heuristic)
    {
        SearchNode node = new SearchNode();
        numOfNodes++;

        node.board = board;
        node.boardValue = heuristic(board);

        node.alpha = alpha;
        node.beta = beta;

        IList<GridPosition> availableMoves = board.GetLegalMoves(player);

        if (depth == 0 || availableMoves.Count <= 0)
        {
            return node;
        }

        int initialSetting;
        if (player == turnToPlay)
        {
            initialSetting = int.MinValue;
        }
        else
        {
            initialSetting = int.MaxValue;
        }
        node.boardValue = initialSetting;

        foreach (GridPosition legalMove in availableMoves)
        {
            IBoardModel moveBoard = makePlay(copy(board), legalMove, player);
            SearchNode nextMove = AlphaBetaSearch(GetOpponent(player), moveBoard, depth - 1, node.alpha, node.beta, heuristic);
            if (player == turnToPlay)
            {
                if (nextMove.boardValue > node.boardValue)
                {
                    node.bestMove = legalMove;
                    node.boardValue = nextMove.boardValue;
                }

                node.alpha = Math.Max(node.alpha, node.boardValue);
            }
            else
            {
                if (nextMove.boardValue < node.boardValue)
                {
                    node.bestMove = legalMove;
                    node.boardValue = nextMove.boardValue;
                }

                node.beta = Math.Min(node.beta, node.boardValue);
            }

            if (node.beta <= node.alpha)
            {
                numOfBreaks++;
                break;
            }
        }

        return node;
    }
    /*
    private GridPosition BeginIterativeDeepeningSearch(DiscColour player, IBoardModel board)
    {

    }

    private SearchNode IterativeDFS(DiscColour player, IBoardModel model)
    {
        SearchNode root;
    }*/

    private class SearchNode
    {
        public IBoardModel board { get; set; }
        public int boardValue { get; set; }
        public DiscColour player { get; set; }
        public Dictionary<GridPosition, SearchNode> children { get; set; }
        public GridPosition bestMove { get; set; }

        public int alpha { get; set; }
        public int beta { get; set; }

        public SearchNode()
        {
            children = new Dictionary<GridPosition,SearchNode>();
        }
    }


    private void PlayMove(GridPosition foundPosition)
    {
        PlayTurnSignal playSignal = injectionBinder.GetInstance<PlayTurnSignal>() as PlayTurnSignal;
        playSignal.Dispatch(foundPosition, turnToPlay);
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

    private int GetWeightedScoreDifference(IBoardModel board)
    {
        int maxPlayerScore = GetWeightedScore(board, turnToPlay);
        int minPlayerScore = GetWeightedScore(board, GetOpponent(turnToPlay));
        return maxPlayerScore - minPlayerScore;
    }

    private int GetWeightedScore(IBoardModel board, DiscColour player)
    {
        int score = 0;
        for (int i = 0; i < board.BoardSize; i++)
        {
            for (int j = 0; j < board.BoardSize; j++)
            {
                if (board.Board[i, j].ContainsDisc() && board.Board[i, j].Disc.Colour == player)
                {
                    score += (int)board.Board[i, j].MobilityScore;
                }
            }
        }
        return score;
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

    private bool IsWinningBoard(IBoardModel board)
    {
        uint numOfDiscs = 0;
        for (int i = 0; i < board.BoardSize; i++)
        {
            for (int j = 0; j < board.BoardSize; j++)
            {
                IBoardSquareModel boardSquare = board.Board[i, j];
                if (!boardSquare.ContainsDisc()) { return false; }

                if (boardSquare.Disc.Colour == turnToPlay) { numOfDiscs++; }
            }
        }
        return numOfDiscs > (board.BoardSize * board.BoardSize / 2.0f);
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
}