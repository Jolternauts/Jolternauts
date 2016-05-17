using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetScript : MonoBehaviour 
{
<<<<<<< HEAD
	public bool charged = false;
	public float magnetStrength;
=======
	public float maxForce = 100f;
	public float currentForce;
	public float forceLossRate = 5f;
//	public bool allCharged;
>>>>>>> origin/master

	MeshRenderer rend;
	DoorCompleteScript doorComp;

	MagnetScript doorMag;

	public int stage;

	public bool charged = false;
	public float magnetStrength = 150f;

	DoorCompleteScript doorComp;

	// Use this for initialization
	void Start () 
	{
		doorComp = this.gameObject.GetComponentInParent<DoorCompleteScript> ();
<<<<<<< HEAD
		doorMag = doorComp.doorMagnet.GetComponent<MagnetScript> ();
		rend = transform.GetComponent<MeshRenderer> ();
=======
		rend = transform.GetComponent<MeshRenderer> ();
		currentForce = 0;
		InvokeRepeating ("CheckMagnetStatus", 0f, 0.1f);
>>>>>>> origin/master
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
<<<<<<< HEAD

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
=======

		if (doorComp.roomA.playerIsHere)
		{
			magnetsOpenDoorSideA ();
		}
		else if (doorComp.roomB.playerIsHere)
		{
			magnetsOpenDoorSideB ();
		}

		bool test = doorComp.areAllTrue(doorComp.truthList);

		if (test == true) 
		{
			doorComp.manualOverrideEnabled = true;
			CancelInvoke ("CheckMagnetStatus");
		}
	}

	public void CheckMagnetStatus () 
	{
		if (charged && magnetStrength > 0) 
		{
			magnetStrength--;
		} 
		else if ( !charged && magnetStrength < 150f) 
		{
			magnetStrength++;
		}

		if (magnetStrength == 0) 
		{
			charged = false;
		}
	}

	/// If the magnet you're aiming at is on whichever side:
	/// If it's on the bottom, charge it.
	/// But if it's higher, the ones from all preceding heights must be on.
	public void magRules ()
	{
		if (doorComp.aMagnets.Contains(this.gameObject))
		{
			MagnetScript aMag1 = doorComp.aMagnets [0].GetComponent<MagnetScript> ();
			MagnetScript aMag2 = doorComp.aMagnets [1].GetComponent<MagnetScript> ();
			MagnetScript aMag3 = doorComp.aMagnets [2].GetComponent<MagnetScript> ();
			MagnetScript aMag4 = doorComp.aMagnets [3].GetComponent<MagnetScript> ();
			MagnetScript aMag5 = doorComp.aMagnets [4].GetComponent<MagnetScript> ();
			MagnetScript aMag6 = doorComp.aMagnets [5].GetComponent<MagnetScript> ();

			if (this.gameObject == doorComp.aMagnets[0] || this.gameObject == doorComp.aMagnets[1])
			{
				if (charged) 
				{
					charged = false;
					doorComp.truthList.Remove (charged);
				}
				else 
				{
					charged = true;
					doorComp.truthList.Add (charged);
					magnetStrength = 150;
				}									
			}
			else if (this.gameObject == doorComp.aMagnets[2] || this.gameObject == doorComp.aMagnets[3])
			{
				if (aMag1.charged && aMag2.charged)
				{
					if (charged) 
					{
						charged = false;
						doorComp.truthList.Remove (charged);
					}
					else 
					{
						charged = true;
						doorComp.truthList.Add (charged);
						magnetStrength = 150;
					}									
				}
			}
			else if (this.gameObject == doorComp.aMagnets[4] || this.gameObject == doorComp.aMagnets[5])
			{
				if (aMag1.charged && aMag2.charged && aMag3.charged && aMag4.charged)
				{
					if (charged) 
					{ 
						charged = false;
						doorComp.truthList.Remove (charged);
					}
					else 
					{
						charged = true;
						doorComp.truthList.Add (charged);
						magnetStrength = 150;
					}									
				}
			}
			else if (this.gameObject == doorComp.aMagnets[6] || this.gameObject == doorComp.aMagnets[7])
			{
				if (aMag1.charged && aMag2.charged && aMag3.charged && aMag4.charged && aMag5.charged && aMag6.charged)
				{
					if(charged) 
					{
						charged = false;
						doorComp.truthList.Remove (charged);
					}
					else 
					{
						charged = true;
						doorComp.truthList.Add (charged);
						magnetStrength = 150;
					}									
				}
			}
		}
		else if (doorComp.bMagnets.Contains(this.gameObject))
		{
			MagnetScript bMag1 = doorComp.bMagnets [0].GetComponent<MagnetScript> ();
			MagnetScript bMag2 = doorComp.bMagnets [1].GetComponent<MagnetScript> ();
			MagnetScript bMag3 = doorComp.bMagnets [2].GetComponent<MagnetScript> ();
			MagnetScript bMag4 = doorComp.bMagnets [3].GetComponent<MagnetScript> ();
			MagnetScript bMag5 = doorComp.bMagnets [4].GetComponent<MagnetScript> ();
			MagnetScript bMag6 = doorComp.bMagnets [5].GetComponent<MagnetScript> ();

			if (this.gameObject == doorComp.bMagnets[0] || this.gameObject == doorComp.bMagnets[1])
			{
				if(charged) 
				{
					charged = false;
					doorComp.truthList.Remove (charged);
				}
				else 
				{
					charged = true;
					magnetStrength = 150;
					doorComp.truthList.Add (charged);
				}									
			}
			else if (this.gameObject == doorComp.bMagnets[2] || this.gameObject == doorComp.bMagnets[3])
			{
				if (bMag1.charged && bMag2.charged)
				{
					if(charged) 
					{
						charged = false;
						doorComp.truthList.Remove (charged);
					}
					else 
					{
						charged = true;
						magnetStrength = 150;
						doorComp.truthList.Add (charged);
					}									
				}
			}
			else if (this.gameObject == doorComp.bMagnets[4] || this.gameObject == doorComp.bMagnets[5])
			{
				if (bMag1.charged && bMag2.charged && bMag3.charged && bMag4.charged)
				{
					if(charged) 
					{
						charged = false;
						doorComp.truthList.Remove (charged);
					}
					else 
					{
						charged = true;
						magnetStrength = 150;
						doorComp.truthList.Add (charged);
					}									
				}
			}
			else if (this.gameObject == doorComp.bMagnets[6] || this.gameObject == doorComp.bMagnets[7])
			{
				if (bMag1.charged && bMag2.charged && bMag3.charged && bMag4.charged && bMag5.charged && bMag6.charged)
				{
					if(charged) 
					{
						charged = false;
						doorComp.truthList.Remove (charged);
					}
					else 
					{
						charged = true;
						magnetStrength = 150;
						doorComp.truthList.Add (charged);
					}									
				}
			}
		}
	}

	public void magnetsOpenDoorSideA ()
	{
		MagnetScript aMag1 = doorComp.aMagnets [0].GetComponent<MagnetScript> ();
		MagnetScript aMag2 = doorComp.aMagnets [1].GetComponent<MagnetScript> ();
		MagnetScript aMag3 = doorComp.aMagnets [2].GetComponent<MagnetScript> ();
		MagnetScript aMag4 = doorComp.aMagnets [3].GetComponent<MagnetScript> ();
		MagnetScript aMag5 = doorComp.aMagnets [4].GetComponent<MagnetScript> ();
		MagnetScript aMag6 = doorComp.aMagnets [5].GetComponent<MagnetScript> ();
		MagnetScript aMag7 = doorComp.aMagnets [6].GetComponent<MagnetScript> ();
		MagnetScript aMag8 = doorComp.aMagnets [7].GetComponent<MagnetScript> ();

		if (aMag1.charged && aMag2.charged && aMag3.charged && aMag4.charged)
		{
			doorComp.bottomDoorUp ();
		}

		if (aMag1.charged && aMag2.charged && aMag3.charged && aMag4.charged && 
			aMag5.charged && aMag6.charged)
		{
			doorComp.bottomDoorUp ();
		}

		if (aMag1.charged && aMag2.charged && aMag3.charged && aMag4.charged && 
			aMag5.charged && aMag6.charged && aMag7.charged && aMag8.charged)
		{
			doorComp.bothDoorsUp ();
		}
	}

	public void magnetsOpenDoorSideB ()
	{
		MagnetScript bMag1 = doorComp.bMagnets [0].GetComponent<MagnetScript> ();
		MagnetScript bMag2 = doorComp.bMagnets [1].GetComponent<MagnetScript> ();
		MagnetScript bMag3 = doorComp.bMagnets [2].GetComponent<MagnetScript> ();
		MagnetScript bMag4 = doorComp.bMagnets [3].GetComponent<MagnetScript> ();
		MagnetScript bMag5 = doorComp.bMagnets [4].GetComponent<MagnetScript> ();
		MagnetScript bMag6 = doorComp.bMagnets [5].GetComponent<MagnetScript> ();
		MagnetScript bMag7 = doorComp.bMagnets [6].GetComponent<MagnetScript> ();
		MagnetScript bMag8 = doorComp.bMagnets [7].GetComponent<MagnetScript> ();

		if (bMag1.charged && bMag2.charged && bMag3.charged && bMag4.charged)
		{
			doorComp.bottomDoorUp ();
		}

		if (bMag1.charged && bMag2.charged && bMag3.charged && bMag4.charged && 
			bMag5.charged && bMag6.charged)
		{
			doorComp.bottomDoorUp ();
		}

		if (bMag1.charged && bMag2.charged && bMag3.charged && bMag4.charged && 
			bMag5.charged && bMag6.charged && bMag7.charged && bMag8.charged)
		{
			doorComp.bothDoorsUp ();
		}
>>>>>>> origin/master
	}

}
