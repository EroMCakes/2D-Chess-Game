using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    // References
    public GameObject controller;
    public GameObject movePlate;

    // Positions
    private int xBoard = -1;
    private int yBorad = -1;

    // Variable to keep track of "black" or "white" player
    private string player = "";

    // Reference for all the sprites taht the chesspiece can be
    public Sprite black_queen, black_knight, black_bishop, black_rook, black_pawn, black_king;
    public Sprite white_queen, white_knight, white_bishop, white_rook, white_pawn, white_king;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        // Take the instantiated location and adjust the transform
        SetCoords();

        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_rook": this.GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_rook": this.GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBorad;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBorad;
    }

    public void SetXboard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBorad = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsCheckMate() && controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
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

    public void InitiateMovePlates()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0);
                LineMovePlate(1, 1);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(-1, 1);
                LineMovePlate(0, -1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0);
                LineMovePlate(-1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(0, -1);
                break;
            case "black_pawn":
                if (this.GetComponent<Chessman>().GetYBoard() == 6)
                {
                    PawnMovePlate(xBoard, yBorad - 2);
                    PawnMovePlate(xBoard, yBorad - 1);
                }
                else
                    PawnMovePlate(xBoard, yBorad - 1);
                break;
            case "white_pawn":
                if (this.GetComponent<Chessman>().GetYBoard() == 1)
                {
                    PawnMovePlate(xBoard, yBorad + 2);
                    PawnMovePlate(xBoard, yBorad + 1);
                }
                else
                    PawnMovePlate(xBoard, yBorad + 1);
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBorad + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<Chessman>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBorad + 2);
        PointMovePlate(xBoard - 1, yBorad + 2);
        PointMovePlate(xBoard + 2, yBorad + 1);
        PointMovePlate(xBoard - 2, yBorad + 1);
        PointMovePlate(xBoard + 2, yBorad - 1);
        PointMovePlate(xBoard - 1, yBorad - 2);
        PointMovePlate(xBoard - 2, yBorad - 1);
        PointMovePlate(xBoard + 1, yBorad - 2);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard + 1, yBorad);
        PointMovePlate(xBoard - 1, yBorad);
        PointMovePlate(xBoard + 1, yBorad + 1);
        PointMovePlate(xBoard - 1, yBorad + 1);
        PointMovePlate(xBoard + 1, yBorad - 1);
        PointMovePlate(xBoard - 1, yBorad - 1);
        PointMovePlate(xBoard, yBorad + 1);
        PointMovePlate(xBoard, yBorad - 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();

        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null && 
                sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(xBoard + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null && 
                sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            {
                MovePlateAttackSpawn(xBoard - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x *= 0.66f;
        y *= 0.66f;

        x += -2.3f;
        y += -2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(x, y, -3f), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
