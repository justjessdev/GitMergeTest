using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClueScript : MonoBehaviour {

    public Clue clue = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnInteraction(GameObject other) {
        InventoryScript playerInventory = other.gameObject.GetComponent<InventoryScript>();
        if (playerInventory != null) {
            //playerInventory.AddClue(clue);
        }
    }
}
