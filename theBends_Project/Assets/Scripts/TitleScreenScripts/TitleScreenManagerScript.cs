using UnityEngine;
using System.Collections;

public class TitleScreenManagerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlayButtonPressed()
    {
        Application.LoadLevel("ClassSelect");
    }

    public void OnOptionsButtonPressed()
    {

    }

    public void OnExitButtonPressed()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
