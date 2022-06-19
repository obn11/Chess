using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    private Piece[,] positions;
    public Piece[] blackPieces = new Piece[16];
    public Piece[] whitePieces = new Piece[16];
    public bool white;

    public Piece[,] GetPositions()
    {
        return positions;
    }

    public void SetPositions(Piece[,] tPositions)
    {
        positions = tPositions;
    }

    public Board(GameController gc)
    {
        if (gc.positions != null)
        {
            positions = toPiece(gc.positions);
            whitePieces = toPiece(gc.whitePieces);
            blackPieces = toPiece(gc.blackPieces);
        }

        deligatePieces();
        white = gc.GetCurrentPlayer() == "white";
    }

    public Board(Piece[,] tpositions, Piece[] twhitePieces, Piece[] tblackPieces, bool twhite)
    {
        positions = tpositions;
        whitePieces = twhitePieces;
        blackPieces = tblackPieces;
        white = twhite;
    }

    public Board()
    {

    }

    public Board Clone()
    {
        Piece[,] nPositions = ClonePieces();
        Board clone = new Board(nPositions, whitePieces, blackPieces, white);
        return clone;
    }

    public bool isGameOver()
    {
        bool whiteKing = false;
        bool blackKing = false;
        foreach (Piece piece in whitePieces)
        {
  
            if (piece != null && piece.name == "white_king") whiteKing = true;
        }
        foreach (Piece piece in blackPieces)
        {
            if (piece != null && piece.name == "black_king") blackKing = true;
        }
        return !(whiteKing && blackKing);
    }

    public void printPieces()
    {
        string str = "White Pieces: ";
        foreach (Piece piece in whitePieces)
        {
            if (piece != null) str += piece.name + ", ";
        }
        str += "\nBlack Pieces: ";
        foreach (Piece piece in blackPieces)
        {
            if (piece != null) str += piece.name + ", ";
        }
        Debug.Log(str);
    }

    public Piece[,] ClonePieces()
    {
        Piece[,] pPositions = new Piece[8, 8];
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                Piece piece = positions[i, j];
                if (piece != null) pPositions[i, j] = piece.Clone();
                else pPositions[i, j] = null;
            }
        }
        return pPositions;
    }

    public Piece[,] toPiece(GameObject[,] positions)
    {
        Piece[,] pPositions = new Piece[8, 8];
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                GameObject piece = positions[i, j];
                if (piece != null) pPositions[i, j] = new Piece(piece.name, piece.GetComponent<PieceController>());
                else pPositions[i, j] = null;
            }
        }
        return pPositions;
    }

    public Piece[] toPiece(List<GameObject> pieces)
    {
        Piece[] pPieces = new Piece[16];
        for (int i = 0; i < 16; i++)
        {
            GameObject piece = pieces[i];
            if (piece != null) pPieces[i] = new Piece(piece.name, piece.GetComponent<PieceController>());
        }
        return pPieces;
    }

    public void getBoard(Board cloneFrom)
    {
        positions = cloneFrom.positions;
        blackPieces = cloneFrom.blackPieces;
        whitePieces = cloneFrom.whitePieces;
        white = cloneFrom.white;
    }

    public bool isEmpty()
    {
        return (positions == null);
    }

    public void deligatePieces()
    {
        int blackIndex = 0;
        int whiteIndex = 0;
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 8; i++)
            {
                if (positions[i, j] != null)
                {
                    switch (positions[i, j].name)
                    {
                        case "black_queen": blackPieces[blackIndex] = positions[i,j]; blackIndex += 1; break;
                        case "black_rook": blackPieces[blackIndex] = positions[i, j]; blackIndex += 1; break;
                        case "black_king": blackPieces[blackIndex] = positions[i, j]; blackIndex += 1; break;
                        case "black_bishop": blackPieces[blackIndex] = positions[i, j]; blackIndex += 1; break;
                        case "black_knight": blackPieces[blackIndex] = positions[i, j]; blackIndex += 1; break;
                        case "black_pawn": blackPieces[blackIndex] = positions[i, j]; blackIndex += 1; break;


                        case "white_king": whitePieces[blackIndex] = positions[i, j]; whiteIndex += 1; break;
                        case "white_queen": whitePieces[blackIndex] = positions[i, j]; whiteIndex += 1; break;
                        case "white_rook": whitePieces[blackIndex] = positions[i, j]; whiteIndex += 1; break;
                        case "white_bishop": whitePieces[blackIndex] = positions[i, j]; whiteIndex += 1; break;
                        case "white_knight": whitePieces[blackIndex] = positions[i, j]; whiteIndex += 1; break;
                        case "white_pawn": whitePieces[blackIndex] = positions[i, j]; whiteIndex += 1; break;
                    }
                }
            }
        }
    }

    //Return a list of all moves possible from current board state
    public List<Board> allMoves()
    {
        List<Board> moves = new List<Board>();
        if (positions == null)
        {
            return moves;
        }
        //deligatePieces();
        if (white)
        {
            Shuffle(whitePieces);
            foreach (Piece piece in whitePieces)
            {
                if (piece != null)
                {
                    List<Board> pieceMoves = this.moves(piece.Clone());
                    foreach (Board move in pieceMoves)
                    {
                        moves.Add(move.Clone());
                    }
                }
            }
        }
        else
        {
            Shuffle(blackPieces);
            foreach (Piece piece in blackPieces)
            {
                if (piece != null)
                {
                    List<Board> pieceMoves = this.moves(piece.Clone());
                    foreach (Board move in pieceMoves)
                    {
                        moves.Add(move.Clone());
                    }
                }
            }
        }
        return moves;
    }

    //Return a list of all moves from a piece
    public List<Board> moves(Piece piece)
    {
        List<Board> moves;
        if (piece != null)
        {
            PieceController pc = piece.pc;
            
            moves = pc.InitiateAiMove(this);
        } else
        {
            moves = new List<Board>();
        }
        //Debug.Log(moves.Count);
        return moves;
    }

    public void SetPosition(Piece obj)
    {
        PieceController pController = obj.pc;
        positions[pController.xBoard, pController.yBoard] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null; //possibly +1
    }

    public Piece GetPosition(int x, int y)
    {
        return positions[x, y]; //possibly +1
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public void printPositions()
    {
        string line = "";
        for (int i = 7; i > -1; i--)
        {
            line += "" + (i+1) + ": ";
            for (int j = 0; j < 8; j++)
            {
                if (positions[j, i] != null)
                {
                    line += "|" + positions[j, i].name + "|";
                }
                else
                {
                    line += "|______________|";
                }
            }
            line += "\n";
        }
        string space = "          ||         ";
        line += "    |         A" + space + "B" + space + "C" + space + "D" + space + "E" + space + "F" + space + "G" + space + "H" + "         |";
        Debug.Log(line);
    }

    public void Shuffle(Piece[] list)
    {
        System.Random rng = new System.Random();

        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Piece value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
