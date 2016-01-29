using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour
{
    public Camera FPSCharacterCam;
    public AudioListener audioListener;
    public CharacterController playerCharacterController;
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController playerFirstPersonController;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        DATACORE.PlayerClassStats.clientGameObject = gameObject;

        FPSCharacterCam.enabled = true;
        audioListener.enabled = true;
        playerFirstPersonController.enabled = true;

        Renderer[] rends = GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in rends)
        {
            rend.enabled = false;
        }

        //PlayerClassScript gameControllerClassScript = GameObject.Find("GameController").GetComponent<PlayerClassScript>();

        PlayerClassScript playerClassScript = GetComponent<PlayerClassScript>();

        playerClassScript.playerClass = DATACORE.PlayerClassStats.playerClass;
        playerClassScript.playerGender = DATACORE.PlayerClassStats.playerGender;
        playerClassScript.playerHeight = DATACORE.PlayerClassStats.playerHeight;
        playerClassScript.playerHairColor = DATACORE.PlayerClassStats.playerHairColor;
        playerClassScript.playerBloodType = DATACORE.PlayerClassStats.playerBloodType;

        GameObject.Find("SceneCamera").SetActive(false);

        
        // TODO: for animations
        //GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
    }

    public override void PreStartClient()
    {
        // TODO: for animations
        // GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
    }

}
