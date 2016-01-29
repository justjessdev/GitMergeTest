using UnityEngine;
using System.Collections;

public class PowerScript : MonoBehaviour {

    public bool powerActive = true;

    [HideInInspector]
    public ToolScript ToolScript;

	// Use this for initialization
	void Start () {
        //Find the game objects with the tag "Player", and retrieve the ToolScript
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ToolScript = player.GetComponent<ToolScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnToolInteraction()
    {
        if (ToolScript.pliers == true)
        {
            if (powerActive) PowerOff();
            else PowerOn();
        }
    }

    void PowerOn()
    {
        print("Power is on");
        powerActive = true;
    }

    void PowerOff()
    {
        print("Power is off");
        powerActive = false;
    }
}
