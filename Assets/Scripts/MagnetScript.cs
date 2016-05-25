using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetScript : MonoBehaviour 
{
	public bool charged = false;
	public float magnetStrength;

	MeshRenderer rend;
	DoorCompleteScript doorComp;

	MagnetScript doorMag;

	// Use this for initialization
	void Start () 
	{
		doorComp = this.gameObject.GetComponentInParent<DoorCompleteScript> ();
		doorMag = doorComp.doorMagnet.GetComponent<MagnetScript> ();
		rend = transform.GetComponent<MeshRenderer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if (charged) 
		{
			rend.material.color = Color.green;
		}
		else
		{
			rend.material.color = Color.grey;
		}

		if (doorComp.roomA.playerIsHere)
		{
			magnetsOpenDoorSideA ();
		}
		else if (doorComp.roomB.playerIsHere)
		{
			magnetsOpenDoorSideB ();
		}

		CheckMagnetStatus ();
	}

	/// Checks the magnet status.
	/// If it's charged and strength over 0, strength decreases.
	/// Else strength increases, not exceeding 500.
	public void CheckMagnetStatus () 
	{
		if (charged && magnetStrength > 0) 
		{
			magnetStrength--;
		} 
		else if (!charged && magnetStrength < 500f) 
		{
			magnetStrength++;
			if (magnetStrength > 500) 
			{
				magnetStrength = 500;
			}
		}

		if (magnetStrength <= 0) 
		{
			magnetStrength = 0;
			magRules ();
		}
	}
		
	/// If the magnet you're aiming at is on whichever side:
	/// If it's on the bottom, charge it.
	/// But if it's higher, the ones from all preceding heights must be on.
	public void magRules ()
	{
		if (this.gameObject != doorComp.doorMagnet)
		{
			if (charged) 
			{				
				doorComp.chargedList.Remove (gameObject);
				charged = false;
			}
			else 
			{
				charged = true;
				if (!doorComp.chargedList.Contains (gameObject)) 
				{
					doorComp.chargedList.Add (gameObject);
				}
			}	
		}
		else if (this.gameObject == doorComp.doorMagnet)
		{
			doorMagRules ();									
		}
	}

	/// If there are 8 charged side magnets:
	/// The door magnet is ready to be charged.
	/// If any of the side ones and the door one is charged:
	/// The manual override is still in effect.
	public void magnetsOpenDoorSideA ()
	{
		if (doorComp.chargedList.Count == 8) 
		{
			doorComp.doorMagReady = true;
		}

		if (doorComp.chargedList.Count > 0 && doorMag.charged) 
		{
			doorComp.manualOverrideAEnabled = true;
		}
		else
		{
			doorComp.manualOverrideAEnabled = false;
			doorMag.charged = false;
		}
	}

	// Same as above, but on the other side.
	public void magnetsOpenDoorSideB ()
	{
		if (doorComp.chargedList.Count == 8) 
		{
			doorComp.doorMagReady = true;
		}

		if (doorComp.chargedList.Count > 0 && doorMag.charged) 
		{
			doorComp.manualOverrideBEnabled = true;
		}
		else
		{
			doorComp.manualOverrideBEnabled = false;
			doorMag.charged = false;
		}
	}

	public void doorMagRules () 
	{
		if (doorComp.doorMagReady) 
		{
			doorMag.charged = true;
		} 
	}

}
