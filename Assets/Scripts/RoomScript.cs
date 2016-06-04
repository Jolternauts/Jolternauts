using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour 
{		
	public GameObject floorGravity;
	public GameObject ceilingGravity;

	public GameObject roomFuseBox;
	public GameObject compass;

	//	GameObject computer;
	//	GameObject gen;

	public GameObject north;
	public GameObject east;
	public GameObject south;
	public GameObject west;
	public GameObject northDoor;
	public GameObject eastDoor;
	public GameObject southDoor;
	public GameObject westDoor;
	public GameObject northLight;
	public GameObject eastLight;
	public GameObject southLight;
	public GameObject westLight;
	public GameObject here;

	public List<GameObject> doors = new List<GameObject>();
	public List<GameObject> neighbours = new List<GameObject>();

	public bool isPowered = false;
	public bool playerIsHere = false;
	public bool hasReceivedSourcePower = false;

	bool runOnce = false;

	GameManager gameMngr;
	AngusMovement player;
	FuseBox box;
	//	PowerDrain console;
	//	PowerGen supplier;

	public RoomScript northScript;
	public RoomScript eastScript;
	public RoomScript southScript;
	public RoomScript westScript;
	public DoorScript northDoorScript;
	public DoorScript eastDoorScript;
	public DoorScript southDoorScript;
	public DoorScript westDoorScript;
	public DoorCompleteScript northDoorCompScript;
	public DoorCompleteScript eastDoorCompScript;
	public DoorCompleteScript southDoorCompScript;
	public DoorCompleteScript westDoorCompScript;
	LightScript northLightScript;
	LightScript eastLightScript;
	LightScript southLightScript;
	LightScript westLightScript;

	CompassScript locator;

	public int totalRoomSupply;
	public int availableRoomSupply;

	public int totalRoomDemand;
	public int currentRoomDemand;
	public int chainPos = 0;

	public float offset1 = 2.5f;

	RoomScript hitRoom;
	DoorScript hitDoor;
	DoorCompleteScript hitDoorComp;


	// Shortcutted reference to initiate functions or set values on Start-up.
	void Start ()
	{	
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement> ();
		roomFuseBox = GetComponentInChildren<FuseBox> ().gameObject;
		box = roomFuseBox.GetComponent<FuseBox> ();
		compass = this.GetComponentInChildren<CompassScript> ().gameObject;
		locator = compass.GetComponent<CompassScript> ();
		this.GetComponent<BoxCollider> ().isTrigger = true;
		here = this.gameObject;
		directionObjectSetup ();
		directionScriptSetup ();

		if (enabled)
		{
			gameMngr.roomList.Add (this.gameObject);
		}
		tallyTotalRoomPower ();

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
		//		drawLines ();
	}

	public void drawLines ()
	{
		if (north)
		{
			Debug.DrawRay (transform.position + Vector3.left * offset1, Vector3.forward, Color.yellow);  
		}

		if (east)
		{
			Debug.DrawRay (transform.position + Vector3.forward * offset1, Vector3.right, Color.red);  
		}

		if (south)
		{
			Debug.DrawRay (transform.position + Vector3.right * offset1, Vector3.back, Color.blue);  
		}

		if (west)
		{
			Debug.DrawRay (transform.position + Vector3.back * offset1, Vector3.left, Color.magenta);  
		}
	}

	/// Throw 2 rays in x direction.
	/// 1 to get the directional object & add it to neighbours.
	/// The other to get the directional door & add it to doors.
	public void directionObjectSetup ()
	{
		RaycastHit hit;


		if (Physics.Raycast (transform.position, Vector3.forward, out hit)) 
		{
			hitRoom = hit.collider.gameObject.GetComponent<RoomScript> ();

			if (hitRoom && hit.collider.gameObject != here)
			{
				north = hit.collider.gameObject;
				neighbours.Add (north);
			}
		}

		if (Physics.Raycast (transform.position, Vector3.right, out hit)) 
		{
			hitRoom = hit.collider.gameObject.GetComponent<RoomScript> ();

			if (hitRoom && hit.collider.gameObject != here)
			{
				east = hit.collider.gameObject;
				neighbours.Add (east);
			}
		}

		if (Physics.Raycast (transform.position, Vector3.back, out hit)) 
		{
			hitRoom = hit.collider.gameObject.GetComponent<RoomScript> ();

			if (hitRoom && hit.collider.gameObject != here)
			{
				south = hit.collider.gameObject;
				neighbours.Add (south);
			}
		}

		if (Physics.Raycast (transform.position, Vector3.left, out hit)) 
		{
			hitRoom = hit.collider.gameObject.GetComponent<RoomScript> ();

			if (hitRoom && hit.collider.gameObject != here)
			{
				west = hit.collider.gameObject;
				neighbours.Add (west);
			}
		}
	}

	/// If x direction exists:
	/// Set its script.
	/// Set its doorScript, whichever doorScript type it is.
	/// Throw a offset ray in that direction to get its light. 
	public void directionScriptSetup ()
	{
		RaycastHit hit;
		LayerMask layerMask = ~( (1 << 11) );
		LayerMask lightLayer = ~( (1 << 3) | (1 << 10) | (1 << 13) );

		if (north) 
		{
			northScript = north.GetComponent<RoomScript> ();

			if (Physics.Raycast (transform.position, Vector3.forward, out hit, locator.range, layerMask))
			{
				hitDoor = hit.collider.gameObject.GetComponent<DoorScript> ();
				hitDoorComp = hit.collider.gameObject.GetComponent<DoorCompleteScript> ();

				if (hitDoor)
				{
					northDoor = hit.collider.gameObject;
					northDoorScript = northDoor.GetComponent<DoorScript> ();
					doors.Add (northDoor);
				}
				if (hitDoorComp)
				{
					northDoor = hit.collider.gameObject;
					northDoorCompScript = northDoor.GetComponent<DoorCompleteScript> ();
					doors.Add (northDoor);
				}
			}

			if (Physics.Raycast (transform.position + Vector3.left * offset1, 
				Vector3.forward, out hit, locator.range, lightLayer.value)) 
			{
				northLight = hit.collider.transform.gameObject;
			}
		}

		if (east)
		{
			eastScript = east.GetComponent<RoomScript> ();

			if (Physics.Raycast (transform.position, Vector3.right, out hit, locator.range, layerMask))
			{
				hitDoor = hit.collider.gameObject.GetComponent<DoorScript> ();
				hitDoorComp = hit.collider.gameObject.GetComponent<DoorCompleteScript> ();

				if (hitDoor)
				{
					eastDoor = hit.collider.gameObject;
					eastDoorScript = eastDoor.GetComponent<DoorScript> ();
					doors.Add (eastDoor);
				}
				else if (hitDoorComp)
				{
					eastDoor = hit.collider.gameObject;
					eastDoorCompScript = eastDoor.GetComponent<DoorCompleteScript> ();
					doors.Add (eastDoor);
				}
			}

			if (Physics.Raycast (transform.position + Vector3.forward * offset1, 
				Vector3.right, out hit, locator.range, lightLayer.value)) 
			{
				eastLight = hit.collider.transform.gameObject;
			}
		}

		if (south)
		{
			southScript = south.GetComponent<RoomScript> ();
			if (Physics.Raycast (transform.position, Vector3.back, out hit, locator.range, layerMask))
			{
				hitDoor = hit.collider.gameObject.GetComponent<DoorScript> ();
				hitDoorComp = hit.collider.gameObject.GetComponent<DoorCompleteScript> ();

				if (hitDoor)
				{
					southDoor = hit.collider.gameObject;
					southDoorScript = southDoor.GetComponent<DoorScript> ();
					doors.Add (southDoor);
				}
				else if (hitDoorComp)
				{
					southDoor = hit.collider.gameObject;
					southDoorCompScript = southDoor.GetComponent<DoorCompleteScript> ();
					doors.Add (southDoor);
				}
			}

			if (Physics.Raycast (transform.position + Vector3.right * offset1, 
				Vector3.back, out hit, locator.range, lightLayer.value)) 
			{
				southLight = hit.collider.transform.gameObject;
			}
		}

		if (west)
		{
			westScript = west.GetComponent<RoomScript> ();
			if (Physics.Raycast (transform.position, Vector3.left, out hit, locator.range, layerMask))
			{
				hitDoor = hit.collider.gameObject.GetComponent<DoorScript> ();
				hitDoorComp = hit.collider.gameObject.GetComponent<DoorCompleteScript> ();

				if (hitDoor)
				{
					westDoor = hit.collider.gameObject;
					westDoorScript = westDoor.GetComponent<DoorScript> ();
					doors.Add (westDoor);
				}
				else if (hitDoorComp)
				{
					westDoor = hit.collider.gameObject;
					westDoorCompScript = westDoor.GetComponent<DoorCompleteScript> ();
					doors.Add (westDoor);
				}

			}

			if (Physics.Raycast (transform.position + Vector3.back * offset1, 
				Vector3.left, out hit, locator.range, lightLayer.value)) 
			{
				westLight = hit.collider.transform.gameObject;
			}
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
				gameMngr.updateRoomUI (totalRoomSupply, totalRoomDemand, 
					availableRoomSupply, currentRoomDemand);
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
			gameMngr.updateRoomUI (totalRoomSupply, totalRoomDemand, 
				availableRoomSupply, currentRoomDemand);

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

	IEnumerator Wait ()
	{
		yield return new WaitForSeconds (.5f);
		tallyTotalRoomPower ();
	}

}


