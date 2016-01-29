using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_IsKillerOrVictimTextScript : NetworkBehaviour {

    Text text;
	// Use this for initialization
	void Start () {
        text = GameObject.Find("Text_KillerOrVictim").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!isLocalPlayer) {
            return;
        }

        text.text = gameObject.GetComponent<PlayerClassScript>().playerRole.ToString();

	}
}
