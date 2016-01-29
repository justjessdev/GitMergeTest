//GasTimer.cs
///////////////////////////////////////

using UnityEngine;
using System.Collections;

public class GasTimer : MonoBehaviour {

    private float deathClock;

    private float resetClock;

    public bool isPlayerInGas;

	// Use this for initialization
	void Start () {
        resetClock = 20.0f;
        deathClock = resetClock;
        isPlayerInGas = true;

	}

    void RunTimer()
    {
        if (isPlayerInGas)
        {
            deathClock -= Time.deltaTime;
            Debug.Log(deathClock);
            if (deathClock <= 0)
            {
                deathClock = 0;
                Debug.Log("YOU DIED: Poison Gas");
            }
        }
        else
        {
            deathClock += Time.deltaTime;
            Debug.Log(deathClock);
            if (deathClock >= resetClock)
            {
                Destroy(this);
            }
        }
    }

	// Update is called once per frame
	void Update ()
    {
        RunTimer();
	}
}
