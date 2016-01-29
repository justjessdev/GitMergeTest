using UnityEngine;
using System.Collections;

public class HideUntilStartup : MonoBehaviour {

    private MeshRenderer meshRenderer;

	// Use this for initialization
	void Awake () {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        if (Application.loadedLevel != 6) meshRenderer.enabled = false;
	}

    void OnLevelWasLoaded(int level) {
        if (level == 5)
            meshRenderer.enabled = true;
            Debug.Log("Hello.");
    }
}
