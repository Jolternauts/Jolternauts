using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuseBox : ObjectClass 
{
	//this is for anything that concerns the fusebox

	public List<GameObject> roomObjects = new List<GameObject>();

//	AngusMovement player;
//	GameManager gameMngr;
	RoomScript room;

	Renderer boxRend;

	public bool besideBox;

	void Start()
	{
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		gameMngr = GameManager.instance;
		room = this.gameObject.GetComponentInParent<RoomScript> ();		
		boxRend = this.gameObject.GetComponent<Renderer> ();
	}

	void Update()
	{

	}

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

	void OnTriggerEnter(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideBox = true;
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
				changeState (this.gameObject);
				statePressed (true);
			}

			if (Input.GetKeyUp (KeyCode.E)) 
			{
				statePressed (false);
			}

			if (Input.GetKeyDown (KeyCode.P) && !statePressed ()) 
			{
				if (room.availableRoomSupply > 0) 
				{
					storePowerPackSupply ();
					statePressed (true);
				}
			}

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

		machineStateMeterCheck ();
	}

	void OnTriggerExit(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideBox = false;
			machineStateMeterCheck ();
		}
	}

	public void storePowerPackSupply()
	{
		int spareSupply = room.availableRoomSupply;
		player.powerPack += spareSupply;
		room.availableRoomSupply -= spareSupply;

		Debug.Log ("Storing Power");
	}

	public void sharePowerPackSupply()
	{
		int requiredSupply = room.totalRoomDemand;
		room.availableRoomSupply += requiredSupply;
		player.powerPack -= requiredSupply;

		Debug.Log ("Sharing Power");
	}
		
	/// <summary>
	/// Changes active state of a room.
	/// </summary>
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

	/// <summary>
	/// Determines what to do when the Fusebox switch is flipped.
	/// When turning it ON, Activate.
	/// When turning it OFF, Sleep.
	/// </summary>
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

	/// <summary>
	/// Turns off the room.
	/// </summary>
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

	/// <summary>
	/// Turns room ON.
	/// Checks if any objects are damaged/neutral.
	/// 
	/// If neutral and not damaged, objects and room turn ON fine and values change accordingly.
	/// Also calls function to plus power values by active room values. 
	/// 
	/// If neutral but damaged, function is called to crash room.
	/// </summary>
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
		
	/// <summary>
	/// Room turns OFF, objects turn OFF and values change accordingly.
	/// These changes include damaging the Fusebox and identifying the culprit which caused the crash.
	/// </summary>
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

	/// <summary>
	/// Increases the active room power values by the sum of the active objects' values.
	/// </summary>
	public void roomMassPowerUp()
	{
		for (int x = 0; x < roomObjects.Count; x++) 
		{
			ObjectClass massScript = roomObjects [x].GetComponent<ObjectClass> ();
//			PowerDrain drainer = roomObjects [x].GetComponent<PowerDrain> ();

			if (massScript.stateActive()) 
			{
				room.availableRoomSupply -= massScript.powerDemand;
				room.currentRoomDemand += massScript.powerDemand;
			}
		}
	}

	/// <summary>
	/// Decreases the active room power values by the sum of the active objects' values.
	/// Also constrains the values to not fall below 0.
	/// </summary>
	public void roomMassPowerDown()
	{
		gameMngr.levelRoomPowerDown (room.currentRoomDemand);

		for (int x = 0; x < roomObjects.Count; x++) 
		{
			PowerDrain drainer = roomObjects [x].GetComponent<PowerDrain> ();
			PowerGen generator = roomObjects [x].GetComponent<PowerGen> ();

			if (drainer.stateActive()) 
			{
				room.availableRoomSupply += drainer.powerDemand;
				room.currentRoomDemand -= drainer.powerDemand;
			}

			if (generator.stateActive()) 
			{
				room.availableRoomSupply += generator.powerDemand;
				room.availableRoomSupply -= generator.powerSupply;
				room.currentRoomDemand -= generator.powerDemand;
			}
		}
	}

	/// <summary>
	/// Increases the active room power values by a single active objects' values.
	/// </summary>
	public void roomSinglePowerUp(int demand)
	{
		gameMngr.levelObjectPowerUp (demand);
		room.availableRoomSupply -= demand;
		room.currentRoomDemand += demand;
	}

	/// <summary>
	/// Decreases the active room power values by a single active objects' values.
	/// Also constrains the values to not fall below 0.
	/// </summary>
	public void roomSinglePowerDown(int demand)
	{
		gameMngr.levelObjectPowerDown (demand);
		room.availableRoomSupply += demand;
		room.currentRoomDemand -= demand;
	}

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

	public void changeRendColor(Color32 colour)
	{
		boxRend.material.color = colour;
	}
}
