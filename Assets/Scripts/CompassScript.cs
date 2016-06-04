using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompassScript : MonoBehaviour 
{
	public float range;

	public GameObject currentHitTarget;
	public GameObject arrow;
	public List<GameObject> arrows = new List<GameObject>();

	RoomScript targetRoom;
	RoomScript room;
	GameManager gameMngr;


	void Start () 
	{
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		this.GetComponent<BoxCollider> ().isTrigger = true;
		gameMngr = GameManager.instance;
		identifyTarget ();
	}

	void Update () 
	{
		//		identifyTarget ();
	}

	/// Cast a ray forward from the compass' arrow, 
	/// If it hits, what it hits is current hit target.
	public void identifyTarget ()
	{
		//		Vector3 forward = arrow.transform.TransformDirection (Vector3.forward) * range; 
		RaycastHit hit;
		if (Physics.Raycast (arrow.transform.position, arrow.transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
	}

	// Wait function.
	public IEnumerator Wait (float seconds)
	{
		yield return new WaitForSeconds (seconds);

		if (targetRoom && currentHitTarget != room.here)
		{
			room.callPowerTransfer ();
			flipDoorSwitch (currentHitTarget);
		}
		Debug.Log (currentHitTarget.name);
	}

	/// Turns the dial left.
	/// Undo power transfer for original target.
	/// Renew the raycast.
	/// Transfer to new target.
	/// Debug what it hits.
	public void turnDialLeft ()
	{
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
		if (room.isPowered) 
		{
			if (targetRoom)
			{				
				if (currentHitTarget != room.here && 
					targetRoom.chainPos >= room.chainPos)
				{
					flipDoorSwitch (currentHitTarget);
				}
				if (currentHitTarget != room.here && 
					targetRoom.chainPos >= room.chainPos && 
					!targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
					targetRoom.hasReceivedSourcePower = false;
				}
			}
			Debug.Log (currentHitTarget.name);
		}
		transform.Rotate (0, -90, 0, Space.Self);
		identifyTarget ();
		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}

	// Same as above but turn right.
	public void turnDialRight ()
	{
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
		if (room.isPowered) 
		{
			if (targetRoom)
			{
				if (currentHitTarget != room.here && 
					targetRoom.chainPos >= room.chainPos)
				{
					flipDoorSwitch (currentHitTarget);
				}
				if (currentHitTarget != room.here && 
					targetRoom.chainPos >= room.chainPos && 
					!targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
					targetRoom.hasReceivedSourcePower = false;
				}
			}
			Debug.Log (currentHitTarget.name);
		}
		transform.Rotate (0, 90, 0, Space.Self);
		identifyTarget ();
		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}

	/// Determines direction of the target.
	/// Accesses that doorScript.
	/// Changes active state of the door.
	/// If changing to off, remove it from active loop list.
	public void flipDoorSwitch (GameObject direction)
	{
		Debug.Log ("FlipSwitch called on " + direction.name);
		if (direction == room.north)
		{
			if (room.northDoorScript)
			{
				room.northDoorScript.changeActiveState (room.northDoor);
				if (!room.northDoorScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.northDoor);
				}
			}
			else if (room.northDoorCompScript)
			{
				room.northDoorCompScript.changeActiveState (room.northDoor);
				if (!room.northDoorCompScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.northDoor);
				}
			}
		}
		if (direction == room.east)
		{
			if (room.eastDoorScript)
			{
				room.eastDoorScript.changeActiveState (room.eastDoor);
				if (!room.eastDoorScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.eastDoor);
				}
			}
			else if (room.eastDoorCompScript)
			{
				room.eastDoorCompScript.changeActiveState (room.eastDoor);
				if (!room.eastDoorCompScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.eastDoor);
				}
			}
		}
		if (direction == room.south)
		{
			if (room.southDoorScript)
			{
				room.southDoorScript.changeActiveState (room.southDoor);
				if (!room.southDoorScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.southDoor);
				}
			}
			else if (room.southDoorCompScript)
			{
				room.southDoorCompScript.changeActiveState (room.southDoor);
				if (!room.southDoorCompScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.southDoor);
				}
			}
		}
		if (direction == room.west)
		{
			if (room.westDoor.GetComponent<DoorScript> ())
			{
				room.westDoorScript = room.westDoor.GetComponent<DoorScript> ();
			}
			else if (room.westDoor.GetComponent<DoorCompleteScript> ())
			{
				room.westDoorCompScript = room.westDoor.GetComponent<DoorCompleteScript> ();
			}

			if (room.westDoorScript)
			{
				room.westDoorScript.changeActiveState (room.westDoor);
				if (!room.westDoorScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.westDoor);
				}
			}
			else if (room.westDoorCompScript)
			{
				room.westDoorCompScript.changeActiveState (room.westDoor);
				if (!room.westDoorCompScript.stateActive ())
				{
					gameMngr.objectsToCut.Remove (room.westDoor);
				}
			}
		}
	}

}
