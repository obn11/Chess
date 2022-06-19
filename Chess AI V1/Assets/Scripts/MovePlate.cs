using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    //BoardPos
    int matrixX;
    int matrixY;

    //false: movement, true attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            //change to red
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0f, 0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<GameController>().GetPosition(matrixX, matrixY);

            if (cp.name == "white_king") controller.GetComponent<GameController>().Winner("black");
            if (cp.name == "black_king") controller.GetComponent<GameController>().Winner("white");

            Destroy(cp);
        }

        GameController gc = controller.GetComponent<GameController>();
        PieceController refController = reference.GetComponent<PieceController>();
        gc.SetPositionEmpty(refController.xBoard, refController.yBoard);

        refController.xBoard = matrixX;
        refController.yBoard = matrixY;
        refController.SetChoords();

        gc.SetPosition(reference);

        MiniMax minimax = controller.GetComponent<MiniMax>();
        Debug.Log("turn: " + gc.turn + " score: " + minimax.EvalMaterial(new Board(gc).GetPositions()));

        gc.turn += 1;
        refController.DestroyMovePlates();
        gc.NextTurn();
    }

    public void SetChoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
