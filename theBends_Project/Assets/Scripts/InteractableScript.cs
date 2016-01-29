// InteractableScript.cs
//////////////////////////////////////////////////////

//////////////////////////////////////////////////////
// TODO
//We need cast timers, but I have no idea how to implement them without an iEnumerator
//Send halp
//////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

// The Interactable requires a Collider component to function
[RequireComponent(typeof(Collider))]
public class InteractableScript : NetworkBehaviour
{

    //Reference to the game manager
    [HideInInspector]
    public GameObject gameManager;

    [Tooltip("The maximum distance the player can be from the interactable and still interact with it")]
    public float interactRange = 5;

    [Tooltip("Whether the player needs to click to interact with the interactable or not")]
	public bool clickRequired = true;
	
	[Tooltip("Whether the player needs to use a tool to interact with the interactable or not only")]
	public bool toolRequired = false;

    [Tooltip("Whether the player can use a tool to interact with the interactable or not")]
    public bool toolIsUsable = false;

    //The position of the object in relation to where the player is looking when interactible
    public enum LookMode { noLookRequired, onScreen, inCenterOfScreen };

    public LookMode viewDirectionChecking = LookMode.inCenterOfScreen;

    [Tooltip("Whether the interaction will fail or not if there is a wall in the way")]
    public bool checkForWalls = true;

    [Tooltip("Whether the player needs to leave the objects radius to interact with the interactable again")]
    public bool leaveRadiusRequired = false;

    [Tooltip("Cool down time (in seconds) until the player can interact with it again")]
    public float interactionCoolDownTime = 10.0f;

    [Tooltip("GameObject to send the OnInteraction event to, defaults to the parent gameobject")]
    public GameObject sendEventTo;

    [Tooltip("The client's player GameObject. For interaction checking, the client only needs to keep track of itself, all players will send their own interaction events to the server")]
    public GameObject m_player;

    //the client's player Camera
    private Transform m_playerCamera;

    //the clients shoot script
    private Player_Shoot m_playerShoot;

    //counts down after the player interacts with it
    private float m_playerInteractionCooldownTimer = 0;

    //if the player needs to leave the radius to interact with the interactable again
    private bool m_playerMustLeave = false;

    void Start()
    {
        //get the main player from the game manager
        //if the game manager isn't initialized for some reason (Stupid getter isnt working the way it should?) then cancel
        if (!DATACORE.PlayerClassStats.clientGameObject) return;

        m_player = DATACORE.PlayerClassStats.clientGameObject;
        //PlayerController control = m_player.GetComponent<PlayerController>();
        m_playerShoot = m_player.GetComponent<Player_Shoot>();
        m_playerCamera = m_player.transform.FindChild("PlayerCamera").transform;

        //initialize the object to send the event to
        if (!sendEventTo) sendEventTo = gameObject.transform.parent.gameObject;
	}
	
	void Update () {
        if (m_player && sendEventTo)
        {
            //interaction cool down timer
            if (m_playerInteractionCooldownTimer > 0)
            {
                m_playerInteractionCooldownTimer -= Time.deltaTime;
                return;
            }

            //if the player is close enough to the interactable
            if (Vector3.Distance(m_player.transform.position, gameObject.transform.position) <= interactRange)
            {
                //if the player needs to leave the radius first, then stop
                if (m_playerMustLeave) return;

				if (isPlayerLookingAtMe())
				{
					if (!clickRequired || 
					    (Input.GetMouseButtonDown(0) && !toolRequired)){
						Interact();
						return;
					}
				    else if (Input.GetMouseButtonDown(1) && (toolRequired || toolIsUsable))
                	{
                        ToolInteract();
                        return;
                    }
                }
            }
            else m_playerMustLeave = false;
        }
        else Start();
	}

    //Called by the script when an interaction actually takes place
    void Interact()
    {
        m_playerShoot.CmdTellServerOnInteract(sendEventTo.name);

        m_playerInteractionCooldownTimer = interactionCoolDownTime;

        if (leaveRadiusRequired) m_playerMustLeave = true;
    }

    //Called by the script when a tool interaction actually takes place
    void ToolInteract()
    {
        m_playerShoot.CmdTellServerOnToolInteract(sendEventTo.name);

        m_playerInteractionCooldownTimer = interactionCoolDownTime;

        if (leaveRadiusRequired) m_playerMustLeave = true;
    }

    //Used to check if the player is looking towards the interactable
    bool isPlayerLookingAtMe()
    {
        //if no looking required, this step is always true
        if (viewDirectionChecking == LookMode.noLookRequired) return true;

        //if it must be in the center of the screen, we need to do a raycast and check walls
        else if (viewDirectionChecking == LookMode.inCenterOfScreen)
        {
            //create the ray
            Ray playerViewRay = new Ray(m_playerCamera.position, m_playerCamera.forward);

            RaycastHit rayHit;

            //if (gameObject.GetComponent<Collider>().Raycast(playerViewRay, rayHit, interactRange){
            if (gameObject.GetComponent<Collider>().Raycast(playerViewRay, out rayHit, interactRange))
            {
                //check if the raycast hits a solid object, if it doesn't than the interaction succeeds
                if (!checkForWalls || !DoesRayHitAWall(playerViewRay, rayHit.distance)) return true;
            }
        }

        //if it just needs to be on screen, we need to take the dot product and check walls
        else
        {
            //get the direction to the interactable
            Vector3 directionToInteractable = Vector3.Normalize(gameObject.transform.position - m_player.transform.position);

            //calculate the dot product between the playing looking direction, and the direction to the interactable
            float dot = Vector3.Dot(m_playerCamera.forward, directionToInteractable);

            //if the dot product is too small, than it is not on screen, and the interaction fails
            if (dot < 0.65) return false;

            if (checkForWalls)
            {
                //create the ray
                Ray playerViewRay = new Ray(m_player.transform.position, directionToInteractable);

                //check if the raycast hits a solid object, if it doesn't than the interaction succeeds
                if (!DoesRayHitAWall(playerViewRay, Vector3.Distance(m_player.transform.position, gameObject.transform.position))) return true;
            }
            else return true;
        }

        return false;
    }

    //used to RayCast with walls to see if the interactable is in view
    bool DoesRayHitAWall(Ray a_ray, float a_distance)
    {
        //create a layer mask to only collide with layer 8 (solid objects)
        LayerMask layerMask = 1 << 8;

        return Physics.Raycast(a_ray, a_distance, layerMask);
    }
}
