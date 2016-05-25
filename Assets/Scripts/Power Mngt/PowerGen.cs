using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerGen : ObjectClass 
{
	//this is for anything that supplies power

	public bool poweringUp;
	public bool besideGen;

	RoomScript room;
	FuseBox box;
	Renderer genRend;

	int trackedSupply;

	GameObject victim;
	LoopScript vicLoop;

	void Start ()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		box = room.roomFuseBox.GetComponent<FuseBox> ();
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		genRend = this.gameObject.GetComponent<Renderer> ();
	}

	void Update ()
	{
		
	}

	// Changes the state of generator.
	public void changeState (GameObject reference)
	{
		genStateChangeCriteria ();
	}

	void OnTriggerEnter (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideGen = true;
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
				if (!stateActive ()) 
				{
					powerUp ();
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
		
	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideGen = false;
			machineStateMeterCheck ();
		}
	}

	/// Wait function.
	public IEnumerator Stall ()
	{
		Debug.Log ("Buffering");
		yield return new WaitForSeconds (repairTime);
		changeState (this.gameObject);
	}

	public void powerUp ()
	{
		changeRendColor (neutralColor);
		StartCoroutine (Stall ());
	}

	public void machineStateMeterCheck ()
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
		if (stateActive () && !stateDamaged () || stateOn () && !stateDamaged ()) 
		{
			stateActive (false);
			stateOn (false);
			changeRendColor (offColor);
		}
		else if (stateActive () && stateDamaged () || stateOn () && stateDamaged ()) 
		{
			stateDamaged (true);
			stateActive (false);
			stateOn (false);
			changeRendColor (damagedColor);
		}
	}

	/// Depending on the state of the generator, change its factors accordingly.
	public void genStateChangeCriteria ()
	{
		if (stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Generator de-activated");
			changeRendColor (offColor);
			stateActive (false);
			room.availableRoomSupply -= powerSupply;
			box.roomSinglePowerDown (powerDemand);
			gameMngr.availableLevelSupply -= powerSupply;
			gameMngr.activeLevelDemand -= powerDemand;
			gameMngr.suppliers.Remove (this.gameObject);
			killEmAll ();
		}

		else if (stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Turning damaged generator off");
			changeRendColor (damagedColor);
			stateActive (false);
			room.availableRoomSupply -= powerSupply;
			box.roomSinglePowerDown (powerDemand);
			gameMngr.availableLevelSupply -= powerSupply;
			gameMngr.activeLevelDemand -= powerDemand;
			gameMngr.suppliers.Remove (this.gameObject);
			killEmAll ();
		}

		else if (!stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Generator activated");
			changeRendColor (activeColor);
			stateActive (true);
			room.availableRoomSupply += powerSupply;
			box.roomSinglePowerUp (powerDemand);
			gameMngr.availableLevelSupply += powerSupply;
			gameMngr.availableLevelSupply -= powerDemand;
			gameMngr.activeLevelDemand += powerDemand;
			gameMngr.suppliers.Add (this.gameObject);
		}

		else if (!stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Damn, generator crashed it!");
			changeRendColor (damagedColor);
			stateActive (true);
			box.roomCrash ();
		}
		else // Debug error with object's name.
			Debug.Log ("ObjectScript ChangeState Error" + this.name);		
	}


	public void killEmAll ()
	{		
		for (int a = 0; a < gameMngr.objectsToCut.Count; a++) 
		{
			victim = gameMngr.objectsToCut [a];
			vicLoop = victim.GetComponent<LoopScript> ();
			if (vicLoop.loop == "Tertiary")
			{
				if (victim.GetComponent<PowerDrain> ())
				{
					PowerDrain vicDev = victim.GetComponent<PowerDrain> ();
					if (gameMngr.objectsToCut.Count > 1) 
					{
						if ((gameMngr.supplyToCut + vicDev.powerDemand) <= powerSupply)
						{
							vicDev.changeState (vicDev.gameObject);
							gameMngr.objectsToCut.RemoveAt (a--);
							gameMngr.supplyToCut += vicDev.powerDemand;
							trackedSupply += vicDev.powerDemand;
						}
					}
					else
					{
						if ((gameMngr.supplyToCut + vicDev.powerDemand) <= powerSupply)
						{
							vicDev.changeState (vicDev.gameObject);
							gameMngr.objectsToCut.RemoveAt (a);
							gameMngr.supplyToCut += vicDev.powerDemand;
							trackedSupply += vicDev.powerDemand;
						}
					}
				}
			}
		}
		for (int b = 0; b < gameMngr.objectsToCut.Count; b++) 
		{
			victim = gameMngr.objectsToCut [b];
			vicLoop = victim.GetComponent<LoopScript> ();
			if (vicLoop.loop == "Secondary")
			{
				if (victim.GetComponent<DoorScript> ())
				{
					DoorScript vicDoor = victim.GetComponent<DoorScript> ();
					if (gameMngr.objectsToCut.Count > 1) 
					{
						if ((gameMngr.supplyToCut + vicDoor.powerDemand) <= powerSupply)
						{
							vicDoor.changeActiveState (vicDoor.gameObject);
							gameMngr.objectsToCut.RemoveAt (b--);
							gameMngr.supplyToCut += vicDoor.powerDemand;
							trackedSupply += vicDoor.powerDemand;
						}
					}
					else
					{
						if ((gameMngr.supplyToCut + vicDoor.powerDemand) <= powerSupply)
						{
							vicDoor.changeActiveState (vicDoor.gameObject);
							gameMngr.objectsToCut.RemoveAt (b);
							gameMngr.supplyToCut += vicDoor.powerDemand;
							trackedSupply += vicDoor.powerDemand;
						}
					}
				}
			}
		}
		for (int c = 0; c < gameMngr.objectsToCut.Count; c++) 
		{
			victim = gameMngr.objectsToCut [c];
			vicLoop = victim.GetComponent<LoopScript> ();
			if (vicLoop.loop == "Primary")
			{
				if (victim.GetComponent<PowerDrain> ())
				{
					PowerDrain vicGoal = victim.GetComponent<PowerDrain> ();
					if (gameMngr.objectsToCut.Count > 1) 
					{
						if ((gameMngr.supplyToCut + vicGoal.powerDemand) <= powerSupply)
						{
							vicGoal.changeState (vicGoal.gameObject);
							gameMngr.objectsToCut.RemoveAt (c--);
							gameMngr.supplyToCut += vicGoal.powerDemand;
							trackedSupply += vicGoal.powerDemand;
						}
					}
					else
					{
						if ((gameMngr.supplyToCut + vicGoal.powerDemand) <= powerSupply)
						{
							vicGoal.changeState (vicGoal.gameObject);
							gameMngr.objectsToCut.RemoveAt (c);
							gameMngr.supplyToCut += vicGoal.powerDemand;
							trackedSupply += vicGoal.powerDemand;
						}
					}
				}
			}
		}
		gameMngr.supplyToCut -= trackedSupply;
	}
}
