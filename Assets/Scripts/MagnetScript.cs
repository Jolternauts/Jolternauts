using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MagnetScript : MonoBehaviour 
{
	public float maxForce = 100f;
	public float currentForce;
	public float forceLossRate = 5f;
//	public bool allCharged;

	MeshRenderer rend;

	public int stage;

	public bool charged = false;
	public float magnetStrength = 150f;

	DoorCompleteScript doorComp;

	// Use this for initialization
	void Start () 
	{
		doorComp = this.gameObject.GetComponentInParent<DoorCompleteScript> ();
		rend = transform.GetComponent<MeshRenderer> ();
		currentForce = 0;
		InvokeRepeating ("CheckMagnetStatus", 0f, 0.1f);
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
	}

}
