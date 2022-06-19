using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    public int xBoard = -1;
    public int yBoard = -1;

    private string player;

    public Sprite black_king, black_queen, black_rook, black_bishop, black_knight, black_pawn;
    public Sprite white_king, white_queen, white_rook, white_bishop, white_knight, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        
        //take Instantiated location and adjust transfrom
        SetChoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; this.transform.localScale = new Vector3(0.14f, 0.14f, 1); break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; this.transform.localScale = new Vector3(0.18f, 0.18f, 1); break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; this.transform.localScale = new Vector3(0.14f, 0.14f, 1); break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; this.transform.localScale = new Vector3(0.42f, 0.42f, 1); break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;

            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; this.transform.localScale = new Vector3(0.09f, 0.09f, 1); break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; this.transform.localScale = new Vector3(0.4f, 0.4f, 1); break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; this.transform.localScale = new Vector3(0.22f, 0.22f, 1); break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; this.transform.localScale = new Vector3(0.43f, 0.43f, 1); break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetChoords ()
    {
        float x = xBoard;
        float y = yBoard;

        x -= 3.5f * 1f;//gridsize
        y -= 3.5f * 1f;

        this.transform.position = new Vector3(x, y, -1f);
        CheckPromotion();
        
    }

    public void CheckPromotion()
    {
        if (this.name == "white_pawn" && this.yBoard == 7)
        {
            this.GetComponent<SpriteRenderer>().sprite = white_queen;
            this.transform.localScale = new Vector3(0.4f, 0.4f, 1);
            this.name = "white_queen";

        } else if (this.name == "black_pawn" && this.yBoard == 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = black_queen;
            this.transform.localScale = new Vector3(0.14f, 0.14f, 1);
            this.name = "black_queen";
        }
    }

    public void OnMouseUp()
    {
        if (!controller.GetComponent<GameController>().IsGameOver() && controller.GetComponent<GameController>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();
            InitiateMovePlates(true) ;
        }
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates(bool isReal)
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0, isReal);
                LineMovePlate(0, 1, isReal);
                LineMovePlate(1, 1, isReal);
                LineMovePlate(-1, 0, isReal);
                LineMovePlate(0, -1, isReal);
                LineMovePlate(-1, -1, isReal);
                LineMovePlate(-1, -1, isReal);
                LineMovePlate(-1, 1, isReal);
                LineMovePlate(1, -1, isReal);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate(isReal);
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1, isReal);
                LineMovePlate(1, -1, isReal);
                LineMovePlate(-1, -1, isReal);
                LineMovePlate(-1, 1, isReal);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate(isReal);
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0, isReal);
                LineMovePlate(0, 1, isReal);
                LineMovePlate(0, -1, isReal);
                LineMovePlate(-1, 0, isReal);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1, isReal);
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1, isReal);
                break;
        }
    }

    //Adds everything in list2 to list1
    public List<Board> flatAdd(List<Board> list1, List<Board> list2)
    {
        foreach (Board board in list2)
        {
            if (!board.isEmpty()) list1.Add(board);
        }
        return list1;
    }

    public List<Board> InitiateAiMove(Board boardt)
    {
        List<Board> moves = new List<Board>();
        
        Board board = boardt.Clone();
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                moves = flatAdd(moves, LineMoveAi(1, 0,board));
                moves = flatAdd(moves, LineMoveAi(0, 1,board));
                moves = flatAdd(moves, LineMoveAi(1, 1,board));
                moves = flatAdd(moves, LineMoveAi(-1,0,board));
                moves = flatAdd(moves, LineMoveAi(0,-1,board));
                moves = flatAdd(moves, LineMoveAi(-1,-1, board));
                moves = flatAdd(moves, LineMoveAi(-1,-1, board));
                moves = flatAdd(moves, LineMoveAi(-1,1,board));
                moves = flatAdd(moves, LineMoveAi(1,-1,board));
                break;
            case "black_knight":
            case "white_knight":
                moves = flatAdd(moves, LMoveAi(board));
                break;
            case "black_bishop":
            case "white_bishop":
                moves = flatAdd(moves, LineMoveAi(1, 1,board));
                moves = flatAdd(moves, LineMoveAi(1, -1,board));
                moves = flatAdd(moves, LineMoveAi(-1, -1, board));
                moves = flatAdd(moves, LineMoveAi(-1, 1, board));
                break;
            case "black_king":
            case "white_king":
                moves = flatAdd(moves, SurroundMoveAi(board));
                break;
            case "black_rook":
            case "white_rook":
                moves = flatAdd(moves, LineMoveAi(1, 0, board));
                moves = flatAdd(moves, LineMoveAi(0, 1, board));
                moves = flatAdd(moves, LineMoveAi(0, -1, board));
                moves = flatAdd(moves, LineMoveAi(-1, 0, board));
                break;
            case "black_pawn":
                moves = flatAdd(moves, PawnMoveAi(xBoard, yBoard - 1, board));
                break;
            case "white_pawn":
                moves = flatAdd(moves, PawnMoveAi(xBoard, yBoard + 1, board));
                break;
        }
        return moves;
    }

    public void LineMovePlate(int xIncrement, int yIncrement, bool isReal)
    {
        GameController gc = controller.GetComponent<GameController>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (gc.PositionOnBoard(x, y) && gc.GetPosition(x,y) == null)
        {
            MovePlateSpawn(x, y, isReal);
            x += xIncrement;
            y += yIncrement;
        }

        if (gc.PositionOnBoard(x, y) && gc.GetPosition(x,y).GetComponent<PieceController>().player != player)
        {
            MovePlateAttackSpawn(x, y, isReal);
        }
    }

    public List<Board> LineMoveAi(int xIncrement, int yIncrement, Board boardt)
    {
        Board board = boardt.Clone();
        List<Board> moves = new List<Board>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (board.PositionOnBoard(x, y) && board.GetPosition(x, y) == null)
        {
            moves.Add(MovePlateAi(x, y, board));
            x += xIncrement;
            y += yIncrement;
        }

        if (board.PositionOnBoard(x, y) && board.GetPosition(x, y).pc.player != player)
        {
            moves.Add(MovePlateAttackAi(x, y, board));
        }
        return moves;
    }

    public void LMovePlate(bool isReal)
    {
        PointMovePlate(xBoard + 1, yBoard + 2, isReal);
        PointMovePlate(xBoard - 1, yBoard + 2, isReal);
        PointMovePlate(xBoard + 2, yBoard + 1, isReal);
        PointMovePlate(xBoard + 2, yBoard - 1, isReal);
        PointMovePlate(xBoard + 1, yBoard - 2, isReal);
        PointMovePlate(xBoard - 1, yBoard - 2, isReal);
        PointMovePlate(xBoard - 2, yBoard + 1, isReal);
        PointMovePlate(xBoard - 2, yBoard - 1, isReal);
    }

    public List<Board> LMoveAi(Board boardt)
    {
        List<Board> moves = new List<Board>();
        Board board = boardt.Clone();
        moves.Add(PointMoveAi(xBoard + 1, yBoard + 2, board));
        moves.Add(PointMoveAi(xBoard - 1, yBoard + 2, board));
        moves.Add(PointMoveAi(xBoard + 2, yBoard + 1, board));
        moves.Add(PointMoveAi(xBoard + 2, yBoard - 1, board));
        moves.Add(PointMoveAi(xBoard + 1, yBoard - 2, board));
        moves.Add(PointMoveAi(xBoard - 1, yBoard - 2, board));
        moves.Add(PointMoveAi(xBoard - 2, yBoard + 1, board));
        moves.Add(PointMoveAi(xBoard - 2, yBoard - 1, board));
        return moves;
    }

    public void SurroundMovePlate(bool isReal)
    {
        PointMovePlate(xBoard + 1, yBoard + 1, isReal);
        PointMovePlate(xBoard + 1, yBoard - 1, isReal);
        PointMovePlate(xBoard - 1, yBoard - 1, isReal);
        PointMovePlate(xBoard - 1, yBoard + 1, isReal);
        PointMovePlate(xBoard, yBoard + 1, isReal);
        PointMovePlate(xBoard, yBoard - 1, isReal);
        PointMovePlate(xBoard + 1, yBoard, isReal);
        PointMovePlate(xBoard - 1, yBoard, isReal);
    }

    public List<Board> SurroundMoveAi(Board board)
    {
        List<Board> moves = new List<Board>();
        Board move;
        move = PointMoveAi(xBoard + 1, yBoard + 1, board);
        if (!move.isEmpty()) moves.Add(move);
        move = PointMoveAi(xBoard - 1, yBoard - 1, board);
        if (!move.isEmpty()) moves.Add(move);
        move = PointMoveAi(xBoard + 1, yBoard - 1, board);
        if (!move.isEmpty()) moves.Add(move);
        move = PointMoveAi(xBoard - 1, yBoard + 1, board);
        if (!move.isEmpty()) moves.Add(move);
        move = PointMoveAi(xBoard, yBoard + 1, board);
        if (!move.isEmpty()) moves.Add(move);
        move = PointMoveAi(xBoard, yBoard - 1, board);
        if (!move.isEmpty()) moves.Add(move);
        move = PointMoveAi(xBoard + 1, yBoard, board);
        if (!move.isEmpty()) moves.Add(move);
        move = PointMoveAi(xBoard - 1, yBoard, board);
        if (!move.isEmpty()) moves.Add(move);
        return moves;
    }

    public void PointMovePlate(int x, int y, bool isReal)
    {
        GameController gc = controller.GetComponent<GameController>();
        if (gc.PositionOnBoard(x, y))
        {
            GameObject cp = gc.GetPosition(x, y);
            if (cp == null)
            {
                MovePlateSpawn(x, y, isReal);
            } else if (cp.GetComponent<PieceController>().player != player)
            {
                MovePlateAttackSpawn(x, y, isReal);
            }
        }
        
    }

    public Board PointMoveAi(int x, int y, Board board)
    {
        Board gc = new Board();
        gc.getBoard(board);
        Board move;
        if (gc.PositionOnBoard(x, y))
        {
            Piece cp = gc.GetPosition(x, y);
            if (cp == null)
            {
                move = MovePlateAi(x, y, board);
            }
            else if (cp.pc.player != player)
            {
                move = MovePlateAttackAi(x, y, board);
            }
            else
            {
                return new Board();
            }
            return move;
        }
        else return new Board();
    }

    public void PawnMovePlate(int x, int y, bool isReal)
    {
        GameController gc = controller.GetComponent<GameController>();
        
        if (gc.PositionOnBoard(x, y))
        {
            if (gc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y, isReal);
            }

            if (gc.PositionOnBoard(x + 1, y) && gc.GetPosition(x + 1, y) != null && 
                gc.GetPosition(x + 1, y).GetComponent<PieceController>().player != player)
            {
                MovePlateAttackSpawn(x+1, y, isReal);
            }

            if (gc.PositionOnBoard(x - 1, y) && gc.GetPosition(x - 1, y) != null &&
                gc.GetPosition(x - 1, y).GetComponent<PieceController>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y, isReal);
            }
        }
        if (y == 2 && player == "white")
        {
            if (gc.GetPosition(x, y+1) == null && gc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y+1, isReal);
            }
        } else if (y == 5 && player == "black")
        {
            if (gc.GetPosition(x, y - 1) == null && gc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y - 1, isReal);
            }
        }
    }

    public List<Board> PawnMoveAi(int x, int y, Board board)
    {
        List<Board> moves = new List<Board>();
        Board gc = board;
        if (gc.PositionOnBoard(x, y))
        {
            if (gc.GetPosition(x, y) == null)
            {
                moves.Add(MovePlateAi(x, y, board));
            }

            if (gc.PositionOnBoard(x + 1, y) && gc.GetPosition(x + 1, y) != null &&
                gc.GetPosition(x + 1, y).pc.player != player)
            {
                moves.Add(MovePlateAttackAi(x + 1, y, board));
            }

            if (gc.PositionOnBoard(x - 1, y) && gc.GetPosition(x - 1, y) != null &&
                gc.GetPosition(x - 1, y).pc.player != player)
            {
                moves.Add(MovePlateAttackAi(x - 1, y, board));
            }
        }
        if (y == 2 && player == "white")
        {
            if (gc.GetPosition(x, y + 1) == null && gc.GetPosition(x, y) == null)
            {
                moves.Add(MovePlateAi(x, y + 1, board));
            }
        }
        else if (y == 5 && player == "black")
        {
            if (gc.GetPosition(x, y - 1) == null && gc.GetPosition(x, y) == null)
            {
                moves.Add(MovePlateAi(x, y - 1, board));
            }
        }
        return moves;
    }

    public void MovePlateSpawn(int matrixX, int matrixY, bool isReal)
    {
        float x = matrixX;
        float y = matrixY;

        x -= 3.5f * 1f;//gridsize
        y -= 3.5f * 1f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);
        
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetChoords(matrixX, matrixY);
    }

    public Board MovePlateAi(int matrixX, int matrixY, Board boardt)
    {
        Board board = boardt.Clone();
        int xPrev = xBoard;
        int yPrev = yBoard;
        Board gc = new Board();
        gc.getBoard(board);
        gc.SetPositionEmpty(xBoard, yBoard);
        xBoard = matrixX;
        yBoard = matrixY;
        Piece piece = new Piece(this.name, this);
        piece.pc.xBoard = matrixX;
        piece.pc.yBoard = matrixY;
        gc.SetPosition(piece);
        //gc.turn += 1;
        gc.white = !gc.white;
        xBoard = xPrev;
        yBoard = yPrev;
        return gc;
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY, bool isReal)
    {
        float x = matrixX;
        float y = matrixY;

        x -= 3.5f * 1f;//gridsize
        y -= 3.5f * 1f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3.0f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetChoords(matrixX, matrixY);
    }

    public Board MovePlateAttackAi(int matrixX, int matrixY, Board boardt)
    {
        Board board = boardt.Clone();
        int xPrev = xBoard;
        int yPrev = yBoard;
        Board gc = new Board();
        gc.getBoard(board);
        gc.SetPositionEmpty(xBoard, yBoard);
        gc.SetPositionEmpty(matrixX, matrixY);
        xBoard = matrixX;
        yBoard = matrixY;
        Piece piece = new Piece(this.name, this);
        piece.pc.xBoard = matrixX;
        piece.pc.yBoard = matrixY;
        gc.SetPosition(piece.Clone());
        //gc.turn += 1;
        gc.white = !gc.white;
        xBoard = xPrev;
        yBoard = yPrev;
        return gc;
    }
}
