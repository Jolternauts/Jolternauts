using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerDrain : ObjectClass
{
	//this is for anything that demands power

	RoomScript room;
	FuseBox box;
	Renderer deviceRend;

	public bool besideDevice;
	public bool uploadComplete;

	void Start()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		box = room.roomFuseBox.GetComponent<FuseBox> ();
		deviceRend = this.gameObject.GetComponent<Renderer> ();
	}

	void Update()
	{

	}

	// Change state of device.
	public void changeState (GameObject reference)
	{
		// If fusebox is active.
		if (box.stateActive ()) 
		{
			// If A.R.S is over 0.
			// Change active / damaged states and elements.
			if (!stateActive ()) 
			{
				if ((room.availableRoomSupply - powerDemand) >= 0)
				{
					drainerStateChangeCriteria ();
<<<<<<< HEAD
					if (myName == "Health_Console") 
=======
<<<<<<< HEAD
					if (myName == "Health_Console") 
=======
<<<<<<< HEAD
					if (myName == "Health_Console") 
=======
					if (myName == "ChargeStation") 
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
					{
						recharge ();
					}
				} 
			} 
			else 
			{
				drainerStateChangeCriteria ();
			}
		} 
		else
		{
			if (!stateOn ()) 
			{
				changeRendColor (neutralColor);
				stateOn (true);
			}

			else if (stateOn ()) 
			{
				changeRendColor (offColor);
				stateOn (false);
			} 
			else // Debug error with object's name.
				Debug.Log ("ObjectScript ChangeState Error" + this.name);

			machineStateMeterCheck ();
		}
	}

	void OnTriggerEnter (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideDevice = true;
			machineStateMeterCheck ();
		}
	}

	void OnTriggerStay (Collider detector)
	{
		// Detect player at object and turn it on by pressing E.
		// If it's a charge station: replenish health, suit power and oxygen.

		if (detector.transform.tag == "Player")
		{
			if (Input.GetKeyDown (KeyCode.E) && !statePressed ()) 
			{
				changeState (this.gameObject);
				statePressed (true);
			}
			if (Input.GetKeyUp (KeyCode.E)) 
			{
				statePressed (false);
<<<<<<< HEAD
			}

			if (Input.GetKeyDown (KeyCode.U) && !statePressed ()) 
			{
				if (myTag == "GoalCon") 
				{
					if (gameMngr.uploadedDevices.Count == (gameMngr.numberOfDevices - 1))
					{
						StartCoroutine (uploadFiles ());
						Debug.Log ("You Win");
					}
				}
				else
				{
					StartCoroutine (uploadFiles ());
					gameMngr.uploadedDevices.Add (this.gameObject);
				}
				statePressed (true);
			}
			if (Input.GetKeyUp (KeyCode.U)) 
			{
				statePressed (false);
=======
>>>>>>> origin/master
			}
		}

		machineStateMeterCheck ();
	}

	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideDevice = false;
			machineStateMeterCheck ();
		}
	}

	IEnumerator uploadFiles ()
	{
		Debug.Log ("Buffering");
		yield return new WaitForSeconds (repairTime);
		uploadComplete = true;
	}

	// Checks if device group is damaged or not and A.R.S is enough to turn on.
	public void massActivationCheck ()
	{
<<<<<<< HEAD
		if ((room.availableRoomSupply - powerDemand) >= 0) 
		{				
			if (!stateDamaged () && stateOn ())
			{
				Debug.Log ("Safe Activation for " + myName);
				stateActive (true);
				stateOn (false);
				changeRendColor (activeColor);
				box.roomSinglePowerUp (powerDemand);
				gameMngr.levelObjectPowerUp (powerDemand);
				gameMngr.objectsToCut.Add (this.gameObject);
				if (myTag == "HealthCon")
				{
					recharge ();
				}
			}
			else if (stateDamaged () && stateOn ()) 
			{
				box.roomCrash ();
				Debug.Log (myName + " is Damaged");
				Debug.Log ("And now Fusebox is Damaged");
			} 
=======
		if (!stateDamaged() && stateOn() && room.availableRoomSupply - powerDemand >= 0)
		{
			Debug.Log ("Safe Activation for " + myName);
			stateActive (true);
			stateOn (false);
			changeRendColor (activeColor);
			box.roomSinglePowerUp (powerDemand);
			gameMngr.levelObjectPowerUp (powerDemand);
//			gameMngr.availableLevelSupply -= powerDemand;
			recharge ();
>>>>>>> origin/master
		}
	}

	/* If device group is turned on:
	 * if any are damaged, changed to damaged criteria.
	 * If true and others aren't make them inactive.	*/
	public void massCrashCheckForDevice ()
	{
		if (stateActive () && !stateDamaged () || stateOn () && !stateDamaged ()) 
		{
			stateActive(false);
			stateOn(false);
			changeRendColor (offColor);
		}
		else if (stateActive () && stateDamaged () || stateOn () && stateDamaged ()) 
		{
			stateDamaged (true);
			stateActive(false);
			stateOn (false);
			changeRendColor (damagedColor);
		}
	}

	public void machineStateMeterCheck ()
	{
		if (besideDevice) 
		{
			if (!stateActive ()) 
			{
				player.changeStateMeterColors (offColor);
			}
			else if (stateActive ()) 
			{
				player.changeStateMeterColors (activeColor);
			}
			else if (stateOn ()) 
			{
				player.changeStateMeterColors (neutralColor);
			}
			else if (stateDamaged ()) 
			{
				player.changeStateMeterColors (damagedColor);
			}
			else if (uploadComplete)
			{
				player.changeStateMeterColors (Color.blue);
			}
		}
		else 
		{
			player.changeStateMeterColors (Color.white);
		}
	}

	public void changeRendColor (Color32 colour)
	{
		deviceRend.material.color = colour;
	}
		
	public void drainerStateChangeCriteria ()
	{
		if (stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Device de-activated");
			changeRendColor (offColor);
			stateActive (false);
			box.roomSinglePowerDown (powerDemand);
			gameMngr.levelObjectPowerDown (powerDemand);
<<<<<<< HEAD
=======
//			gameMngr.availableLevelSupply += powerDemand;
>>>>>>> origin/master
		}
		else if (stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Turning damaged device off");
			changeRendColor (damagedColor);
			stateActive (false);
			box.roomSinglePowerDown (powerDemand);
			gameMngr.levelObjectPowerDown (powerDemand);
<<<<<<< HEAD
=======
//			gameMngr.availableLevelSupply += powerDemand;
>>>>>>> origin/master
		}
		else if (!stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Device activated");
			changeRendColor (activeColor);
			stateActive (true);
			box.roomSinglePowerUp (powerDemand);
			gameMngr.levelObjectPowerUp (powerDemand);
<<<<<<< HEAD
			gameMngr.objectsToCut.Add (this.gameObject);
=======
//			gameMngr.availableLevelSupply -= powerDemand;
>>>>>>> origin/master
		}
		else if (!stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Device caused a crash");
			changeRendColor (damagedColor);
			stateActive (true);
			box.roomCrash ();
		}
		else // Debug error with object's name.
			Debug.Log ("ObjectScript ChangeState Error" + this.name);
	}

	public void recharge ()
	{
		player.changeHealth (100f);
		player.changeEnergy (100f);
		player.changeOxygen (100f);
	}
}
