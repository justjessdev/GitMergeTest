using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManagerScript : MonoBehaviour {
    
    static public GameManagerScript Instance;
    
    static public bool isActive { 
    	get {
             return Instance != null; 
    	} 
    }
    
    static public GameManagerScript instance
    {
    	get
    	{
             if (Instance == null)
    		{
                 Instance = Object.FindObjectOfType(typeof(GameManagerScript)) as GameManagerScript;
    
                 if (Instance == null)
    			{
    				GameObject go = new GameObject("_gamemanager");
    				DontDestroyOnLoad(go);
                    Instance = go.AddComponent<GameManagerScript>();
    			}
    		}
             return Instance;
    	}
    }

    /////
    //Global Game State variables to keep track of game progress
    public enum GameState { waitingForPlayers, countDown, gameRunning, gameFinished };

    [Tooltip("Global gamestate for the running game")]
    public GameState gameState = GameState.waitingForPlayers;

    //List of players for the game manager to keep track of
    [Tooltip("Global list of players for the GameManager to keep track of (Auto-Populates)")]
    public List<GameObject> players;

    //Timer for the game
    [HideInInspector]
    //[Tooltip("Timer for the game")]
    public int gameTimer = -1;

    //Starting value for the timer, 54000 = 15 minutes
    [Tooltip("Starting value for the timer, 54000 = 15 minutes")]
    public int gameTimerStart = 54000;

    ////Holds a reference to the game's killer
    //[HideInInspector]
    //public GameObject killerPlayer;

    //Timer for the countdown
    [HideInInspector]
    //[Tooltip("Timer for the countdown")]
    public int countdownTimer = -1;

    //Starting value for the countdown, 1800 = 30 seconds
    [Tooltip("Starting value for the countdown, 1800 = 30 seconds")]
    public int countdownTimerStart = 300;

    //Whether the Game Manager has been initialized or not
    private bool m_isInitialized = false;

    void FindPlayers()
    {
        players.Clear();
        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject thisPlayer in foundPlayers)
        {
            players.Add(thisPlayer);
        }

        //Debug.Log("players in list" + players.Count);
    }
    void OnLevelWasLoaded(int level)
    {
        if (level == 4)
            gameState = GameState.countDown;

    }

    void Initialize()
    {
        if (!m_isInitialized)
        {
            m_isInitialized = true;

            FindPlayers();

            gameTimer = gameTimerStart;
            countdownTimer = countdownTimerStart;
        }
	}
	
	// Update is called once per frame
    void Update()
    {
        //Countdown game state
        if (gameState == GameState.countDown)
        {
            if (!m_isInitialized) Initialize();

            --countdownTimer;
            if (countdownTimer <= 0)
            {
                gameState = GameState.gameRunning;
                countdownTimer = countdownTimerStart;

                FindPlayers();

                //int killer = Random.Range(0, players.Count);
                //
                ///////////////////////////////////
                ////TEMPORARILY MAKE THE FIRST PLAYER ALWAYS THE KILLER
                //killer = 0;
                ///////////////////////////////////
                //
                //for (int i = 0; i < players.Count; ++i)
                //{
                //    PlayerClassScript classScript = players[i].GetComponent<PlayerClassScript>();
                //
                //    if (i == killer) classScript.KillerRole();
                //    else classScript.VictimRole();
                //}
                //
                //killerPlayer = players[killer];
                //players[killer].AddComponent<KillerScript>();

            }
        }
        //Running game state
        else if (gameState == GameState.gameRunning)
        {
            --gameTimer;
            if (gameTimer <= 0)
            {
                gameState = GameState.gameFinished;
                gameTimer = gameTimerStart;
            }
        } 
	}
}
