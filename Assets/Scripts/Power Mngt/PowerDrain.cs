using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerDrain : ObjectClass
{
	//this is for anything that demands power

	//this tracks the devices max demandable power
//	public int powerDemand;

//	AngusMovement player;
	RoomScript room;
	FuseBox box;
	Renderer deviceRend;

	public bool besideDevice;

	void Start()
	{
//		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		box = room.roomFuseBox.GetComponent<FuseBox> ();
		deviceRend = this.gameObject.GetComponent<Renderer> ();
	}

	void Update()
	{
		
	}

	public void changeState(GameObject reference)
	{
		if (box.stateActive ()) 
		{
			if (room.availableRoomSupply > 0) 
			{
				if (stateActive () && !stateDamaged ()) 
				{
					Debug.Log ("Device de-activated");
					changeRendColor (offColor);
					stateActive (false);
					box.roomSinglePowerDown (powerDemand);
				}
				else if (stateActive () && stateDamaged ()) 
				{
					Debug.Log ("Turning damaged device off");
					changeRendColor (damagedColor);
					stateActive (false);
					box.roomSinglePowerDown (powerDemand);
				}
				else if (!stateActive () && !stateDamaged ()) 
				{
					Debug.Log ("Device activated");
					changeRendColor (activeColor);
					stateActive (true);
					box.roomSinglePowerUp (powerDemand);
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
		} 
		else
		{ // If the room isn't powered:
			/* If the object is not Neutral: (That means not off but with no power reaching it anyway.)
				 * Debug.
				 * Change its color.
				 * Turn it to Neutral !!!.
				*/
			if (!stateOn ()) 
			{
				Debug.Log ("Device neutral");
				changeRendColor (neutralColor);
				stateOn (true);
			}

			/* If the object is Neutral:
				 * Debug.
				 * Change its color.
				 * Turn it to OFF !!!.
				*/
			else if (stateOn ()) 
			{
				Debug.Log ("Device off");
				changeRendColor (offColor);
				stateOn (false);
			} 
			else // Debug error with object's name.
				Debug.Log ("ObjectScript ChangeState Error" + this.name);
		}
	}

	void OnTriggerEnter(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideDevice = true;
			machineStateMeterCheck ();
		}
	}

	void OnTriggerStay(Collider detector)
	{
		// Detect player at object and turn it on by pressing E.
		// If it's a charge station: replenish health, suit power and oxygen.

		if (detector.transform.tag == "Player")
		{
			if (Input.GetKeyDown (KeyCode.E) && !statePressed ()) 
			{
				if (myName == "ChargeStation") 
				{
					player.changeHealth (100f);
					player.changeEnergy (100f);
					player.changeOxygen (100f);
				}
				changeState(this.gameObject);
				statePressed (true);
			}

			if (Input.GetKeyUp (KeyCode.E)) 
			{
				statePressed(false);
			}
		}

		machineStateMeterCheck ();
	}

	void OnTriggerExit(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideDevice = false;
			machineStateMeterCheck ();
		}
	}

	public void massActivationCheck()
	{
		if (!stateDamaged() && stateOn() && room.availableRoomSupply - powerDemand >= 0)
		{
			Debug.Log ("Safe Activation for " + myName);
			stateActive(true);
			stateOn(false);
			changeRendColor (activeColor);
			box.roomSinglePowerUp (powerDemand);
		}
		else if (stateDamaged() && stateOn()) 
		{
			box.roomCrash ();
			Debug.Log (myName + " is Damaged");
			Debug.Log ("And now Fusebox is Damaged");
		} 
	}

	public void machineStateMeterCheck()
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
		}

		else 
		{
			player.changeStateMeterColors (Color.white);
		}
	}

	public void changeRendColor(Color32 colour)
	{
		deviceRend.material.color = colour;
	}

	public void massCrashCheckForDevice()
	{
		if (stateActive() && !stateDamaged() || stateOn() && !stateDamaged()) 
		{
			stateActive(false);
			stateOn(false);
			changeRendColor (offColor);
		}
		else if (stateActive() && stateDamaged() || stateOn() && stateDamaged()) 
		{
			stateDamaged (true);
			stateActive(false);
			stateOn(false);
			changeRendColor (damagedColor);
		}
	}
}
