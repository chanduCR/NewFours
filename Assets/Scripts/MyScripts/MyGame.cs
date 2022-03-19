using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using MyProject;

public class MyGame : MonoBehaviour
{
    [SerializeField]
    Text Log;
    [SerializeField]
    Game game;
    //int waitTime = 30;
    float waitedTime = 0;
    int quitTime = 5;
    int totalPlayers;

    MyPlayer localPlayer;
    MyPlayer otherPlayer;
    MyPlayer lastPlayer;

    bool gameStarted = false;
    

    void Start()
    {
        totalPlayers = FindObjectOfType<MyLoader>().TotalPlayers();
        StartCoroutine(waitForPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStarted)
        {
            if (isMyTurn())
            {
                Output("Your Turn");
                // enable effets
                if(Input.GetMouseButtonDown(0))
                {
                    // validate input
                    if(game.isValidInput())
                    {
                        localPlayer.ChangePosition(game.getCurrentCol());
                        if(game.playForPlayer())
                        {
                            Output("You Won");
                            gameStarted = false;
                            game.endGame();
                        }
                        else if(game.isDraw())
                        {
                            drawGame();
                        }
                        else
                        {
                            game.disableInput();
                            game.changePlayer();
                        }
                    }
                }
            }
            else
            {
                Output("Opponents Turn");
            }
        }
    }


    private IEnumerator waitForPlayer()
    {
        while (quitTime > waitedTime)
        {
            waitedTime += Time.deltaTime;
            if (FindObjectsOfType<MyPlayer>().Length > 0)
            {
                Output("Waiting for server");
                StartCoroutine(waitForOtherPlayer());
                yield break;
            }
            yield return null;
        }
        StartCoroutine(QuitApplication());
    }


    private IEnumerator QuitApplication()
    {
        waitedTime = 0;
        Output("Couldn't Connect to server closing application in " + quitTime + " secs.");
        while (quitTime > waitedTime)
        {
            waitedTime += Time.deltaTime;
            yield return null;
        }
        Application.Quit();
    }

    private IEnumerator waitForOtherPlayer()
    {
        while (FindObjectsOfType<MyPlayer>().Length != totalPlayers)
        {
            Output("waitng for other player(s).");
            yield return null;
        }
        Output("All Players Joined");
        GetOtherPlayerInfo();
    }

    public void Output(string msg)
    {
        if (Log.text != msg)
        {
            Log.text = msg;
        }
    }

    private void GetOtherPlayerInfo()
    {
        foreach(MyPlayer player in FindObjectsOfType<MyPlayer>())
        {
            player.registerHandler(this);
            if(player.IsLocalPlayer)
            {
                localPlayer = player;
            }
            else
            {
                otherPlayer = player;
            }
        }
        game.setPlayer(Tile.PLAYER1);
        // setting the game start config
        if (NetworkManager.Singleton.IsHost)
        {
            // host gets to play first
            lastPlayer = otherPlayer;
            game.enableInput();
        }
        else
        {
            // client waits till the game is setup
            lastPlayer = localPlayer;
            game.disableInput();
            //StartCoroutine(waitTillSetup());
        }
        // once the game is all setup, we start responding to player input accordingly
        gameStarted = true;
        game.startGame();
        //StartCoroutine(GameLoop());
    }
    

    // checking by client only so, opposite to host start config

    private IEnumerator GameLoop()
    {
        yield return null;
    }

    public void passTurn(MyPlayer player, int playersChoice)
    {
        lastPlayer = player;
        if(player.IsLocalPlayer)
        {
            return;
        }
        //Output(player.ToString());
        // when other players moves recived
        game.setCurrentCol(playersChoice);
        if(game.playForPlayer())
        {
            Output("You Lost");
            gameStarted = false;
            game.endGame();
        }
        else if(game.isDraw())
        {
            drawGame();
        }
        else
        {
            game.changePlayer();
            game.enableInput();
        }
        // update local machine
        // check for win or draw
        Debug.Log("recived control from other player");
    }

    public bool isMyTurn()
    {
        //return ((localPlayer.GetTurn() == true) && otherPlayer.GetTurn() == false));
        return lastPlayer != localPlayer;
    }

    public bool getStatus()
    {
        return gameStarted;
    }

    void drawGame()
    {
        Output("GameOver");
        gameStarted = false;
        game.endGame();
    }
}
