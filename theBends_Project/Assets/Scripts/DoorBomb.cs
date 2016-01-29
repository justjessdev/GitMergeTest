//DoorBomb.cs
/////////////////////////////////////////

using UnityEngine;
using System.Collections;

//NOTE: When placing SphereCollider, position the transform and scale on RADIUS of the collider

public class DoorBomb : MonoBehaviour {

    private bool trapSet;
    private bool trapUsed;

	// Use this for initialization
	void Start () {
        trapSet = false;
        trapUsed = false;
	}

    public void OnInteraction() {
        if (!trapSet && !trapUsed)
        {
            //TODO: Start Door Bomb visual cue
            trapSet = true;
            Debug.Log("Door Bomb Activated");
        }
        else
        {
            //TODO: Create a Player layer. OverlapSphere can check by layer
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);

            //Loop through all players in the radius
            for (int i = 0; i < hitColliders.Length; ++i)
            {
                //TODO: Kill the players
                Debug.Log(hitColliders[i].name);
            }

            Debug.Log("YOU DIED: Door Bomb");
            trapUsed = true;

        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
