//EquipmentTrap.cs
///////////////////////////////////////

using UnityEngine;
using System.Collections;

public class EquipmentTrap : MonoBehaviour {

    private bool trapSet;
    private bool trapUsed;

	// Use this for initialization
	void Start () {
        trapSet = false;
        trapUsed = false;
	}
	
    public void OnInteraction() {
        if (!trapUsed)
        {
            Debug.Log("Equipment Trap Activated");
            trapSet = true;
        }
        //TODO: Start Equipment visual cue
    }

    void OnTriggerEnter(Collider player) {
        if (trapSet)
        {
            //TODO: Kill the single player
            Debug.Log("YOU DIED: Equipment Trap");
            trapSet = false;
            trapUsed = true;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
