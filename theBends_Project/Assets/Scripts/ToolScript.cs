using UnityEngine;
using System.Collections;

public class ToolScript : MonoBehaviour {

    [HideInInspector]
    public bool wrench, pliers, databasePass, syringe, bioScanner, accessCard, toxinDetector, knife, axe;

    [HideInInspector]
    public PlayerClassScript playerClassScript;

	// Use this for initialization
	void Start () 
    {
        playerClassScript = gameObject.GetComponent<PlayerClassScript>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        ToolFunctionality();
	}

    //Sets the tool for each player based on their class
    public void ToolSetter()
    {
        if (playerClassScript.playerClass == PlayerClassScript.PlayerClass.Mechanic)
        {
            wrench = true;
            pliers = true;
            databasePass = false;
            syringe = false;
            bioScanner = false;
            accessCard = false;
            toxinDetector = false;
            knife = false;
            axe = false;
        }
        if (playerClassScript.playerClass == PlayerClassScript.PlayerClass.Manager)
        {
            wrench = false;
            pliers = false;
            databasePass = true;
            syringe = false;
            bioScanner = false;
            accessCard = true;
            toxinDetector = false;
            knife = false;
            axe = false;
        }
        if (playerClassScript.playerClass == PlayerClassScript.PlayerClass.Medic)
        {
            wrench = false;
            pliers = false;
            databasePass = true;
            syringe = true;
            bioScanner = false;
            accessCard = false;
            toxinDetector = false;
            knife = false;
            axe = false;
        }
        if (playerClassScript.playerClass == PlayerClassScript.PlayerClass.Scientist)
        {
            wrench = false;
            pliers = false;
            databasePass = false;
            syringe = false;
            bioScanner = true;
            accessCard = false;
            toxinDetector = true;
            knife = false;
            axe = false;
        }
    }

    void ToolFunctionality()
    {

    }

    void WrenchFunction()
    {
        print("Wrench is equipped");
    }

    void PliersFunction()
    {
        print("Pliers are equipped");
    }
    void DatabasePassFunction()
    {
        print("Database Pass is equipped");
    }
    void SyringeFunction()
    {
        print("Syringe is equipped");
    }
    void BioScannerFunction()
    {
        print("Bio-Scanner is equipped");
    }
    void AccessCardFunction()
    {
        print("Access Card is equipped");
    }
    void ToxinDetectorFunction()
    {
        print("Toxin Detector is equipped");
    }
}
