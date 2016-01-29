
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class doorControllerScript : NetworkBehaviour {

	public SphereCollider trigger;
	[Range(2.0f, 10.0f)]
	public float proximityRange = 1.0f;

	private Animator anim;

	void Start()
	{
		anim = GetComponent<Animator> ();
	
		GetComponent<NetworkAnimator> ().SetParameterAutoSend (0, true);

		if(trigger == null)
			trigger = GetComponent<SphereCollider> ();
	}

	// Update is called once per frame
	void Update () 
	{	
		trigger.radius = proximityRange;
	}

	void OnTriggerStay(Collider c)
	{
		if(c.CompareTag("Player"))
		{
			anim.SetBool ("isOpen", true);
		}
	}

	void OnTriggerExit(Collider c)
	{
		if(c.CompareTag("Player"))
		{
			anim.SetBool ("isOpen", false);
		}
	}
}
