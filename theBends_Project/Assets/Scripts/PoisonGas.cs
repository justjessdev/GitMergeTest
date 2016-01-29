//PoisonGas.cs
/////////////////////////////////////

using UnityEngine;
using System.Collections;

public class PoisonGas : MonoBehaviour {

    private float deathClock = 20;

    private bool trapSet;

    private bool isPlayerInArea;

	// Use this for initialization
	void Start () {
        trapSet = false;
        isPlayerInArea = false;
    }


    public void OnInteraction()
    {
        trapSet = true;
        Debug.Log("Poison Gas Activated!");
    }

    void OnTriggerEnter(Collider collider)
    {
        //TODO: Need player's camera for gas image effects
        if (trapSet)
        {
            //Add GasTimer for countdown
            GameObject player = collider.gameObject;
            if (!player.GetComponent<GasTimer>())
            {
                player.AddComponent<GasTimer>();
            }
            else
            {
                player.GetComponent<GasTimer>().isPlayerInGas = true;
            }
            
            Debug.Log(player.name + " entered the poison gas");
            isPlayerInArea = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        //TODO: Remove gas image effects
        //Reverse countdown timer
        if (trapSet)
        {
            GameObject player = collider.gameObject;
            if (player.GetComponent<GasTimer>())
            {
                player.GetComponent<GasTimer>().isPlayerInGas = false;
            }
            isPlayerInArea = false;
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
