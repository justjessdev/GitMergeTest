using UnityEngine;
using System.Collections;

public static class DATACORE {

    public static class PlayerClassStats
    {
        //PlayerClassScript.PlayerRole playerRole = PlayerClassScript.PlayerRole.Victim;
        public static PlayerClassScript.PlayerClass playerClass = PlayerClassScript.PlayerClass.Manager;
        public static PlayerClassScript.PlayerGender playerGender = PlayerClassScript.PlayerGender.Female;
        public static PlayerClassScript.PlayerHeight playerHeight = PlayerClassScript.PlayerHeight.Short;
        public static PlayerClassScript.PlayerHairColor playerHairColor = PlayerClassScript.PlayerHairColor.Black;
        public static PlayerClassScript.PlayerBloodType playerBloodType = PlayerClassScript.PlayerBloodType.A;

        public static GameObject clientGameObject;

        public static string clientChatName;

        public static string GetLocalPlayerName()
        {
            return DATACORE.PlayerClassStats.clientGameObject.GetComponentInChildren<LobbyPlayer>().playerName;
        }
    }

}
