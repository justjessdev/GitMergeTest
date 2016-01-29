using UnityEngine;
using System.Collections;

public class PlayerOutOfBoundsCatcher : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            //Debug.Log("ding");
            other.gameObject.transform.position = gameObject.transform.parent.transform.FindChild("RespawnPoint").transform.position;
        }
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
