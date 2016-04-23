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
	public GameObject compass;
	public GameObject here;

	public List<GameObject> roomItems = new List<GameObject>();
	public List<GameObject> doors = new List<GameObject>();
	public List<GameObject> neighbours = new List<GameObject>();

	public bool isPowered = false;
	public bool playerIsHere = false;
	public bool receivedSourcePower = false;
	bool runOnce = false;
		
	GameManager gameMngr;
	AngusMovement player;
	PowerGen supplyGen;
	RoomScript supplyRoom;
	public RoomScript northScript;
	public RoomScript eastScript;
	public RoomScript southScript;
	public RoomScript westScript;

	public int totalRoomSupply;
	public int availableRoomSupply;

	public int totalRoomDemand;
	public int currentRoomDemand;


	//These are here for shortcutted reference to initiate functions or set values on Start-up.
	void Start()
	{	
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		this.GetComponent<BoxCollider> ().isTrigger = true;
		tallyTotalRoomPower ();
		directionSetup ();
		here = this.gameObject;
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
    }

	// Checks what direction a neighbouring room is in and accesses their script.
	public void directionSetup ()
	{
		if (north) 
		{
			northScript = north.GetComponent<RoomScript> ();
		}
		else if (east)
		{
			eastScript = east.GetComponent<RoomScript> ();
		}
		else if (south)
		{
			southScript = south.GetComponent<RoomScript> ();
		}
		else if (west)
		{
			westScript = west.GetComponent<RoomScript> ();
		}
	}

	/// Detects player entering room & updates power UI.
	void OnTriggerEnter(Collider detector)
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
	void OnTriggerStay(Collider detector)
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
	void OnTriggerExit(Collider detector)
	{
		if (detector.transform.tag == "Player")
		{
            runOnce = false;
			playerIsHere = false;
       }
	}
		
	/// Check active state of room.
	/// If the room is inactive oxygen & health are depleted.
	public void roomStateCheck()
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
	public void tallyTotalRoomPower()
	{
		FuseBox box = roomFuseBox.GetComponent<FuseBox> ();

		for (int x = 0; x < box.roomObjects.Count; x++) 
		{
			ObjectClass machine = box.roomObjects [x].GetComponent<ObjectClass> ();
			totalRoomSupply += machine.powerSupply;
			totalRoomDemand += machine.powerDemand;
		}
	}

	/// Transfers the power supply in the designated direction.
	/// Deducts that room's demand from the room with supply.
	public void transferPowerSupply(GameObject direction)
	{
		#pragma warning disable
		RoomScript directionScript = direction.GetComponent<RoomScript> ();
		int requiredSupply = directionScript.totalRoomDemand;;
		directionScript.availableRoomSupply += requiredSupply;

		for (int x = 0; x < gameMngr.suppliers.Count; x++) 
		{
			supplyRoom = gameMngr.suppliers[x].GetComponentInParent<RoomScript>();
		}
		supplyRoom.availableRoomSupply -= requiredSupply;
		gameMngr.availableLevelSupply -= requiredSupply;
		directionScript.receivedSourcePower = true;
		#pragma warning restore
	}

	public void takeBackPowerSupply ()
	{
		#pragma warning disable
//		RoomScript directionScript = direction.GetComponent<RoomScript> ();
//		directionScript.availableRoomSupply -= requiredSupply;
		int sharedSupply;

		for (int x = 0; x < gameMngr.roomList.Count; x++) 
		{
			RoomScript listedRoom = gameMngr.roomList [x].GetComponent<RoomScript> (); 
			if (listedRoom.receivedSourcePower)
			{
				for (int y = 0; y < listedRoom.roomItems.Count; y++) 
				{
					if (roomItems[y].tag == "Generator")
					{
						supplyRoom = listedRoom.roomItems[x].GetComponentInParent<RoomScript>();
					}					
				}
			}
//			break;
		}
//		supplyRoom.availableRoomSupply += requiredSupply;
//		gameMngr.availableLevelSupply += requiredSupply;
		#pragma warning restore
	}
}


