using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_Shoot : NetworkBehaviour
{

    private int damage = 100;
    private float range = 200.0f;
    float attackRange = 2.5f;
    [SerializeField] private Transform camTransform;
    private RaycastHit hit;

    PlayerClassScript playerClassScript;

    // Use this for initialization
    void Start()
    {
        //playerClassScript = GetComponent<PlayerClassScript>();
    }

    public override void OnStartClient()
    {
        
        playerClassScript = gameObject.GetComponent<PlayerClassScript>();
    }

    
    [Command]
    public void CmdTellServerOnInteract(string uniqueIDInteractable)
    {
        RpcTellClientsOnInteract(gameObject.name, uniqueIDInteractable);
        //RpcTellClientsOnInteract(gameObject.GetComponent<Player_ID>().playerNetworkName, uniqueIDInteractable);
    }

    [Command]
    public void CmdTellServerOnToolInteract(string uniqueIDInteractable)
    {
        RpcTellClientsOnToolInteract(gameObject.name, uniqueIDInteractable);
        //RpcTellClientsOnToolInteract(gameObject.GetComponent<Player_ID>().playerNetworkName, uniqueIDInteractable);
        
    }

    [ClientRpc]
    void RpcTellClientsOnInteract(string uniqueIDPlayer, string uniqueIDInteractable)
    {
        GameObject player = GameObject.Find(uniqueIDPlayer);
        GameObject interactable = GameObject.Find(uniqueIDInteractable);
        interactable.SendMessage("OnInteraction", player);

        Debug.Log(uniqueIDPlayer + " Interacted with " + uniqueIDInteractable + "!");
    }

    [ClientRpc]
    void RpcTellClientsOnToolInteract(string uniqueIDPlayer, string uniqueIDInteractable)
    {
        GameObject player = GameObject.Find(uniqueIDPlayer);
        GameObject interactable = GameObject.Find(uniqueIDInteractable);
        interactable.SendMessage("OnToolInteraction", player);

        Debug.Log(uniqueIDPlayer + " Interacted with " + uniqueIDInteractable + " using a tool!");
    }

    [Command]
    public void CmdTellServerKillPlayer(string uniqueIDVictim)
    {
        //RpcTellClientsPlayerHasBeenKilled(gameObject.name, uniqueIDVictim);
    }

    [ClientRpc]
    void RpcTellClientsPlayerHasBeenKilled(string uniqueIDKiller, string uniqueIDVictim)
    {
        GameObject killer = GameObject.Find(uniqueIDKiller);
        GameObject victim = GameObject.Find(uniqueIDVictim);
        Debug.Log(victim.name + " has been killed!");
        victim.GetComponent<Player_Health>().DeductHealth(100);

        victim.GetComponent<InventoryScript>().OnDeath();
        //TODO: play kill animation
        killer.GetComponent<KillerScript>().isKilling = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfShooting();
    }

    void CheckIfShooting()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootRay();
        }
    }

    void ShootRay()
    {
        if (Physics.Raycast(camTransform.TransformPoint(0, 0, 0.2f), camTransform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.tag);

            if (hit.transform.tag == "Player" && hit.transform != transform)
            {
                string uIdentity = hit.transform.name;
                //Debug.Log(Vector3.Distance(transform.position, hit.transform.position));

                if (Vector3.Distance(transform.position, hit.transform.position) <= attackRange)
                {
                    if (playerClassScript.playerRole == PlayerClassScript.PlayerRole.Killer)
                    {
                        CmdTellServerWhoWasShot(uIdentity, gameObject.name, damage);
                    }

                    Debug.Log(uIdentity + " hit.");
                }
            }

            //else if (hit.transform.tag == "Zombie")
            //{
            //    string uIdentity = hit.transform.name;
            //    CmdTellServerWhichZombieWasShot(uIdentity, damage);
            //}
        }
    }

    [Command]
    void CmdTellServerWhoWasShot(string victimUniqueID, string killerUniqueID, int damage)
    {
        GameObject victim = GameObject.Find(victimUniqueID);
        GameObject killer = GameObject.Find(killerUniqueID);
        // apply the damage to that player
        victim.GetComponent<Player_Health>().DeductHealth(damage);
        victim.GetComponent<InventoryScript>().OnDeath();

        killer.GetComponent<KillerScript>().isKilling = true;
    }

    //[Command]
    //void CmdTellServerWhichZombieWasShot(string uniqueID, int damage)
    //{
    //    GameObject go = GameObject.Find(uniqueID);
    //    // apply the damage to that player
    //    // TODO: apply damage to zombie
    //    //go.GetComponent<Zombie_Health>().DeductHealth(damage);
    //}
}
