using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InventoryScript : MonoBehaviour {
    private GameObject gameManager;
    private ItemDatabaseScript database;

    public List<Clue> clueInventory = new List<Clue>();

	public List<Clue> loot = new List<Clue> ();

	// Use this for initialization
	void Start () {
        gameManager = GameObject.FindGameObjectWithTag("GameController");

        //On GameScene Load
        database = gameManager.GetComponent<ItemDatabaseScript>();

        //GenerateLoot();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I)) {
            OnDeath();
        }
	}

    public void AddClueToInventory(Clue a_clue) {
        clueInventory.Add(a_clue);
        Debug.Log("Clue Added!");
		Debug.Log(a_clue);
    }

	//On player death, generate 1-3 clues of random types
	public void OnDeath() {
		GenerateLoot ();
	}  
    
	void GenerateLoot() {
        loot.Clear();
        Debug.Log("/////////////////////////////////////");
        System.Random random = new System.Random();
        int numClues = UnityEngine.Random.Range (1, 4);
        IEnumerable<int> randNums = Enumerable.Range(0, 5).OrderBy(x => random.Next()).Take(numClues);
        int[] randomIndices = randNums.ToArray();

        //List<int> randomIndices = new List<int>();
        //while (randomIndices.Count < numClues) { 
        //    int randNum = Random.Range(0, 5);
        //    if (!randomIndices.Contains(randNum)) {
        //        randomIndices.Add(randNum);
        //    }
        //}

        for (int i = 0; i < numClues; ++i) {         
            Clue clueToAdd = ItemToClue(database.FetchItemByID(randomIndices[i]));
            loot.Add(clueToAdd);
            Debug.Log("Loot dropped: " + loot[i].item.type);
        }          
	}

    
	//Creates a clue using the base definition of an item listed in the database
	//and from the killer's attributes
	private Clue ItemToClue(Item a_item) {
		Clue clue = new Clue ();
		clue.item = a_item;
		//fill in clue.attributes with the killer's info.
		return clue;
	}
}