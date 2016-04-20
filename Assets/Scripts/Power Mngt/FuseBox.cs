using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuseBox : ObjectClass 
{
	//this is for anything that concerns the fusebox

	public List<GameObject> roomObjects = new List<GameObject>();

	RoomScript room;
	CompassScript compass;

	Renderer boxRend;

	public bool besideBox;

	void Start()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		gameMngr = GameManager.instance;
		room = this.gameObject.GetComponentInParent<RoomScript> ();		
		boxRend = this.gameObject.GetComponent<Renderer> ();
		compass = room.GetComponentInChildren<CompassScript> ();
	}

	void Update()
	{

	}

	// If there is available room supply, change the box's state.
	public void changeState(GameObject reference)
	{
		if (room.availableRoomSupply > 0) 
		{
			roomStateCheck ();

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
	}

	// When player enters trigger:
	// He is beside this.
	// Call meter color change function.
	void OnTriggerEnter(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideBox = true;
			machineStateMeterCheck ();
		}
	}

	// While inside trigger.
	void OnTriggerStay(Collider detector)
	{
		// Detect player at object
		if (detector.transform.tag == "Player")
		{
			// If E is pressed turn it on.
			if (Input.GetKeyDown (KeyCode.E) && !statePressed ()) 
			{
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

			if (stateActive())
			{
				// Press T: call transfer supply -> north.
				if (Input.GetKeyDown (KeyCode.T) && room.north && !statePressed ()) 
				{
					room.transferPowerSupply(room.north);
					Debug.Log ("Power moved north to " + room.north.name);
					statePressed (true);
				}

				// Press H: call transfer supply -> east.
				if (Input.GetKeyDown (KeyCode.H) && room.east && !statePressed ()) 
				{
					room.transferPowerSupply(room.east);
					Debug.Log ("Power moved east to " + room.east.name);
					statePressed (true);
				}

				// Press G: call transfer supply -> south.
				if (Input.GetKeyDown (KeyCode.G) && room.south && !statePressed ()) 
				{
					room.transferPowerSupply(room.south);
					Debug.Log ("Power moved south to " + room.south.name);
					statePressed (true);
				}

				// Press F: call transfer supply -> west.
				if (Input.GetKeyDown (KeyCode.F) && room.west && !statePressed ()) 
				{
					room.transferPowerSupply(room.west);
					Debug.Log ("Power moved west to " + room.west.name);
					statePressed (true);
				}
			}

			if (Input.GetKeyUp (KeyCode.T) || Input.GetKeyUp (KeyCode.H) ||
				Input.GetKeyUp (KeyCode.G) || Input.GetKeyUp (KeyCode.F)) 
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
	void OnTriggerExit(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideBox = false;
			machineStateMeterCheck ();
		}
	}

	// Store power supply from the room to the suit. 
	public void storePowerPackSupply()
	{
		int spareSupply = room.availableRoomSupply;
		player.powerPack += spareSupply;
		room.availableRoomSupply -= spareSupply;

		Debug.Log ("Storing Power");
	}

	// Share power supply from the suit to the room. 
	public void sharePowerPackSupply()
	{
		int requiredSupply = room.totalRoomDemand;
		room.availableRoomSupply += requiredSupply;
		player.powerPack -= requiredSupply;

		Debug.Log ("Sharing Power");
	}
		
	/// Checks and changes active state of a room.
	public void roomStateCheck()
	{
		Debug.Log ("Room Check Called");

		if (stateActive()) 
		{
			room.isPowered = false;
		}
		else if (!stateActive()) 
		{
			room.isPowered = true;
		}
		roomStateChange ();
	}

	/// Determines what to do when the Fusebox switch is flipped.
	/// When turning it ON, Activate.
	/// When turning it OFF, Sleep.
	public void roomStateChange()
	{
		Debug.Log ("Room State Change Called");

		if (room.isPowered) 
		{
			roomActivate ();
		} 
		else
		{
			for (int x = 0; x < roomObjects.Count; x++) 
			{
				if (roomObjects [x].tag == "Device") 
				{
					PowerDrain device = roomObjects [x].GetComponent<PowerDrain> ();

					for (int y = 0; y < roomObjects.Count; y++) 
					{
						if (device.stateActive ()) 
						{
							roomSleep ();
						}
					}
				}
			}
		}
	}

	/// Turns off the room.
	public void roomSleep()
	{
		Debug.Log ("Room Sleep Called");

		// Go through room items list.
		for (int x = 0; x < roomObjects.Count; x++)  
		{
			if (roomObjects [x].tag == "Device") 
			{
				PowerDrain device = roomObjects [x].GetComponent<PowerDrain> ();

				device.stateActive (false);
				device.changeRendColor (offColor);
				roomSinglePowerDown (device.powerDemand);
			}
		}
	}

	/// Turns room ON.
	/// Calls check for if device could be damaged.
	public void roomActivate()
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
	public void roomCrash()
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
	public void roomMassPowerUp()
	{
		for (int x = 0; x < roomObjects.Count; x++) 
		{
			ObjectClass massScript = roomObjects [x].GetComponent<ObjectClass> ();

			if (massScript.stateActive()) 
			{
				room.availableRoomSupply -= massScript.powerDemand;
				gameMngr.availableLevelSupply -= massScript.powerDemand;
			}
		}
	}

	/// Decreases the active room power values by the sum of the active objects' values.
	/// Also constrains the values to not fall below 0.
	public void roomMassPowerDown()
	{
		for (int x = 0; x < roomObjects.Count; x++) 
		{
			PowerDrain drainer = roomObjects [x].GetComponent<PowerDrain> ();
			PowerGen generator = roomObjects [x].GetComponent<PowerGen> ();

			if (drainer.stateActive()) 
			{
				room.availableRoomSupply += drainer.powerDemand;
				gameMngr.availableLevelSupply += drainer.powerDemand;
			}

			if (generator.stateActive()) 
			{
				room.availableRoomSupply += generator.powerDemand;
				room.availableRoomSupply -= generator.powerSupply;
			}
		}
	}

	/// Increases the active room power values by a single active objects' values.
	public void roomSinglePowerUp(int demand)
	{
		room.availableRoomSupply -= demand;
	}

	/// Decreases the active room power values by a single active objects' values.
	/// Also constrains the values to not fall below 0.
	public void roomSinglePowerDown(int demand)
	{
		room.availableRoomSupply += demand;
	}

	// If player is beside box (or not), depending on its state, change state meters colors accordingly.
	public void machineStateMeterCheck()
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
	public void changeRendColor(Color32 colour)
	{
		boxRend.material.color = colour;
	}


}
