using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerGen : ObjectClass 
{
	//this is for anything that supplies power

	//this tracks the devices max suppliable power
//	public int powerSupply;

	//this tracks the devices max demandable power
//	public int powerDemand;

	public bool poweringUp;
	public bool besideGen;

	RoomScript room;
	FuseBox box;
//	GameManager gameMngr;
//	AngusMovement player;
	Renderer genRend;


	void Start()
	{
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		box = room.roomFuseBox.GetComponent<FuseBox> ();
//		gameMngr = GameManager.instance;
//		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		genRend = this.gameObject.GetComponent<Renderer> ();

	}

	void Update()
	{
		if (poweringUp) 
		{
			powerUp ();
		}

	}

	public void changeState(GameObject reference)
	{
		if (stateActive() && !stateDamaged()) 
		{
			Debug.Log ("Generator de-activated");
			changeRendColor (offColor);
			stateActive (false);
			gameMngr.availableLevelSupply -= powerSupply;
			room.availableRoomSupply -= powerSupply;
			box.roomSinglePowerDown (powerDemand);
		}

		else if (stateActive() && stateDamaged()) 
		{
			Debug.Log ("Turning damaged generator off");
			changeRendColor (damagedColor);
			stateActive (false);
			gameMngr.availableLevelSupply -= powerSupply;
			room.availableRoomSupply -= powerSupply;
			box.roomSinglePowerDown (powerDemand);
		}

		else if (!stateActive() && !stateDamaged()) 
		{
			Debug.Log ("Generator activated");
			changeRendColor (activeColor);
			stateActive (true);
			gameMngr.availableLevelSupply += powerSupply;
			room.availableRoomSupply += powerSupply;
			box.roomSinglePowerUp (powerDemand);
			poweringUp = false;
		}

		else if (!stateActive() && stateDamaged()) 
		{
			Debug.Log ("Damn, generator crashed it!");
			changeRendColor (damagedColor);
			stateActive (true);
			box.roomCrash ();
		}
		else // Debug error with object's name.
			Debug.Log ("ObjectScript ChangeState Error" + this.name);
	}

	void OnTriggerEnter(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideGen = true;
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
				if (!stateActive ()) 
				{
					poweringUp = true;
				} 
				else
					changeState (this.gameObject);
				
				statePressed (true);
			}

			if (Input.GetKeyUp (KeyCode.E)) 
			{
				statePressed (false);
			}
		}

		machineStateMeterCheck ();

	}
		
	void OnTriggerExit(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideGen = false;
			machineStateMeterCheck ();
		}
	}

	public void powerUp()
	{
		// Change stateMeter2 color. (StateMeter2 is Object UI for the action of activating a power supplier).
		// Fill stateMeter2 over time. (Duration depends on object's individual activation time (as represented by repair time)).
		gameMngr.machineStateMeter2.color = activeColor;
		gameMngr.machineStateMeter2.fillAmount += 1.0f / repairTime * Time.deltaTime;

		/* When meter is filled:
		 * Empty the meter.
		 * Change Object state.
		 */
		if (gameMngr.machineStateMeter2.fillAmount == 1.0f) 
		{
			Debug.Log ("Meter filled, attempt changeState");
			gameMngr.machineStateMeter2.fillAmount = 0f;
			changeState (this.gameObject);
			poweringUp = false;
		}
	}

	public void machineStateMeterCheck()
	{
		if (besideGen) 
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

	public void changeRendColor (Color32 colour)
	{
		genRend.material.color = colour;
	}

	public void massCrashCheckForGen ()
	{
		if (stateActive() && !stateDamaged() || stateOn() && !stateDamaged()) 
		{
			stateActive (false);
			stateOn (false);
			changeRendColor (offColor);
		}
		else if (stateActive() && stateDamaged() || stateOn() && stateDamaged()) 
		{
			stateDamaged (true);
			stateActive (false);
			stateOn (false);
			changeRendColor (damagedColor);
		}
	}
}
