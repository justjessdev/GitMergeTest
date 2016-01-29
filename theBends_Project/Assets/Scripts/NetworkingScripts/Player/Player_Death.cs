using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Death : NetworkBehaviour {

    private Player_Health healthScript;
    private Image crossHairImage;

    public override void PreStartClient()
    {
        healthScript = GetComponent<Player_Health>();
        healthScript.EventDie += DisablePlayer;
    }

    public override void OnStartLocalPlayer()
    {
        crossHairImage = GameObject.Find("CrossHairImage").GetComponent<Image>();
    }

    public override void OnNetworkDestroy()
    {
        healthScript.EventDie -= DisablePlayer;
    }

    void DisablePlayer()
    {
        GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        GetComponent<CharacterController>().enabled = false;
        GetComponent<Player_Shoot>().enabled = false;
        GetComponentInChildren<CapsuleCollider>().enabled = false;
        
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer ren in renderers)
        {
            ren.enabled = false;
        }

        healthScript.isDead = true;

        if(isLocalPlayer) {
            crossHairImage.enabled = false;
            // respawn button
            // TODO: set respawn button to true
            //GameObject.Find("GameManager").GetComponent<GameManager_References>().respawnButton.SetActive(true);
        }
    }
}
