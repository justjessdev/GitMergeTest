using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;

[NetworkSettings(channel = 0, sendInterval = 0.1f)]
public class Player_SyncPosition : NetworkBehaviour
{

    [SyncVar(hook = "SyncPositionValues")]
    private Vector3 syncPos;

    [SyncVar]//(hook = "SyncSprintState")]
    private float sprint = 0.0f;

    [SerializeField]
    Transform myTransform;
    private float lerpRate;
    private float walkingLerpRate = 5.2f;
    private float normalLerpRate = 13.0f;
    private float fasterLerpRate = 22.0f;

    private Vector3 lastPos;
    private float threshhold = 0.3f;

    private List<Vector3> syncPosList = new List<Vector3>();
    [SerializeField]
    private bool useHistoricalLerping = false;
    private float closeEnough = 0.3f;

    void Start()
    {
        lerpRate = normalLerpRate;
    }

    void Update()
    {
        LerpPosition();
        TransmitPosition();
    }

    void ControlLerpSpeedAccordingToSprintState()
    {
        if(sprint == 0.0f) {
            lerpRate = walkingLerpRate;
        }
        else
        {
            lerpRate = normalLerpRate;
        }
    }

    void LerpPosition()
    {
        if (!isLocalPlayer)
        {
            if (useHistoricalLerping)
            {
                //ControlLerpSpeedAccordingToSprintState();
                HistoricalLerping();
            }
            else
            {
                OrdinaryLerping();
            }

        }
    }

    [Command]
    void CmdProvidePositionToServer(Vector3 pos, float sprintState)
    {
        syncPos = pos;
        sprint = sprintState;
        // Debug.Log("command called");
    }

    [ClientCallback]
    void TransmitPosition()
    {
        if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshhold)
        {
            CmdProvidePositionToServer(myTransform.position, Input.GetAxis("Run"));
            lastPos = myTransform.position;
        }
    }

    [Client]
    void SyncPositionValues(Vector3 latestPos)
    {
        syncPos = latestPos;
        syncPosList.Add(syncPos);
    }

    void OrdinaryLerping()
    {
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    void HistoricalLerping()
    {
        if (syncPosList.Count > 0)
        {
            myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);

            if (Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
            {
                syncPosList.RemoveAt(0);
            }

            if (syncPosList.Count > 10)
            {
                lerpRate = fasterLerpRate;
            }
            else
            {
                //lerpRate = normalLerpRate;
                ControlLerpSpeedAccordingToSprintState();
            }

            //Debug.Log(syncPosList.Count.ToString());
        }
    }

}
