using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerClassScript : NetworkBehaviour
{

    public enum PlayerRole { Killer, Victim };
    [Tooltip("The role of the player during the match")]
    
    [SyncVar]
    public PlayerRole playerRole;

    public enum PlayerClass { Mechanic, Manager, Medic, Scientist };
    [Tooltip("The class of the player during the match")]

    [SyncVar]
    public PlayerClass playerClass;

    public enum PlayerGender { Male, Female };
    [Tooltip("The gender of the player during the match")]

    [SyncVar]
    public PlayerGender playerGender;

    public enum PlayerHeight { Tall, Short };
    [Tooltip("The height of the player during the match")]

    [SyncVar]
    public PlayerHeight playerHeight;

    public enum PlayerHairColor { Blonde, Brown, Red, Black };
    [Tooltip("The hair color of the player during the match")]

    [SyncVar]
    public PlayerHairColor playerHairColor;

    public enum PlayerBloodType { O, A, B, AB };
    [Tooltip("The blood type of the player during the match")]

    [SyncVar]
    public PlayerBloodType playerBloodType;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Player Role Functions
    public void KillerRole() { playerRole = PlayerRole.Killer; }
    public void VictimRole() { playerRole = PlayerRole.Victim; }

    //Player Class Functions
    public void MechanicClass() { playerClass = PlayerClass.Mechanic; }
    public void ManagerClass() { playerClass = PlayerClass.Manager; }
    public void MedicClass() { playerClass = PlayerClass.Medic; }
    public void ScientistClass() { playerClass = PlayerClass.Scientist; }

    //Player Gender Functions
    public void MaleGender() { playerGender = PlayerGender.Male; }
    public void FemaleGender() { playerGender = PlayerGender.Female; }

    //Player Height Functions
    public void TallHeight() { playerHeight = PlayerHeight.Tall; }
    public void ShortHeight() { playerHeight = PlayerHeight.Short; }

    //Player Hair Color Functions
    public void BlondeHair() { playerHairColor = PlayerHairColor.Blonde; }
    public void BrownHair() { playerHairColor = PlayerHairColor.Brown; }
    public void RedHair() { playerHairColor = PlayerHairColor.Red; }
    public void BlackHair() { playerHairColor = PlayerHairColor.Black; }

    //Player Blood Type Functions
    public void OBlood() { playerBloodType = PlayerBloodType.O; }
    public void ABlood() { playerBloodType = PlayerBloodType.A; }
    public void BBlood() { playerBloodType = PlayerBloodType.B; }
    public void ABBlood() { playerBloodType = PlayerBloodType.AB; }
}
