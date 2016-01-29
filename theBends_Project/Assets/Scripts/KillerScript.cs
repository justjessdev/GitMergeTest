using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class KillerScript : NetworkBehaviour
{

    private InventoryScript m_playerInventory;

    private ToolScript m_playerTools;

    public int selectedWeapon = 0; //0 = knife, 1 = axe, 2 = wrench

    public float weaponCoolDown = 0;

    public float weaponCycleCoolDown = 0;

    public bool isKilling = false;

    private float weaponCycleTime = 0.5f;
    private float[] swingTime = new float[3];
    private float[] killTime = new float[3];
    private float[] weaponRange = new float[3];

    private List<GameObject> playersList;

    //the clients shoot script
    private Player_Shoot m_playerShoot;

    void GetListOfPlayers()
    {
        playersList = GameManagerScript.instance.players;

        m_playerShoot = DATACORE.PlayerClassStats.clientGameObject.GetComponent<Player_Shoot>();
    }

	//Initialize the weapon cooldown arrays and find the players
	void Start () {


	    swingTime[0] = 1.0f;
	    swingTime[1] = 2.0f;
        swingTime[2] = 1.5f;

        killTime[0] = 3.0f;
        killTime[1] = 5.0f;
        killTime[2] = 4.0f;

        weaponRange[0] = 3.0f;
        weaponRange[1] = 4.0f;
        weaponRange[2] = 3.5f;

        GetListOfPlayers();

        m_playerInventory = gameObject.GetComponent<InventoryScript>();
        m_playerTools = gameObject.GetComponent<ToolScript>();
    }
	
	void Update () {

        if (!isLocalPlayer) return;

        //if there is a weapon cool down, don't let the player do anything with their weapons until its over
        if (weaponCoolDown > 0)
        {
            weaponCoolDown -= Time.deltaTime;
            if (weaponCoolDown <= 0)
            {
                weaponCoolDown = 0;
                isKilling = false;
            }
            return;
        }

        //if there is a switching weapon cool down, don't let the player do anything with their weapons until its over
        if (weaponCycleCoolDown > 0)
        {
            weaponCycleCoolDown -= Time.deltaTime;
            if (weaponCycleCoolDown <= 0)
            {
                weaponCycleCoolDown = 0;
            }
            return;
        }

        //if (DATACORE.PlayerClassStats.clientGameObject != gameObject) return;

        //Weapon changing button
        if (Input.GetButtonDown("WeaponCycle"))
        {
            //Remember the weapon the killer held before pressing the button
            int startingWeapon = selectedWeapon;

            //switch to the next weapon
            ++selectedWeapon;
            if (selectedWeapon >= 3) selectedWeapon = 0;

            //if the killer doesnt have that weapon, cycle until a weapon they are holding is owned
            while (!DoIHaveTheSelectedWeapon())
            {
                ++selectedWeapon;
                if (selectedWeapon >= 3) selectedWeapon = 0;
            }

            //if the killer is on a new weapon, start the cooldown for changing weapons
            if (selectedWeapon != startingWeapon) weaponCycleCoolDown = weaponCycleTime;
        }

        //Attack button
        if (Input.GetButtonDown("Fire1"))
        {
            
            GameObject otherPlayer = GetPlayerToKill();
            if (otherPlayer != null){
                
                KillPlayer(otherPlayer);
            }
            else weaponCoolDown = swingTime[selectedWeapon];
        }
	}

    //Checks all the players to see if they are in range to be killed
    GameObject GetPlayerToKill()
    {
        for (int i = 0; i < playersList.Count; ++i)
        {
            if (playersList[i] == null || playersList[i] == this.gameObject) continue;
            
            if (CheckPlayerInRange(playersList[i]))
            {
                return playersList[i];
            }

        }
        return null;
    }

    //checks a single player to see if they are in range to be killed, used by the GetPlayerToKill function
    bool CheckPlayerInRange(GameObject other)
    {
        if (Vector3.Magnitude(other.transform.position - this.transform.position) < weaponRange[selectedWeapon]){
            if (Vector3.Dot(this.transform.forward, Vector3.Normalize(other.transform.position - this.transform.position)) > 0.5f){
                return true;
            }
        }
        return false;
    }

    bool DoIHaveTheSelectedWeapon()
    {
        if (selectedWeapon == 0) return true;
        else if (selectedWeapon == 1) return m_playerTools.axe;
        else return m_playerTools.wrench;
    }

    void KillPlayer(GameObject otherPlayer)
    {
        weaponCoolDown = killTime[selectedWeapon];
        m_playerShoot.CmdTellServerKillPlayer(otherPlayer.name);
    }
}
