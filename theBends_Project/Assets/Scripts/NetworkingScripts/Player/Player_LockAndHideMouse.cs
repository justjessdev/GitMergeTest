using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Player_LockAndHideMouse : NetworkBehaviour {

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer) return;
        LockAndHideMouseCursor();
        GetComponent<Player_Health>().EventDie += UnlockAndShowCursor;
        GetComponent<Player_Health>().EventRespawn += LockAndHideMouseCursor;
	}

    void OnDisable()
    {
        if (!isLocalPlayer) return;
        GetComponent<Player_Health>().EventDie -= UnlockAndShowCursor;
        GetComponent<Player_Health>().EventRespawn -= LockAndHideMouseCursor;
    }

    public void LockAndHideMouseCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UnlockAndShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
