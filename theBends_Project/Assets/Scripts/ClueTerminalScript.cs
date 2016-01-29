using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClueTerminalScript : MonoBehaviour {

    private Rect terminalMainWnd = new Rect(Screen.width / 2 - ((Screen.width - 200) / 2), Screen.height / 2 - ((Screen.height - 200) / 2), Screen.width - 200, Screen.height - 200);
    
    //tie bool into toggle function, run function when the player searches a body
    public bool render = false;
    public int terminalSTATE = 0;
    public int numCluesGathered = 0;

    public List<Clue> submittedClues = new List<Clue>();

    public void OnGUI() {
        if (render)
            terminalMainWnd = GUI.Window(0, terminalMainWnd, WindowFunction, "Accuse the Killer!");
        
    }

    void WindowFunction(int windowID) {
        //main terminal window
        if (terminalSTATE == 0) {
            if (GUI.Button(new Rect(80, 100, 200, 200), "Submit Clue(s)")) {
                //Run LogClue()
                Debug.Log("Submitting your clues!");
                terminalSTATE = 1;
            }

            if (GUI.Button(new Rect(330, 100, 200, 200), "Clue Log")) {
                terminalSTATE = 1;
            }

            GUI.enabled = false;
            if (numCluesGathered >= 5)
                GUI.enabled = true;
            if (GUI.Button(new Rect(580, 100, 200, 200), "Accuse!")) {
                MakeAccusation();
            }

            GUI.enabled = true;
            if (GUI.Button(new Rect(725, 350, 100, 50), "Quit")) {
                render = false;
            }

        //clue log window - displays all submitted clues
        } else if (terminalSTATE == 1) {
            GUI.Label(new Rect(Screen.width / 2 - 150, 40, 300, 50), "Viewing all submitted clues");

            if (GUI.Button(new Rect(725, 350, 100, 50), "Back")) {
                terminalSTATE = 0;
            }

        //accuse window - allows you to accuse a player other than yourself
        } else if (terminalSTATE == 2) {
            if (GUI.Button(new Rect(75, 75, 100, 100), "Player1")) {
                Debug.Log("You have accused Player1 of murder!");
            }
            if (GUI.Button(new Rect(275, 75, 100, 100), "Player2")) {
                Debug.Log("You have accused Player2 of murder!");
            }
            if (GUI.Button(new Rect(475, 75, 100, 100), "Player3")) {       
                Debug.Log("You have accused Player3 of murder!");
            }
            if (GUI.Button(new Rect(675, 75, 100, 100), "Player4")) {
                Debug.Log("You have accused Player4 of murder!");
            }
            if (GUI.Button(new Rect(175, 250, 100, 100), "Player5")) {
                Debug.Log("You have accused Player5 of murder!");
            }
            if (GUI.Button(new Rect(375, 250, 100, 100), "Player6")) {
                Debug.Log("You have accused Player6 of murder!");
            }
            if (GUI.Button(new Rect(575, 250, 100, 100), "Player7")) {
                Debug.Log("You have accused Player7 of murder macaque!");
            }

            if (GUI.Button(new Rect(725, 350, 100, 50), "Back")) {
                terminalSTATE = 0;
            }
        }

        
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Player can choose to submit any clues they have gathered
    void LogClue() { 
        //get the player's ID who interacted with the terminal
        //loop through player's clueInventory List to determine if they have a clue to submit
        //if they do, log it into the terminal
        //Check win condition
            //have the survivors gathered all the clues yet?
                //if yes, run MakeAccusation()
    }

    //once players have enough clues they will be able to make a guess as to which player is the killer
    void MakeAccusation() { 
        //bring up Accusation window as well as a window next to it displaying all the clue information
        terminalSTATE = 2;
    }

    //Sort function to order clues in the terminal
    void SortClues() { 
        //run some sort of List.sort()
    }
}
