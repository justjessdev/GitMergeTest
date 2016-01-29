using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class DoorScript : MonoBehaviour {

	public SphereCollider trigger;

	public bool isProximity = true;

	[Range(2.0f, 10.0f)]
	public float proximityRange = 1.0f;
	
	private Animator anim;

    public bool doorOpen = false;
    public bool doorUnlocked = true;

    [HideInInspector]
    public ToolScript ToolScript;

	// Use this for initialization
	void Start () {
        //Find the game objects with the tag "Player", and retrieve the ToolScript
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ToolScript = player.GetComponent<ToolScript>();

		anim = GetComponent<Animator> ();
		
		GetComponent<NetworkAnimator> ().SetParameterAutoSend (0, true);
		
		if(trigger == null)
			trigger = GetComponent<SphereCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		trigger.radius = proximityRange;
		anim.SetBool ("isOpen", doorOpen);
	}

    void OnInteraction()
    {
		if (isProximity)
			return;
        if (doorOpen) CloseDoor();
        else OpenDoor();
    }

    void OnToolInteraction(GameObject other)
    {
		if (isProximity)
			return;
		Debug.Log("Tool Interaction");
		if (other.GetComponent<ToolScript>().wrench == true)
        {
            if (!doorOpen && doorUnlocked)
			{
				LockDoor();
			}
            else UnlockDoor();
        }
    }

    void OpenDoor()
    {
        print("Door is open");
        doorOpen = true;
    }

    void CloseDoor()
    {
        print("Door is closed");
        doorOpen = false;
    }

    void LockDoor()
    {
        print("Door is locked");
        doorUnlocked = false;
    }

    void UnlockDoor()
    {
        print("Door is unlocked");
        doorUnlocked = true;
    }

	void OnTriggerStay(Collider c)
	{
		if (!isProximity)
			return;
		if(c.CompareTag("Player"))
		{
			OpenDoor();
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if (!isProximity)
			return;
		if(c.CompareTag("Player"))
		{
			CloseDoor();
		}
	}
}
