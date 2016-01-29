using UnityEngine;
using System.Collections;

public class CorpseScript : MonoBehaviour {

    public bool clueAvailable = true;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnInteraction()
    {
        if (clueAvailable) RevealClue();
        else return;
    }

    void RevealClue()
    {
        print("A clue has been found!");
        clueAvailable = false;
    }
}
