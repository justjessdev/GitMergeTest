using UnityEngine;
using System.Collections;

public class SecurityDoorScript : MonoBehaviour {

    public bool securityDoorClosed = true;

    [HideInInspector]
    public ToolScript toolScript;

	// Use this for initialization
	void Start () {
        //Find the game objects with the tag "Player", and retrieve the ToolScript
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        toolScript = player.GetComponent<ToolScript>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnToolInteraction()
    {
        if (toolScript.accessCard == true)
        {
            if (securityDoorClosed) OpenDoor();
            else CloseDoor();
        }
    }

    void OpenDoor()
    {
        print("Security door is open");
        securityDoorClosed = false;
    }

    void CloseDoor()
    {
        print("Security door is closed");
        securityDoorClosed = true;
    }
}
