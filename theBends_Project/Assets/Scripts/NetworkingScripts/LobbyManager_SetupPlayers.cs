using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LobbyManager_SetupPlayers : NetworkLobbyManager
{

    // for users to apply settings from their lobby player object to their in-game player object
    public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer, GameObject gamePlayer)
    {
        lobbyPlayer.transform.parent = gamePlayer.transform;

        return true;
    }

    public override void OnLobbyServerSceneChanged(string sceneName)
    {
        base.OnLobbyServerSceneChanged(sceneName);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        AssignRoleOfKillerToRandomPlayer(players);

    }

    void AssignRoleOfKillerToRandomPlayer(GameObject[] players)
    {
        //Debug.Log(players.Length);

        int randomNumber = Random.Range(0, players.Length);

        players[randomNumber].GetComponent<PlayerClassScript>().playerRole = PlayerClassScript.PlayerRole.Killer;
        players[randomNumber].AddComponent<KillerScript>();

       // DATACORE.PlayerClassStats.killerGameObject = players[randomNumber];
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}
