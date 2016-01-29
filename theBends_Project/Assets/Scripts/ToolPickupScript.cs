using UnityEngine;
using System.Collections;

public class ToolPickupScript : MonoBehaviour {

    public enum ToolType {Wrench, Pliers, DatabasePass, Syringe, BioScanner, AccessCard, ToxinDetector, Knife, Axe };

    public ToolType thisTool = ToolType.Wrench;
	
	void OnInteraction (GameObject other) {
        PlayerClassScript otherClass = other.GetComponent<PlayerClassScript>();
        ToolScript otherTools = other.GetComponent<ToolScript>();

        if (thisTool == ToolType.Wrench)
        {
            if (!otherTools.wrench)
            {
                otherTools.wrench = true;
                Debug.Log("Picked up a Wrench!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.Pliers)
        {
            if (!otherTools.pliers)
            {
                otherTools.pliers = true;
                Debug.Log("Picked up Pliers!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.DatabasePass)
        {
            if (!otherTools.databasePass)
            {
                otherTools.databasePass = true;
                Debug.Log("Picked up a Database Pass!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.Syringe)
        {
            if (!otherTools.syringe)
            {
                otherTools.syringe = true;
                Debug.Log("Picked up a Syringe!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.BioScanner)
        {
            if (!otherTools.bioScanner)
            {
                otherTools.bioScanner = true;
                Debug.Log("Picked up a Bio Scanner!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.AccessCard)
        {
            if (!otherTools.accessCard)
            {
                otherTools.accessCard = true;
                Debug.Log("Picked up an Access Card!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.ToxinDetector)
        {
            if (!otherTools.toxinDetector)
            {
                otherTools.toxinDetector = true;
                Debug.Log("Picked up a Toxin Detector!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.Knife)
        {
            if (!otherTools.knife && otherClass.playerRole == PlayerClassScript.PlayerRole.Killer)
            {
                otherTools.knife = true;
                Debug.Log("Picked up a Knife!");
                Destroy(gameObject);
                return;
            }
        }

        if (thisTool == ToolType.Axe)
        {
            if (!otherTools.axe && otherClass.playerRole == PlayerClassScript.PlayerRole.Killer)
            {
                otherTools.axe = true;
                Debug.Log("Picked up an Axe!");
                Destroy(gameObject);
                return;
            }
        }
	}

    //Leave this here so it catchs ToolInteraction messages
    void OnToolInteraction(GameObject other)
    {

    }
}
