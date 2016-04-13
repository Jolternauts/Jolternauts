using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetScript : MonoBehaviour 
{
	public float maxForce = 100f;
	public float currentForce;
	public float forceLossRate = 5f;
	public bool charged = false;
	public bool allCharged;

	MeshRenderer rend;

	// Use this for initialization
	void Start () 
	{
		rend = transform.GetComponent<MeshRenderer> ();
		currentForce = 0;
//		allCharged = Door.magnetStrip.TrueForAll(charged);

	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if (charged) 
		{
			rend.material.color = Color.green;
			currentForce -= forceLossRate * Time.deltaTime;
			if (currentForce <= 0) 
			{
				currentForce = 0;
				rend.material.color = Color.red;
				charged = false;
			}

		}



/*		if (allCharged) 
		{
			Door.Locked = false;
			foreach (GameObject magnet in Door.magnetStrip) 
			{
				magnet.GetComponent<MagnetScript>().currentForce = currentForce;
			}
		}
*/	}


}
