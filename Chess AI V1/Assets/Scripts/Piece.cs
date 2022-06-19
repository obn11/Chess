using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece
{
    public string name;
    public PieceController pc;

    public Piece(string tname, PieceController tpc)
    {
        name = tname;
        pc = tpc;
    }

    public Piece()
    {

    }

    public Piece Clone()
    {
        Piece clone = new Piece(name, pc);
        return clone;
    }
}
