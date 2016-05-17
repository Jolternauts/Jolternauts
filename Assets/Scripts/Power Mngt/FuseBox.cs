using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FuseBox : ObjectClass 
{
	//this is for anything that concerns the fusebox

	public List<GameObject> roomObjects = new List<GameObject>();

	public RoomScript room;
<<<<<<< HEAD
=======

>>>>>>> origin/master
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

	/// Conditioning to basically force:
	/// Having to continue the power chain to turn on rooms, except with the first link.
	/// And being able to turn on empty rooms.
	public void fuseBoxRules ()
	{
		if (!stateActive ()) 
		{
			if (gameMngr.chainLinks.Count == 0) 
			{
				if (roomObjects.Count > 0)
				{
					if (room.availableRoomSupply > 0) 
					{
						changeState (this.gameObject);
					}
				}
				else
					changeState (this.gameObject);
			}
			else if (gameMngr.chainLinks.Count > 0)
			{
				activeChainRules ();
			}
		}
		else
			changeState (this.gameObject);
	}

	/// If this room has x neighbours:
	/// If the chain contains one of them:
	/// If there are devices, supply is needed to turn the box on.
	/// Else just turn it on.
	public void activeChainRules ()
	{
//		thisRoom = targetBox.GetComponentInParent<RoomScript> ();

		if (room.neighbours.Count == 1)
		{
			if (gameMngr.chainLinks.Contains (room.neighbours [0]))
			{
				if (roomObjects.Count > 0)
				{
					if (room.availableRoomSupply > 0) 
					{
						changeState (this.gameObject);
					}
				}
				else
					changeState (this.gameObject);
			}						
		}
		else if (room.neighbours.Count == 2)
		{
			if (gameMngr.chainLinks.Contains (room.neighbours [0]) || 
				gameMngr.chainLinks.Contains (room.neighbours [1]))
			{
				if (roomObjects.Count > 0)
				{
					if (room.availableRoomSupply > 0) 
					{
						changeState (this.gameObject);
					}
				}
				else
					changeState (this.gameObject);
			}						
		}
		else if (room.neighbours.Count == 3)
		{
			if (gameMngr.chainLinks.Contains (room.neighbours [0]) || 
				gameMngr.chainLinks.Contains (room.neighbours [1]) || 
				gameMngr.chainLinks.Contains (room.neighbours [2]))
			{
				if (roomObjects.Count > 0)
				{
					if (room.availableRoomSupply > 0) 
					{
						changeState (this.gameObject);
					}
				}
				else
					changeState (this.gameObject);
			}						
		}
		else if (room.neighbours.Count == 4)
		{
			if (gameMngr.chainLinks.Contains (room.neighbours [0]) || 
				gameMngr.chainLinks.Contains (room.neighbours [1]) || 
				gameMngr.chainLinks.Contains (room.neighbours [2]) || 
				gameMngr.chainLinks.Contains (room.neighbours [3]))
			{
				if (roomObjects.Count > 0)
				{
					if (room.availableRoomSupply > 0) 
					{
						changeState (this.gameObject);
					}
				}
				else
					changeState (this.gameObject);
			}						
		}
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
<<<<<<< HEAD
=======

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
>>>>>>> origin/master
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
<<<<<<< HEAD
			Debug.Log ("Room not Powered ");
=======
<<<<<<< HEAD
			Debug.Log ("Room not Powered ");
=======
>>>>>>> origin/master
>>>>>>> origin/master
		}
		else if (!stateActive()) 
		{
			room.isPowered = true;
<<<<<<< HEAD
			Debug.Log ("Room Powered ");
=======
<<<<<<< HEAD
			Debug.Log ("Room Powered ");
=======
>>>>>>> origin/master
>>>>>>> origin/master
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
<<<<<<< HEAD
			Debug.Log ("Doing the power up stuff");
			gameMngr.chainLinks.Add (room.here);
			room.callPowerTransfer ();
=======
<<<<<<< HEAD
			Debug.Log ("Doing the power up stuff");
//			powerChainIncrease ();
			gameMngr.chainLinks.Add (room.here);
			room.callPowerTransfer ();
=======
			powerChainIncrease ();
<<<<<<< HEAD
			room.callPowerTransfer ();
=======
<<<<<<< HEAD
			room.callPowerTransfer ();
=======
<<<<<<< HEAD
			room.callPowerTransfer ();
=======
			callPowerTransfer ();
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
			roomActivate ();
		} 
		else if (!room.isPowered)
		{
<<<<<<< HEAD
			Debug.Log ("Doing the power down stuff");
			breakChain ();
			gameMngr.chainLinks.Remove (room.here);
			room.hasReceivedSourcePower = false;
=======
<<<<<<< HEAD
			Debug.Log ("Doing the power down stuff");
=======
>>>>>>> origin/master
			gameMngr.chainLinks.Remove (room.here);
>>>>>>> origin/master
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
			GameObject device = roomObjects [x];
			if (device.tag == "GoalCon") 
			{
				PowerDrain goalCon = device.GetComponent<PowerDrain> ();
				if (goalCon.stateActive ()) 
				{
					goalCon.stateActive (false);
					goalCon.changeRendColor (offColor);
					roomSinglePowerDown (goalCon.powerDemand);
					gameMngr.levelObjectPowerDown (goalCon.powerDemand);
				}
			}

			if (device.tag == "HealthCon") 
			{
				PowerDrain healthCon = device.GetComponent<PowerDrain> ();
				if (healthCon.stateActive ()) 
				{
					healthCon.stateActive (false);
					healthCon.changeRendColor (offColor);
					roomSinglePowerDown (healthCon.powerDemand);
					gameMngr.levelObjectPowerDown (healthCon.powerDemand);
				}
			}

<<<<<<< HEAD
			if (device.tag == "ServerCon") 
			{
				PowerDrain serverCon = device.GetComponent<PowerDrain> ();
				if (serverCon.stateActive ()) 
				{
					serverCon.stateActive (false);
					serverCon.changeRendColor (offColor);
					roomSinglePowerDown (serverCon.powerDemand);
					gameMngr.levelObjectPowerDown (serverCon.powerDemand);
=======
				if (device.stateActive ()) 
				{
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
					device.stateActive (false);
					device.changeRendColor (offColor);
					roomSinglePowerDown (device.powerDemand);
					gameMngr.levelObjectPowerDown (device.powerDemand);
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
					device.changeState (device.gameObject);
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
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
			GameObject device = roomObjects [x];

			if (device.tag == "GoalCon") 
			{
				PowerDrain goalCon = device.GetComponent<PowerDrain> ();
				goalCon.massActivationCheck ();
			}

			if (device.tag == "HealthCon") 
			{
				PowerDrain healthCon = device.GetComponent<PowerDrain> ();
				healthCon.massActivationCheck ();
			}

			if (device.tag == "ServerCon") 
			{
				PowerDrain serverCon = device.GetComponent<PowerDrain> ();
				serverCon.massActivationCheck ();
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

<<<<<<< HEAD
			if (drainer.stateActive ()) 
			{
				roomSinglePowerUp (drainer.powerDemand);
				gameMngr.levelObjectPowerUp (drainer.powerDemand);
//				room.availableRoomSupply -= drainer.powerDemand;
//				gameMngr.availableLevelSupply -= drainer.powerDemand;
=======
			if (drainer.stateActive()) 
			{
				room.availableRoomSupply -= drainer.powerDemand;
				gameMngr.availableLevelSupply -= drainer.powerDemand;
>>>>>>> origin/master
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

			if (drainer.stateActive ()) 
			{
<<<<<<< HEAD
				roomSinglePowerDown (drainer.powerDemand);
				gameMngr.levelObjectPowerDown (drainer.powerDemand);
//				room.availableRoomSupply += drainer.powerDemand;
//				gameMngr.availableLevelSupply += drainer.powerDemand;
=======
				room.availableRoomSupply += drainer.powerDemand;
				gameMngr.availableLevelSupply += drainer.powerDemand;
>>>>>>> origin/master
			}
		}
	}

	/// Depending on the state of the box, change its factors accordingly.
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
		room.currentRoomDemand += demand;
	}

	/// Decreases the active room power values by a single active objects' values.
<<<<<<< HEAD
=======
	/// Also constrains the values to not fall below 0.
>>>>>>> origin/master
	public void roomSinglePowerDown (int demand)
	{
		room.availableRoomSupply += demand;
		room.currentRoomDemand -= demand;
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

<<<<<<< HEAD
	/// Increases/creates the chain.
	/// If there is no chain, just add a link.
	/// Else go thorugh this room's neighbours and the chain.
	/// If a neghbour is in the chain, add link.
	public void powerChainIncrease ()
=======
	// Store power supply from the room to the suit. 
	public void storePowerPackSupply ()
>>>>>>> origin/master
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

<<<<<<< HEAD
	/// Breaks the chain.
	/// Go thorugh the chain.
	/// If a room has a higher position.
	/// Turn it off too. 
	public void breakChain ()
=======
	// Share power supply from the suit to the room. 
	public void sharePowerPackSupply ()
>>>>>>> origin/master
	{
		for (int x = 0; x < gameMngr.chainLinks.Count; x++)
		{
			GameObject link = gameMngr.chainLinks [x];
			RoomScript linkScript = link.GetComponent<RoomScript> ();
			FuseBox linkBox = linkScript.roomFuseBox.GetComponent<FuseBox> ();

			if (linkScript.chainPos > room.chainPos) 
			{
				linkBox.changeState (linkBox.gameObject);
			}
		}
	}
<<<<<<< HEAD
=======

	public void powerChainIncrease ()
	{
		if (gameMngr.chainLinks.Count == 0) 
		{
			gameMngr.chainLinks.Add (room.here);
		}

<<<<<<< HEAD
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
=======
<<<<<<< HEAD
		 else if (gameMngr.chainLinks.Count > 0)
=======
<<<<<<< HEAD
		 else if (gameMngr.chainLinks.Count > 0)
=======
<<<<<<< HEAD
		 else if (gameMngr.chainLinks.Count > 0)
=======
		if (gameMngr.chainLinks.Count > 0)
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
		{
			#pragma warning disable
			for (int x = 0; x < room.doors.Count; x++)
			{
				DoorScript door;
				door = room.doors [x].GetComponent<DoorScript> ();
				if (door.isDirectionalReceiver)
				{
					gameMngr.chainLinks.Add (room.here);
				}
				break;
			}
			#pragma warning restore
		}
	}

<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
	public void callPowerTransfer ()
	{
		#pragma warning disable
		for (int x= 0; x < room.neighbours.Count; x++)
		{
			GameObject friend;
			RoomScript friendScript;
			friend = room.neighbours [x];
			friendScript = room.neighbours [x].GetComponent<RoomScript> ();
			for (int y = 0; y < friendScript.doors.Count; y++)
			{
				DoorScript door;
				door = friendScript.doors [y].GetComponent<DoorScript> ();
				if (door.isDirectionalReceiver && !friendScript.hasReceivedSourcePower)
				{
					room.transferPowerSupply (friend);
					friendScript.hasReceivedSourcePower = true;
				}
				break;
			}
		}
		#pragma warning restore
	}
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
}
