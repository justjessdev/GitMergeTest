using UnityEngine;
using System.Collections;

public class ShowAndUnlockMouse : MonoBehaviour {

    public bool confinedToScreen = true;
	
	// Update is called once per frame
	void Update () {
        if (confinedToScreen) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
