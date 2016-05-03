using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuseBox : ObjectClass 
{
	//this is for anything that concerns the fusebox

	public List<GameObject> roomObjects = new List<GameObject>();

	public RoomScript room;

	Renderer boxRend;

	public bool besideBox;

	void Start ()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		gameMngr = GameManager.instance;
		room = this.gameObject.GetComponentInParent<RoomScript> ();		
		boxRend = this.gameObject.GetComponent<Renderer> ();
	}

	void Update ()
	{

	}

	// If there is available room supply, change the box's state.
	public void changeState (GameObject reference)
	{
		roomStateCheck ();
		boxStateChangeCriteria ();
	}

	// When player enters trigger:
	// He is beside this.
	// Call meter color change function.
	void OnTriggerEnter (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideBox = true;
			machineStateMeterCheck ();
		}
	}

	// While inside trigger.
	void OnTriggerStay (Collider detector)
	{
		// Detect player at object
		if (detector.transform.tag == "Player")
		{
			// If E is pressed turn it on.
			if (Input.GetKeyDown (KeyCode.E) && !statePressed ()) 
			{
				if (!stateActive ()) 
				{
					if (room.availableRoomSupply > 0) 
					{
						changeState (this.gameObject);
					}
				}
				else
					changeState (this.gameObject);

				statePressed (true);
			}
				
			if (Input.GetKeyUp (KeyCode.E)) 
			{
				statePressed (false);
			}

			// If A.R.S > 0, store power supply from the room in the suit. 
			if (Input.GetKeyDown (KeyCode.P) && !statePressed ()) 
			{
				if (room.availableRoomSupply > 0) 
				{
					storePowerPackSupply ();
					statePressed (true);
				}
			}

			// If A.R.S is 0, share power supply from the suit to the room. 
			if (Input.GetKey (KeyCode.LeftShift) && Input.GetKeyDown (KeyCode.P) && !statePressed ()) 
			{
				if (room.availableRoomSupply == 0) 
				{
					sharePowerPackSupply ();
					statePressed (true);
				}
			}

			if (Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.P)) 
			{
				statePressed (false);
			}
		}

		// Check what color the statemeters should be.
		machineStateMeterCheck ();
	}

	// When player leaves trigger:
	// Player is not beside.
	// Check what color the statemeters should be.
	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideBox = false;
			machineStateMeterCheck ();
		}
	}
		
	/// Checks and changes active state of a room.
	public void roomStateCheck ()
	{
		Debug.Log ("Room Check Called");

		// Turn off room.
		// Remove it from Chain Links.
		if (stateActive()) 
		{
			room.isPowered = false;
			Debug.Log ("Room not Powered ");
		}
		else if (!stateActive()) 
		{
			room.isPowered = true;
			Debug.Log ("Room Powered ");
		}
		roomStateChange ();
	}

	/// Determines what to do when the Fusebox switch is flipped.
	/// When turning it ON, Activate.
	/// When turning it OFF, Sleep.
	public void roomStateChange ()
	{
		Debug.Log ("Room State Change Called");

		if (room.isPowered) 
		{
			Debug.Log ("Doing the power up stuff");
//			powerChainIncrease ();
			gameMngr.chainLinks.Add (room.here);
			room.callPowerTransfer ();
			roomActivate ();
		} 
		else if (!room.isPowered)
		{
			Debug.Log ("Doing the power down stuff");
			gameMngr.chainLinks.Remove (room.here);
			roomSleep ();
		}
	}

	/// Turns off the room.
	public void roomSleep ()
	{
		Debug.Log ("Room Sleep Called");

		// Go through room items list.
		for (int x = 0; x < roomObjects.Count; x++)  
		{
			if (roomObjects [x].tag == "Device") 
			{
				PowerDrain device = roomObjects [x].GetComponent<PowerDrain> ();

				if (device.stateActive ()) 
				{
					device.stateActive (false);
					device.changeRendColor (offColor);
					roomSinglePowerDown (device.powerDemand);
					gameMngr.levelObjectPowerDown (device.powerDemand);
				}
			}
		}
	}

	/// Turns room ON.
	/// Calls check for if device could be damaged.
	public void roomActivate ()
	{
		Debug.Log ("Room Activate Called");

		for (int x = 0; x < roomObjects.Count; x++) 
		{
			if (roomObjects [x].tag == "Device") 
			{
				PowerDrain device = roomObjects [x].GetComponent<PowerDrain> ();

				for (int y = 0; y < roomObjects.Count; y++) 
				{
					device.massActivationCheck ();
				}
			}
		}
	}
		
	/// Room turns OFF, objects turn OFF and values change accordingly.
	/// These changes include damaging the Fusebox and identifying the culprit which caused the crash.
	public void roomCrash ()
	{
		Debug.Log ("Oops! Room Crash Called");

		roomMassPowerDown ();

		stateDamaged (true);
		stateActive (false);
		changeRendColor (damagedColor);

		for (int x = 0; x < roomObjects.Count; x++) 
		{
			ObjectClass massScript = roomObjects [x].GetComponent<ObjectClass> ();
			PowerDrain drain = massScript.gameObject.GetComponent<PowerDrain> ();

			drain.massCrashCheckForDevice ();
		}
	}

	/// Increases the active room power values by the sum of the active objects' values.
	public void roomMassPowerUp ()
	{
		for (int x = 0; x < roomObjects.Count; x++) 
		{
			PowerDrain drainer = roomObjects [x].GetComponent<PowerDrain> ();

			if (drainer.stateActive()) 
			{
				room.availableRoomSupply -= drainer.powerDemand;
				gameMngr.availableLevelSupply -= drainer.powerDemand;
			}
		}
	}

	/// Decreases the active room power values by the sum of the active objects' values.
	/// Also constrains the values to not fall below 0.
	public void roomMassPowerDown ()
	{
		for (int x = 0; x < roomObjects.Count; x++) 
		{
			PowerDrain drainer = roomObjects [x].GetComponent<PowerDrain> ();

			if (drainer.stateActive()) 
			{
				room.availableRoomSupply += drainer.powerDemand;
				gameMngr.availableLevelSupply += drainer.powerDemand;
			}
		}
	}

	public void boxStateChangeCriteria ()
	{
		if (stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Your box is a turn off");
			changeRendColor (offColor);
			stateActive (false);
		}
		else if (stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Turning damaged box off");
			changeRendColor (damagedColor);
			stateActive (false);
		}
		else if (!stateActive () && !stateDamaged ()) 
		{
			Debug.Log ("Much turn on, Much WOW");
			changeRendColor (activeColor);
			stateActive (true);
		}
		else if (!stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Damn, the box crashed it");
			changeRendColor (damagedColor);
			stateActive (true);
			roomCrash ();
		} 
		else // Debug error with object's name.
			Debug.Log ("ObjectScript ChangeState Error" + this.name);		

	}

	/// Increases the active room power values by a single active objects' values.
	public void roomSinglePowerUp (int demand)
	{
		room.availableRoomSupply -= demand;
	}

	/// Decreases the active room power values by a single active objects' values.
	/// Also constrains the values to not fall below 0.
	public void roomSinglePowerDown (int demand)
	{
		room.availableRoomSupply += demand;
	}

	// If player is beside box (or not), depending on its state, change state meters colors accordingly.
	public void machineStateMeterCheck ()
	{
		if (besideBox) 
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

	// Change color of box.
	public void changeRendColor (Color32 colour)
	{
		boxRend.material.color = colour;
	}

	// Store power supply from the room to the suit. 
	public void storePowerPackSupply ()
	{
		int spareSupply = room.availableRoomSupply;
		player.powerPack += spareSupply;
		room.availableRoomSupply -= spareSupply;

		Debug.Log ("Storing Power");
	}

	// Share power supply from the suit to the room. 
	public void sharePowerPackSupply ()
	{
		int requiredSupply = room.totalRoomDemand;
		room.availableRoomSupply += requiredSupply;
		player.powerPack -= requiredSupply;

		Debug.Log ("Sharing Power");
	}

	public void powerChainIncrease ()
	{
		if (gameMngr.chainLinks.Count == 0) 
		{
			gameMngr.chainLinks.Add (room.here);
		}

		 else if (gameMngr.chainLinks.Count > 0)
		{
			for (int x = 0; x < room.neighbours.Count; x++) 
			{
				GameObject friend = room.neighbours [x];
				for (int y = 0; y < gameMngr.chainLinks.Count; y++)
				{
					GameObject link = gameMngr.chainLinks [y];
					if (link == friend)
					{
						gameMngr.chainLinks.Add (room.here);
					}
				}
			}
		}
	}
}
