using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour 
{		
	public GameObject floorGravity;
	public GameObject ceilingGravity;

	public GameObject roomFuseBox;
	public GameObject north;
	public GameObject east;
	public GameObject south;
	public GameObject west;
	public GameObject northDoor;
	public GameObject eastDoor;
	public GameObject southDoor;
	public GameObject westDoor;
	public GameObject compass;
	public GameObject here;

	GameObject supplier;

	public List<GameObject> roomItems = new List<GameObject>();
	public List<GameObject> doors = new List<GameObject>();
	public List<GameObject> neighbours = new List<GameObject>();

	public bool isPowered = false;
	public bool playerIsHere = false;
	public bool hasReceivedSourcePower = false;

	bool runOnce = false;

	GameManager gameMngr;
	AngusMovement player;
	public RoomScript northScript;
	public RoomScript eastScript;
	public RoomScript southScript;
	public RoomScript westScript;
	public DoorScript northDoorScript;
	public DoorScript eastDoorScript;
	public DoorScript southDoorScript;
	public DoorScript westDoorScript;

	CompassScript locator;

	public int totalRoomSupply;
	public int availableRoomSupply;

	public int totalRoomDemand;
	public int currentRoomDemand;
	public int chainPos = 0;

	//These are here for shortcutted reference to initiate functions or set values on Start-up.
	void Start()
	{	
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		locator = compass.GetComponent<CompassScript> ();
		this.GetComponent<BoxCollider> ().isTrigger = true;
		here = this.gameObject;
		tallyTotalRoomPower ();
		directionSetup ();
    }

	void Update ()
	{
		if (availableRoomSupply < 0 || currentRoomDemand < 0) 
		{
			availableRoomSupply = 0;
			currentRoomDemand = 0;
		}


		if (playerIsHere) 
		{
			player.room = this.gameObject.GetComponent<RoomScript> ();
		}

		if (gameMngr.chainLinks.Contains (this.gameObject)) 
		{
			chainPos = gameMngr.chainLinks.IndexOf (this.gameObject) + 1;
		} 
		else
			chainPos = 0;

    }

	// Checks what direction a neighbouring room is in and accesses their script.
	public void directionSetup ()
	{
		if (north) 
		{
			northScript = north.GetComponent<RoomScript> ();
			northDoorScript = northDoor.GetComponent<DoorScript> ();
		}
		if (east)
		{
			eastScript = east.GetComponent<RoomScript> ();
			eastDoorScript = eastDoor.GetComponent<DoorScript> ();
		}
		if (south)
		{
			southScript = south.GetComponent<RoomScript> ();
			southDoorScript = southDoor.GetComponent<DoorScript> ();
		}
		if (west)
		{
			westScript = west.GetComponent<RoomScript> ();
			westDoorScript = westDoor.GetComponent<DoorScript> ();
		}
	}
		
	/// Detects player entering room & updates power UI.
	void OnTriggerEnter (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			if (!runOnce) 
			{
				runOnce = true;
				playerIsHere = true;
				roomStateCheck ();
				gameMngr.updateRoomUI(totalRoomSupply, totalRoomDemand, availableRoomSupply, currentRoomDemand);
			}
		}
	}

	/// Detects player still in room.
	/// Runs roomStateCheck function.
	/// If Fusebox is Active, Room is Active.
	void OnTriggerStay (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			playerIsHere = true;
			roomStateCheck ();
			gameMngr.updateRoomUI(totalRoomSupply, totalRoomDemand, availableRoomSupply, currentRoomDemand);

			if (this.roomFuseBox.GetComponent<ObjectClass>().stateActive())
			{
				this.isPowered = true;
			}
		}
	}

	/// Detects player leaving room & updates power UI.
	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player")
		{
            runOnce = false;
			playerIsHere = false;
       }
	}
		
	/// Check active state of room.
	/// If the room is inactive oxygen & health are depleted.
	public void roomStateCheck ()
	{
		if (isPowered) 
		{

		} 
		else 
		{
			player.currentOxygen -= player.oxygenDown * Time.deltaTime;

			if (player.currentOxygen <= 0f) 
			{
				player.currentHealth -= player.healthDown * Time.deltaTime;
			}
		}
	}

	/// Tallies the total room power.
	/// It is summed up by the values of every device in the room.
	public void tallyTotalRoomPower ()
	{
		FuseBox box = roomFuseBox.GetComponent<FuseBox> ();
		ObjectClass machine;
		for (int x = 0; x < box.roomObjects.Count; x++) 
		{
			machine = box.roomObjects [x].GetComponent<ObjectClass> ();
			totalRoomSupply += machine.powerSupply;
			totalRoomDemand += machine.powerDemand;
		}
	}

	/// Transfers the power supply in the designated direction.
	/// Deducts that room's demand from the room with supply.
	public void transferPowerSupply(GameObject direction)
	{
		RoomScript directionScript = direction.GetComponent<RoomScript> ();

		int requiredSupply = directionScript.totalRoomDemand;
		if (gameMngr.availableLevelSupply > 0) 
		{
			if (gameMngr.availableLevelSupply >= directionScript.totalRoomDemand)
			{
				directionScript.availableRoomSupply += requiredSupply;
				directionScript.hasReceivedSourcePower = true;
			}
		}
	}

	/// If x direction exists:
	/// If this room's compass' target is that direction:
	/// If that room has not received power from the source;
	/// Call the power transfer on that room.
	public void callPowerTransfer ()
	{
		if (north)
		{
			northScript = north.GetComponent<RoomScript> ();
			if (locator.currentHitTarget == north)
			{				
				if (!northScript.hasReceivedSourcePower)
				{
					transferPowerSupply (north);
				}
			}
		}

		if (east)
		{
			eastScript = east.GetComponent<RoomScript> ();
			if (locator.currentHitTarget == east)
			{
				if (!eastScript.hasReceivedSourcePower)
				{
					transferPowerSupply (east);
				}
			}
		}

		if (south)
		{
			southScript = south.GetComponent<RoomScript> ();
			if (locator.currentHitTarget == south)
			{
				if (!southScript.hasReceivedSourcePower)
				{
					transferPowerSupply (south);
				}
			}
		}

		if (west)
		{
			westScript = west.GetComponent<RoomScript> ();
			if (locator.currentHitTarget == west)
			{
				if (!westScript.hasReceivedSourcePower)
				{
					transferPowerSupply (west);
				}
			}
		}
	}

	/// If the room the compass here is targeting is on:
	/// Turn it off and reverse the power transfer.
	public void redirectPowerTransfer (GameObject victim)
	{
		RoomScript victimScript = victim.GetComponent<RoomScript> ();
		GameObject victimBox = victimScript.roomFuseBox;
		FuseBox victimBoxScript = victimBox.GetComponent<FuseBox> ();

		if (victimScript.isPowered)
		{
			victimBoxScript.changeState (victimBox);
		}

		int supplyToTake = victimScript.availableRoomSupply;

		victimScript.availableRoomSupply -= supplyToTake;
		victimScript.hasReceivedSourcePower = false;
	}

	/// Reverses the power transfer.
	public void reversePowerTransfer (GameObject returner) 
	{
		RoomScript returnerScript = returner.GetComponent<RoomScript> ();
		int supplyToGive = returnerScript.availableRoomSupply;
		gameMngr.availableLevelSupply += supplyToGive;
		returnerScript.availableRoomSupply -= supplyToGive;
		returnerScript.hasReceivedSourcePower = false;
	}


}


