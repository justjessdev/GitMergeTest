using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkGameLogicManager_AssignKiller : NetworkBehaviour {

    //bool killerAssigned = false;

    [Command]
    public void CmdTellServerWhoTheKillerIs(string uniquePlayerID)
    {
        GameObject player = GameObject.Find(uniquePlayerID);
        player.GetComponent<PlayerClassScript>().playerRole = PlayerClassScript.PlayerRole.Killer;
        player.AddComponent<KillerScript>();
    }

    // Use this for initialization
    void Start () {
	
	}
    
    // Update is called once per frame
    void Update () {
        
	}
}
