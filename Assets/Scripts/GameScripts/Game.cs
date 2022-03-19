using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyProject;

public class Game : MonoBehaviour
{
    [SerializeField]
    TileColumn[] columns;
    [SerializeField]
    Tile CurrentPlayerImage;
    [SerializeField]
    TileColumn Helper;

    int X;
    int Y;
    int TileCount;

    int[,] GameAry;

    //
    int CurrentPlayer;
    int CurrentColumn = -1;
    int oldCurrentColumn;
    bool myTurn;
    bool gameStatus;

    // Start is called before the first frame update
    void Start()
    {
        X = columns.Length;
        Y = columns[0].GetSize();
        GameAry = new int[X, Y];
        TileCount = X * Y;
        gameStatus = false;
    }

    // Update is called once per frame

    public void changePlayer()
    {
        if(CurrentPlayer == Tile.PLAYER1)
        {
            setPlayer(Tile.PLAYER2);
        }
        else
        {
            setPlayer(Tile.PLAYER1);
        }
    }

    public void startGame()
    {
        gameStatus = true;
    }

    public bool playForPlayer()
    {
        TileCount--;
        TileColumn col = columns[CurrentColumn];
        int tempY = col.GetPointer();
        GameAry[CurrentColumn, tempY] = CurrentPlayer;
        col.PlaceTile(CurrentPlayer);
        return CheckPlayer(CurrentColumn, tempY);
    }

    public bool isDraw()
    {
        return TileCount == 0;
    }

    public void enableInput()
    {
        myTurn = true;
    }

    public void disableInput()
    {
        myTurn = false;
        Helper.GetTile(CurrentColumn).HideSprite();
    }

    public bool isValidInput()
    {
        if(CurrentColumn != -1)
        {
            if(columns[CurrentColumn].GetPointer() < Y)
            {
                return true;
            }
        }
        return false;
    }

    public int getCurrentCol()
    {
        return CurrentColumn;
    }

    public void setCurrentCol(int col)
    {
        oldCurrentColumn = CurrentColumn;
        CurrentColumn = col;
    }
    public void endGame()
    {
        CurrentPlayerImage.HideSprite();
        gameStatus = false;
    }

    public void setPlayer(int player)
    {
        CurrentPlayer = player;
        CurrentPlayerImage.SetPlayerImage(player);
    }

    /*
    public void play()
    {
        //CurrentPlayerImage.SetPlayerImage(CurrentPlayer);
        if (myGame.getStatus())
        {
            if (CurrentColumn != -1)
            {
                TileColumn col = columns[CurrentColumn];
                int tempY = col.GetPointer();
                if (tempY < Y)
                {
                    col.PlaceTile(CurrentPlayer);
                    TileCount--;
                    GameAry[CurrentColumn, tempY] = CurrentPlayer;
                    // check if player won
                    if (CheckPlayer(CurrentColumn, tempY))
                    {
                        displayText.text = "You Won";
                        myGame.endGame();
                        Helper.ClearTile(CurrentColumn);
                        //Application.Quit();
                    }
                    else if (TileCount > 0)
                    {
                        // channge player
                        CurrentPlayer %= 2;
                        CurrentPlayer++;
                        CurrentPlayerImage.SetPlayerImage(CurrentPlayer);
                        // change player in helper section also
                        Helper.SetTile(CurrentColumn, CurrentPlayer);
                        // Reduce Count
                    }
                    else
                    {
                        // game over draw
                        displayText.text = "Game Draw";
                        CurrentPlayerImage.HideSprite();
                        Helper.ClearTile(CurrentColumn);
                        myGame.endGame();
                    }
                }
            }
        }
    }
    */

    private bool CheckPlayer(int x, int y)
    {
        //return CheckVertical(x) || CheckHorizontal(y);
        return CheckNegativeSlope(x, y) || CheckPositiveSlope(x, y) || CheckVertical(x) || CheckHorizontal(y);
    }

    private bool CheckVertical(int x)
    {
        int count = 0;
        for (int y = 0; y < Y; y++)
        {
            if (GameAry[x, y] == CurrentPlayer)
            {
                count++;
                if (count == 4)
                {
                    return true;
                }
            }
            else if (GameAry[x, y] == 0)
            {
                return false;
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    private bool CheckHorizontal(int y)
    {
        int count = 0;
        for (int x = 0; x < X; x++)
        {
            if (GameAry[x, y] == CurrentPlayer)
            {
                count++;
                if (count == 4)
                {
                    return true;
                }
            }
            else
            {
                count = 0;
            }
        }
        return false;
    }

    private bool CheckNegativeSlope(int tx, int ty)
    {
        int x;
        int y;
        int count = 0;
        if (tx + ty < X)
        {
            x = tx + ty;
            y = 0;
        }
        else
        {
            x = X - 1;
            y = ty + tx - X + 1;
        }
        while ((x >= 0) && (y < Y))
        {
            //Debug.Log(x + "," + y);
            if (GameAry[x, y] == CurrentPlayer)
            {
                count++;
                if (count == 4)
                {
                    return true;
                }
            }
            else
            {
                count = 0;
            }
            x--;
            y++;
        }
        return false;
    }

    private bool CheckPositiveSlope(int tx, int ty)
    {
        int x = 0;
        int y = 0;
        int count = 0;
        if (tx < ty)
        {
            x = 0;
            y = ty - tx;
        }
        else
        {
            x = tx - ty;
            y = 0;
        }
        while ((x < X) && (y < Y))
        {
            if (GameAry[x, y] == CurrentPlayer)
            {
                count++;
                if (count == 4)
                {
                    return true;
                }
            }
            else
            {
                count = 0;
            }
            x++;
            y++;
        }
        return false;
    }

    private void OnColumnHelper(int x)
    {
        CurrentColumn = x;
        if (myTurn && gameStatus)
        {
            Helper.SetTile(x, CurrentPlayer);
        }
    }


    // very bad implementation
    public void OnEnter0()
    {
        OnColumnHelper(0);
    }
    public void OnEnter1()
    {
        OnColumnHelper(1);
    }
    public void OnEnter2()
    {
        OnColumnHelper(2);
    }
    public void OnEnter3()
    {
        OnColumnHelper(3);
    }
    public void OnEnter4()
    {
        OnColumnHelper(4);
    }
    public void OnEnter5()
    {
        OnColumnHelper(5);
    }
    public void OnEnter6()
    {
        OnColumnHelper(6);
    }
    public void OnExitHelper()
    {
        if (myTurn)
        {
            Helper.GetTile(CurrentColumn).HideSprite();
        }
        CurrentColumn = -1;
    }

    // unused methods
    /*
    private void Test()
    {
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Y; y++)
            {
                //Debug.Log(((x + y) % 2) + 1);
                GetTile(x, y).SetPlayerImage(((x + y) % 2) + 1);
            }
        }
    }

    private Tile GetTile(int x, int y)
    {
        return columns[x].GetTile(y);
    }
    */
}
