using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadOnClick : MonoBehaviour
{

    public GameObject loadingImage;
    //public GameObject gameController;
    //private PlayerClassScript playerClassScript;
    public Text currentClassText;

    string playerClass = "";
    string playerGender = "";
    string playerHeight = "";
    string playerHair = "";

    void Start()
    {

    }

    public void OnDefaultButtonPressed()
    {
        OnManagerButtonPressed();
        OnMaleButtonPressed();
        OnTallButtonPressed();
        OnRedHairButtonPressed();
    }

    public void LoadMain(string main)
    {
        loadingImage.SetActive(true);
        Application.LoadLevel(main);
    }

    public void OnMechanicButtonPress()
    {
        DATACORE.PlayerClassStats.playerClass = PlayerClassScript.PlayerClass.Mechanic;
        playerClass = "Mechanic, ";
    }

    public void OnMedicButtonPressed()
    {
        DATACORE.PlayerClassStats.playerClass = PlayerClassScript.PlayerClass.Medic;
        playerClass = "Medic, ";
    }

    public void OnManagerButtonPressed()
    {
        DATACORE.PlayerClassStats.playerClass = PlayerClassScript.PlayerClass.Manager;
        playerClass = "Manager, ";
    }

    public void OnScientistButtonPressed()
    {
        DATACORE.PlayerClassStats.playerClass = PlayerClassScript.PlayerClass.Scientist;
        playerClass = "Scientist, ";
    }

    public void OnMaleButtonPressed()
    {
        DATACORE.PlayerClassStats.playerGender = PlayerClassScript.PlayerGender.Male;
        playerGender = "Male, ";
    }
    public void OnFemaleButtonPressed()
    {
        DATACORE.PlayerClassStats.playerGender = PlayerClassScript.PlayerGender.Female;
        playerGender = "Female, ";
    }
    public void OnTallButtonPressed()
    {
        DATACORE.PlayerClassStats.playerHeight = PlayerClassScript.PlayerHeight.Tall;
        playerHeight = "Tall, ";
    }
    public void OnShortButtonPressed()
    {
        DATACORE.PlayerClassStats.playerHeight = PlayerClassScript.PlayerHeight.Short;
        playerHeight = "Short, ";
    }
    public void OnBrownHairButtonPressed()
    {
        DATACORE.PlayerClassStats.playerHairColor = PlayerClassScript.PlayerHairColor.Brown;
        playerHair = "Brown hair.";
    }
    public void OnBlackHairButtonPressed()
    {
        DATACORE.PlayerClassStats.playerHairColor = PlayerClassScript.PlayerHairColor.Black;
        playerHair = "Black hair.";
    }
    public void OnBlondeHairButtonPressed()
    {
        DATACORE.PlayerClassStats.playerHairColor = PlayerClassScript.PlayerHairColor.Blonde;
        playerHair = "Blonde hair.";
    }
    public void OnRedHairButtonPressed()
    {
        DATACORE.PlayerClassStats.playerHairColor = PlayerClassScript.PlayerHairColor.Red;
        playerHair = "Red hair.";
    }


    public void OnContinueButtonPressed(string gameScene)
    {
        if (playerClass == "" || playerGender == "" || playerHeight == "" || playerHair == "")
        {
            return;
        }
        Application.LoadLevel(gameScene);
    }

    void Update()
    {
        if(currentClassText == null) {
            return;
        }

        currentClassText.text = playerClass + playerGender + playerHeight + playerHair;
    }

}