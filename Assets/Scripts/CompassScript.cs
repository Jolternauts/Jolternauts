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


	void Start () 
	{
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		this.GetComponent<BoxCollider> ().isTrigger = true;
	}
	
	void Update () 
	{
		/// Cast a ray forward from the compass' arrow, if it hits, what it hits is current hit target.
		RaycastHit hit;
		Vector3 forward = arrow.transform.TransformDirection(Vector3.forward) * range; 
		Debug.DrawRay (arrow.transform.position, forward, Color.green);  

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
				if (currentHitTarget != room.here && targetRoom.chainPos >= room.chainPos)
				{
					flipDoorSwitch (currentHitTarget);
				}
				if (currentHitTarget != room.here && targetRoom.chainPos >= room.chainPos && !targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
					targetRoom.hasReceivedSourcePower = false;
				}
			}
			Debug.Log (currentHitTarget.name);
		}
		transform.Rotate (0, -90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
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
				if (currentHitTarget != room.here && targetRoom.chainPos >= room.chainPos)
				{
					flipDoorSwitch (currentHitTarget);
				}
				if (currentHitTarget != room.here && targetRoom.chainPos >= room.chainPos && !targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
					targetRoom.hasReceivedSourcePower = false;
				}
			}
			Debug.Log (currentHitTarget.name);
		}
		transform.Rotate (0, 90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}

	public void flipDoorSwitch (GameObject direction)
	{
		if (direction == room.north)
		{
			room.northDoorScript.changeActiveState (room.northDoor);
		}
		if (direction == room.east)
		{
			room.eastDoorScript.changeActiveState (room.eastDoor);
		}
		if (direction == room.south)
		{
			room.southDoorScript.changeActiveState (room.southDoor);
		}
		if (direction == room.west)
		{
			room.westDoorScript.changeActiveState (room.westDoor);
		}
	}

}
