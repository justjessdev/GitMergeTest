using UnityEngine;
using System.Collections;

public class LootWindowScript : MonoBehaviour {

    private Rect windowRect = new Rect(Screen.width / 2 - 350, Screen.height / 2 - 100, 700, 200);

	//tie bool into toggle function, run function when the player searches a body
	public bool render = false;

	public void OnGUI () {
		if(render)
			windowRect = GUI.Window (0, windowRect, WindowFunction, "Loot Window");
	}

	//TODO: create search function
	void WindowFunction(int windowID) {
		if (GUI.Button (new Rect (20, 50, 100, 100), "Clothing Fiber")) {
			StartCoroutine(SearchForClue("Player found a Clothing Fiber!"));
			//access dead player's InventoryScript "loot" list, check to see if it contains the "Clothing Fiber" ID
			//if so player has x% chance to add the clue to their inventory - run search function
				//delete the clue from the loot list
			//otherwise output a message that states that your search yielded nothing
		}
		
		if (GUI.Button (new Rect (160, 50, 100, 100), "Strand of Hair")) {
			StartCoroutine(SearchForClue("Player found a Strand of Hair!"));
			//access dead player's InventoryScript "loot" list, check to see if it contains the "Strand of Hair" ID
			//if so player has x% chance to add the clue to their inventory - run search function
				//delete the clue from the loot list
			//otherwise output a message that states that your search yielded nothing
		}
		
		if (GUI.Button (new Rect (300, 50, 100, 100), "Shoeprint")) {
			StartCoroutine(SearchForClue("Player found a Shoeprint!"));
			//access dead player's InventoryScript "loot" list, check to see if it contains the "Shoeprint" ID
			//if so player has x% chance to add the clue to their inventory - run search function
				//delete the clue from the loot list
			//otherwise output a message that states that your search yielded nothing
		}
		
		if (GUI.Button (new Rect (440, 50, 100, 100), "Blood")) {
			StartCoroutine(SearchForClue("Player found some Blood!"));
			//access dead player's InventoryScript "loot" list, check to see if it contains the "Blood" ID
			//if so player has x% chance to add the clue to their inventory - run search function
				//delete the clue from the loot list
			//otherwise output a message that states that your search yielded nothing
		}

		if (GUI.Button (new Rect (580, 50, 100, 100), "Killer's Class")) {
			StartCoroutine(SearchForClue("Player identified the Killer's Class!s"));
			//access dead player's InventoryScript "loot" list, check to see if it contains the "Killer's Class" ID
			//if so player has x% chance to add the clue to their inventory - run search function
				//delete the clue from the loot list
			//otherwise output a message that states that your search yielded nothing
		}
	}

    //After 5 seconds the player has an 80% chance to get 
	IEnumerator SearchForClue(string a_message) {
        float chanceToLoot = Random.value;
		Debug.Log("Player is searching for clues.");
		yield return new WaitForSeconds(5);
        if (chanceToLoot < 0.8) {
            Debug.Log(a_message);
        } else {
            Debug.Log("Your search failed!");
        }		
	}
}
