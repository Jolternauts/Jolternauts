using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerGen : ObjectClass 
{
	//this is for anything that supplies power

	List <GameObject> objectsToCut = new List <GameObject> ();

	public bool poweringUp;
	public bool besideGen;

	public int genListPos;
	int supplyToCut;

	RoomScript room;
	FuseBox box;
	Renderer genRend;

	GameObject victim;
	GameObject victimBox;
	FuseBox victimBoxScript;
	RoomScript victimScript;

	PowerGen priLoopReactor;
	PowerDrain priLoopGoal;
	DoorScript secLoopDoor;
	PowerGen secLoopGen;
	PowerDrain terLoopDevice;


	void Start()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		box = room.roomFuseBox.GetComponent<FuseBox> ();
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		genRend = this.gameObject.GetComponent<Renderer> ();
	}

	void Update()
	{
		if (gameMngr.suppliers.Contains (this.gameObject)) 
		{
			genListPos = gameMngr.chainLinks.IndexOf (this.gameObject) + 1;
		} 
		else
			genListPos = 0;
		
	}

	// Changes the state of generator.
	public void changeState(GameObject reference)
	{
		genStateChangeCriteria ();
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
		
	void OnTriggerExit(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideGen = false;
			machineStateMeterCheck ();
		}
	}

	/// Wait function.
	public IEnumerator Stall()
	{
		Debug.Log ("Buffering");
		yield return new WaitForSeconds (repairTime);
		changeState (this.gameObject);
	}

	public void powerUp()
	{
		changeRendColor (neutralColor);
		StartCoroutine (Stall ());
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

	/// Depending on the state of the generator, change its factors accordingly.
	public void genStateChangeCriteria ()
	{
		if (stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Generator de-activated");
			changeRendColor (offColor);
			stateActive (false);
			gameMngr.availableLevelSupply -= powerSupply;
			gameMngr.activeLevelDemand -= powerDemand;
			room.availableRoomSupply -= powerSupply;
			box.roomSinglePowerDown (powerDemand);
			gameMngr.suppliers.Remove (this.gameObject);
//			killEmAll ();
		}

		else if (stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Turning damaged generator off");
			changeRendColor (damagedColor);
			stateActive (false);
			gameMngr.availableLevelSupply -= powerSupply;
			gameMngr.activeLevelDemand -= powerDemand;
			room.availableRoomSupply -= powerSupply;
			box.roomSinglePowerDown (powerDemand);
			gameMngr.suppliers.Remove (this.gameObject);
//			killEmAll ();
		}

		else if (!stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Generator activated");
			changeRendColor (activeColor);
			stateActive (true);
			gameMngr.availableLevelSupply += powerSupply;
			gameMngr.availableLevelSupply -= powerDemand;
			gameMngr.activeLevelDemand += powerDemand;
			room.availableRoomSupply += powerSupply;
			box.roomSinglePowerUp (powerDemand);
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
		if (powerSupply > supplyToCut)
		{
			for (int x = 0; x < gameMngr.tier3.Count; x++)
			{
				terLoopDevice = gameMngr.tier3 [x].GetComponent<PowerDrain> ();
				if (terLoopDevice.stateActive())
				{
					objectsToCut.Add (terLoopDevice.gameObject);
					supplyToCut += terLoopDevice.powerDemand;
				}
			}
		}
	}
}
