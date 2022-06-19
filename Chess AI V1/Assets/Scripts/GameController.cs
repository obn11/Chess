using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Implement En Pisent
    // Implement Castleing
    // Implement Pawn Promotion

    public GameObject chesspiece;

    [SerializeField]
    public GameObject[,] positions = new GameObject[8, 8];
    [SerializeField]
    public List<GameObject> blackPieces = new List<GameObject>();
    [SerializeField]
    public List<GameObject> whitePieces = new List<GameObject>();

    public string player = "white";
    public string Ai = "black";

    public MiniMax minimax;

    [SerializeField]
    public enum PlayMode { Manual, Minimax }
    public PlayMode currentMode = PlayMode.Minimax;
    public enum BoardMode { Normal, Random }
    public BoardMode currentBoardMode = BoardMode.Normal;
    public int turn = 0;


    //[SerializeField]
    //List<GameObject> squares;

    private string currentPlayer = "white";

    public bool gameOver = false;
    public string winner;

    public void printPositions()
    {
        string line = "";
        for (int i = 7; i > -1; i--)
        {
            line += "" + (i + 1) + ": ";
            for (int j = 0; j < 8; j++)
            {
                if (positions[j, i])
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


    // Start is called before the first frame update
    void Start()
    {
        whitePieces = new List<GameObject>()
        {
            Create("white_rook", 0, 0), Create("white_knight", 1, 0), Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0),
            Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0), Create("white_pawn", 0, 1), Create("white_pawn", 1, 1),
            Create("white_pawn", 2, 1), Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1), Create("white_pawn", 6, 1),
            Create("white_pawn", 7, 1)
        };
        blackPieces = new List<GameObject>()
        {
            Create("black_rook", 0, 7), Create("black_knight", 1, 7), Create("black_bishop", 2, 7), Create("black_queen", 3, 7), Create("black_king", 4, 7),
            Create("black_bishop", 5, 7), Create("black_knight", 6, 7), Create("black_rook", 7, 7), Create("black_pawn", 0, 6), Create("black_pawn", 1, 6),
            Create("black_pawn", 2, 6), Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6), Create("black_pawn", 6, 6), 
            Create("black_pawn", 7, 6),
            
        };
        if (currentBoardMode == BoardMode.Random)
        {
            ShufflePieces();
        }
        for (int i = 0; i < whitePieces.Count; i++)
        {
            SetPosition(blackPieces[i]);
            SetPosition(whitePieces[i]);
        }
        //printPositions();
    }

    private GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        PieceController pController = obj.GetComponent<PieceController>();
        pController.name = name;
        pController.xBoard = x;
        pController.yBoard = y;
        pController.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        PieceController pController = obj.GetComponent<PieceController>();
        positions[pController.xBoard, pController.yBoard] = obj;
    }

    public GameObject SetPositions(Piece[,] pieces)
    {
        GameObject newPiece = new GameObject();
        if (pieces == null)
        {
            return newPiece;    
        }
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] != null)

                {
                    if (positions[i, j])
                    {
                        if (pieces[i, j].name != positions[i, j].name)
                        {
                            GameObject piece1 = GetPosition(i, j);
                            Destroy(piece1);
                            SetPositionEmpty(i, j);
                            GameObject piece = Create(pieces[i, j].name, i, j);
                            SetPosition(piece);
                            piece.GetComponent<PieceController>().SetChoords();
                            newPiece = piece;
                        }
                    }
                    else
                    {
                        GameObject piece = Create(pieces[i, j].name, i, j);
                        SetPosition(piece);
                        piece.GetComponent<PieceController>().SetChoords();
                        newPiece = piece;
                    }
                    string player = positions[i, j].name.Substring(0,5);
                    if (player == "white")
                    {
                        if (!whitePieces.Contains(positions[i, j])) whitePieces.Add(positions[i, j]);
                    } else if (player == "black")
                    {
                        if (!blackPieces.Contains(positions[i, j])) blackPieces.Add(positions[i, j]);
                    } else
                    {
                        Debug.Log("Player: " + player);
                    }
                }
                else
                {
                    if (positions[i, j])
                    {
                        GameObject piece1 = GetPosition(i, j);
                        Destroy(piece1);
                    }
                    SetPositionEmpty(i, j);
                }
            }
        }
        CleanWhiteBlackPieces();
        return newPiece;
    }

    public void SetWhiteBlackPieces(Piece[] tWhitePieces, Piece[] tBlackPieces, GameObject newPiece)
    {
        for (int i = 0; i < 16; i++)
        {
            if ((!blackPieces[i]) || blackPieces[i].name != tBlackPieces[i].name)
            {
                blackPieces[i] = newPiece;
            }
            if ((!whitePieces[i]) || whitePieces[i].name != tWhitePieces[i].name)
            {
                whitePieces[i] = newPiece;
            }
        }
    }

    public void CleanWhiteBlackPieces()
    {
        for (int i = (whitePieces.Count - 1); i >= 0; i--) {  
            if (whitePieces.Count > 16 && i < whitePieces.Count && i > 0 && whitePieces[i] == null) whitePieces.RemoveAt(i);
        }
        for (int i = (blackPieces.Count - 1); i >= 0; i--) {

            if (blackPieces.Count > 16 && i < blackPieces.Count && i > 0 && blackPieces[i] == null) blackPieces.RemoveAt(i);
        }
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null; //possibly +1
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y]; //possibly +1
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
            if (currentMode == PlayMode.Minimax)
            {
                //Board board = new Board(this).Clone();
                //Piece piece = board.whitePieces[1];
                //Debug.Log(piece.name);
                //List<Board> moves = board.allMoves(true);
                //foreach (Board move in moves)
                //{
                //    move.printPositions();
                //}

                StartCoroutine(CallMinimax());
            }
        } 
        else
        {
            //Board board = new Board(this);
            //Debug.Log("gameOver = " + board.isGameOver());
            currentPlayer = "white";
        }
    }
    public void printPieces()
    {
        string str = "White Pieces: ";
        foreach (GameObject piece in whitePieces)
        {
            if (piece) str += piece.name + ", ";
        }
        str += "\nBlack Pieces: ";
        foreach (GameObject piece in blackPieces)
        {
            if (piece) str += piece.name + ", ";
        }
        Debug.Log(str);
    }

    IEnumerator CallMinimax()
    {
        yield return new WaitForSeconds(0.1f);
        Board bestmove = minimax.CallMinimax(false);
        //bestmove.printPositions();
        GameObject newPiece = SetPositions(bestmove.GetPositions());
        //printPositions();
        //SetWhiteBlackPieces(bestmove.whitePieces, bestmove.blackPieces, newPiece);
        NextTurn();
    }  

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
        if (!containsName(blackPieces, "black_king"))
        {
            Winner("white");
        } 
        else if (!containsName(whitePieces, "white_king"))
        {
            Winner("black");
        }
    }

    bool containsName(List<GameObject> listy, string name)
    {
        foreach (GameObject obj in listy)
        {
            if (obj)
            {
                if (name == obj.name)
                    return true;
            }
        }
        return false;
    }

    public void ShufflePieces()
    {
        System.Random rng = new System.Random();

        
        int n = whitePieces.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject piece = whitePieces[k];
            if (whitePieces[k].name != "white_king" && whitePieces[k].name != "white_pawn" && whitePieces[n].name != "white_king" && whitePieces[n].name != "white_pawn")
            {
                int txBoard = piece.GetComponent<PieceController>().xBoard;
                int tyBoard = piece.GetComponent<PieceController>().yBoard;
                piece.GetComponent<PieceController>().xBoard = whitePieces[n].GetComponent<PieceController>().xBoard;
                piece.GetComponent<PieceController>().yBoard = whitePieces[n].GetComponent<PieceController>().yBoard;
                whitePieces[n].GetComponent<PieceController>().xBoard = txBoard;
                whitePieces[n].GetComponent<PieceController>().yBoard = tyBoard;
                whitePieces[k].GetComponent<PieceController>().SetChoords();
                whitePieces[n].GetComponent<PieceController>().SetChoords();
            }
        }

        n = blackPieces.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject piece = blackPieces[k];
            if (blackPieces[k].name != "black_king" && blackPieces[k].name != "black_pawn" && blackPieces[n].name != "black_king" && blackPieces[n].name != "black_pawn")
            {
                int txBoard = piece.GetComponent<PieceController>().xBoard;
                int tyBoard = piece.GetComponent<PieceController>().yBoard;
                piece.GetComponent<PieceController>().xBoard = blackPieces[n].GetComponent<PieceController>().xBoard;
                piece.GetComponent<PieceController>().yBoard = blackPieces[n].GetComponent<PieceController>().yBoard;
                blackPieces[n].GetComponent<PieceController>().xBoard = txBoard;
                blackPieces[n].GetComponent<PieceController>().yBoard = tyBoard;
                blackPieces[k].GetComponent<PieceController>().SetChoords();
                blackPieces[n].GetComponent<PieceController>().SetChoords();
            }
        }

    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        winner = playerWinner;

        Text winText = GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>();
        winText.enabled = true;
        winText.text = playerWinner + " wins!";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;

    }
}
