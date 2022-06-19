using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScores
{
    public Board move;
    public float score;

    public GameScores(float tScore, Board board)
    {
        move = board;
        score = tScore;
    }
}


public class MiniMax : MonoBehaviour
{
    public GameController controller;
    public int maxDepth;
    int checks = 0;
    Board logic;
    public List<GameScores> ChildrenScoreList;

    private void Start()
    {
    }

    public Board CallMinimax(bool isPlayer)
    {
        logic = new Board(controller).Clone();
        ChildrenScoreList = new List<GameScores>();
        checks = 0;
        //minimax(0, -10000, 10000, isPlayer);
        //return ReturnWorstMove();

        search(maxDepth, -10000, 10000, isPlayer);
        return ReturnBestMove();

        //Board prev = logic;
        //logic = logic.moves(logic.blackPieces[4])[1];
        //logic = prev;
        //return logic;
    }

    //Attempt at a redo, works better but still doesn't work right
    int search(int depth, int alpha, int beta, bool isWhite)
    {

        if (depth == 0 || logic.isGameOver())
        {
            return (int)EvaluateBoard(logic, isWhite);
        }
        List<Board> moves = logic.allMoves();

        if (alpha >= beta)
        {
            return alpha;
        }

        foreach (Board move in moves)
        {
            move.white = !move.white;
            Board pre = logic.Clone();
            logic = move.Clone();
            int evaluation = -search(depth - 1, -beta, -alpha, !isWhite);
            logic = pre.Clone();
            if(evaluation >= beta)
                return beta;
            if (evaluation > alpha || (evaluation == alpha && Random.Range(0, 3) == 0))//If equal 1/4 chance of using new move
            {
                alpha = evaluation;
                if (depth == maxDepth)
                    ChildrenScoreList.Add(new GameScores(alpha, move));
            }
        }
        return alpha;
    }

    public float minimax(int depth, float alpha, float beta, bool isWhite)
    {
        float value = EvaluateBoard(logic, isWhite);
        checks += 1;
        // STEP 1: If terminal node, check if the game has been won
        // At the terminal node, we return the static evaluation function result. 
        // In this case, basically return +1 or -1, no calculation required
        if (depth == maxDepth)
        {
            return value;
        }
        if (logic.isGameOver())
        {
            return value;
        }

        List<Board> moves;
        Board prev = logic.Clone();

        List<float> scores = new List<float>();
        //if (isWhite)
        //{
        // STEP 2: Get all the available moves on the board
        moves = logic.allMoves();
        //Debug.Log("num moves: " + moves.Count);
        //float maxEval = -10000f;
        for (int i = 0; i < moves.Count; i++)
        {
            Board move = moves[i];
            bool neg = false;
            if (EvaluateBoard(move, isWhite) < 0)
            {
                Debug.Log("-ve move");
                move.printPositions();
                neg = true;
            }
            //move.white = !move.white;
            logic = move;

            float eval = minimax(depth + 1, alpha, beta, !isWhite);

            logic = prev.Clone();//Undo move

            if (neg)
            {
                //Debug.Log("Neg move eval: " + eval);
            }
            scores.Add(eval);
            

            if (depth == 0)
            {
                ChildrenScoreList.Add(new GameScores(eval, move));
            }
            
            // If a bit better -> use move, else if close randomly take or not
            //if ((eval > (maxEval + 0.25f)) || (eval > maxEval - 0.25f && Random.Range(0, 1) == 0))
        }
        float returnVal;
        if (isWhite)
        {
            //printList(scores);
            returnVal = scores.Max();
            //Debug.Log("Max Score: " + returnVal);
        }
        else
        {
            //printList(scores);
            returnVal = scores.Min();
            //Debug.Log("Min Score: " + returnVal);
        }
        return returnVal;
        //    if (depth == 0)
        //        return bestMove;
        //    Debug.Log("Max Score: " + bestMove.score);
        //    return bestMove;
        //} else //isMinimising
        //{
        //    moves = board.allMoves(true);
        //    //Debug.Log("num moves: " + moves.Count);
        //    float minEval = 10000f;
        //    for (int i = 0; i < moves.Count; i++)
        //    {
        //        Board move = moves[i];

        //        board.SetPositions(move.GetPositions()); //Do Move
        //        float eval = minimax(board, depth + 1, alpha, beta, true).score;// Recurse
        //        board.SetPositions(prev);//Undo move
        //        // If a bit worse -> use move, else if close randomly take or not
        //        //if ((eval < (minEval - 0.25f)) || (eval < (minEval+0.25f) && Random.Range(0, 1) == 0))
        //        if (eval < minEval)
        //        {
        //            minEval = eval;
        //            bestMove = new Move();
        //            bestMove.board = move;
        //            bestMove.score = minEval;
        //        }
        //        //beta = Mathf.Min(beta, eval);
        //        //if (beta <= alpha)
        //         //   break;
        //    }
        //    if (depth == 0)
        //        return bestMove;
        //    Debug.Log("Min Score: " + bestMove.score);
        //    return bestMove;
    }

    void printList(List<float> scores)
    {
        string result = "Scores: [";
        foreach (var item in scores)
        {
            result += item.ToString() + ", ";
        }
        result += "]";
        Debug.Log(result);
    }

    public Board ReturnWorstMove()
    {
        float min = 10000; //start from an ultra low number
        int best = -1; // The index of the best move
        string str = "Children Scores: ";
        // STEP 1: Look for the best move
        for (int i = 0; i < ChildrenScoreList.Count; i++)
        {
            str += ChildrenScoreList[i].score + ", ";
            if (min > ChildrenScoreList[i].score)
            {
                min = ChildrenScoreList[i].score;
                best = i;
            }
        }

        if (best > -1 && best < ChildrenScoreList.Count)
        {
            Debug.Log(str);
            //Debug.Log("best move: ");
            //ChildrenScoreList[best].move.printPositions();
            return ChildrenScoreList[best].move;
        }


        // STEP 2: Otherwise return empty
        //Debug.Log("What went wrong?");
        Board empty = new Board();
        return empty;
    }

    public Board ReturnBestMove()
    {
        float max = -10000; //start from an ultra low number
        int best = -1; // The index of the best move
        string str = "Children Scores: ";
        // STEP 1: Look for the best move
        for (int i = 0; i < ChildrenScoreList.Count; i++)
        {
            str += ChildrenScoreList[i].score + ", ";
            if (max < ChildrenScoreList[i].score)
            {
                max = ChildrenScoreList[i].score;
                best = i;
            }
        }

        if (best > -1 && best < ChildrenScoreList.Count)
        {
            Debug.Log(str);
            //Debug.Log("best move: ");
            //ChildrenScoreList[best].move.printPositions();
            return ChildrenScoreList[best].move;
        }


        // STEP 2: Otherwise return empty
        //Debug.Log("What went wrong?");
        Board empty = new Board();
        return empty;
    }

    public float EvaluateBoard(Board board, bool isWhite)
    {
        float score = 0;
        int perspecitve = isWhite ? 1 : -1;
        score += EvalMaterial(board.GetPositions()) * 1f * perspecitve;
        return score;
    }

    public float EvalMaterial(Piece[,] positions)
    {
        float score = 0f;
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                if (positions[i, j] != null)
                {
                    switch (positions[i, j].name)
                    {
                        case "black_queen": score -= 9; break;
                        case "black_rook": score -= 5; break;
                        case "black_king": score -= 100; break;
                        case "black_bishop": score -= 3; break;
                        case "black_knight": score -= 3; break;
                        case "black_pawn": score -= 1; break;
                            

                        case "white_king": score += 100; break;
                        case "white_queen": score += 9; break;
                        case "white_rook": score += 5; break;
                        case "white_bishop": score += 3; break;
                        case "white_knight": score += 3; break;
                        case "white_pawn": score += 1; break;
                    }
                }
            }
        }
        return score;
    }

}


