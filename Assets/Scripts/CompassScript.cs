using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompassScript : MonoBehaviour 
{
	public float range;

	bool keyPressed;
	bool hittingTarget;

	RoomScript room;
	RoomScript neighbourScript; 
	DoorScript targetDoorScript;
	DoorScript thisRoomDoorScript;
	GameObject thisRoomDoor;
	GameObject targetDoor;
	GameObject neighbour;
	public GameObject currentHitTarget;
	public GameObject arrow;
	public List<GameObject> arrows = new List<GameObject>();

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
		Debug.DrawRay(arrow.transform.position, forward, Color.green);  

		if (Physics.Raycast (arrow.transform.position, arrow.transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
	}

	// Get the name of what the ray hits.
	void sayMyName (RaycastHit hit)
	{
		Debug.Log (hit.transform.name);
	}

	// Wait function.
	public IEnumerator WaitAMinute()
	{
		yield return new WaitForSeconds (3);
		Debug.Log (currentHitTarget.name);
	}

	/// Checks the flow receiver (the complicated first attempt way).
	/// The crteria it checks are as follows:

	/// Is the room the compass is in powered.
	/// Does the room have which ever direction.
	/// Is the direction the target.
	/// Go through this room's door list.
	/// Go through that room's door list.
	/// If both lists have the same door, it is a receiver.

	/// If the direction is not the target, it's not a receiver.
	/// To be sure, if the room isn't pwoered, no doors here are receivers.
	public void checkFlowReceiver ()
	{
//		#pragma warning disable

		if (room.isPowered) 
		{
			if (room.north) 
			{
				if (currentHitTarget == room.north) 
				{
					for (int x = 0; x < room.doors.Count; x++) 
					{
						thisRoomDoor = room.doors [x];
						thisRoomDoorScript = thisRoomDoor.GetComponent<DoorScript> ();
					}
					for (int y = 0; y < room.northScript.doors.Count; y++) 
					{
						if (room.northScript.doors.Contains (thisRoomDoor)) 
						{
							thisRoomDoorScript.isDirectionalReceiver = true;
						}
					}
				}
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}

			if (room.east) 
			{
				if (currentHitTarget == room.east) 
				{
					for (int x = 0; x < room.doors.Count; x++) 
					{
						thisRoomDoor = room.doors [x];
						thisRoomDoorScript = thisRoomDoor.GetComponent<DoorScript> ();
					}
					for (int y = 0; y < room.eastScript.doors.Count; y++) 
					{
						if (room.eastScript.doors.Contains (thisRoomDoor)) 
						{
							thisRoomDoorScript.isDirectionalReceiver = true;
						}
					}
				}
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}

			if (room.south) 
			{
				if (currentHitTarget == room.south) 
				{
					for (int x = 0; x < room.doors.Count; x++) 
					{
						thisRoomDoor = room.doors [x];
						thisRoomDoorScript = thisRoomDoor.GetComponent<DoorScript> ();
					}
					for (int y = 0; y < room.southScript.doors.Count; y++) 
					{
						if (room.southScript.doors.Contains (thisRoomDoor)) 
						{
							thisRoomDoorScript.isDirectionalReceiver = true;
						}
					}
				}
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}

			if (room.west) 
			{
				if (currentHitTarget == room.west) 
				{
					for (int x = 0; x < room.doors.Count; x++) 
					{
						thisRoomDoor = room.doors [x];
						thisRoomDoorScript = thisRoomDoor.GetComponent<DoorScript> ();
					}
					for (int y = 0; y < room.westScript.doors.Count; y++) 
					{
						if (room.westScript.doors.Contains (thisRoomDoor)) 
						{
							thisRoomDoorScript.isDirectionalReceiver = true;
						}
					}
				} 
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}
		} 
		else
			for (int x = 0; x < room.doors.Count; x++) 
			{
				thisRoomDoor = room.doors [x];
				thisRoomDoorScript = thisRoomDoor.GetComponent<DoorScript> ();
				thisRoomDoorScript.isDirectionalReceiver = false;
			}
//		#pragma warning restore
	}

	// If player is colliding and Q is pressed, turn compass to the right. 
	void OnTriggerStay(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			if (Input.GetKeyDown (KeyCode.Q) && !statePressed ()) 
			{
				turnDialRight ();
				statePressed (true);
			}

			if (Input.GetKeyUp (KeyCode.Q)) 
			{
				statePressed (false);
			}
		}
	}

	/// Turns the dial left.
	/// Renew the raycast.
	/// Debug what it hits.
	public void turnDialLeft ()
	{
		transform.Rotate (0, -90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
		Debug.Log (currentHitTarget.name);
	}

	// Same as above but turn right.
	public void turnDialRight ()
	{
		transform.Rotate (0, 90, 0, Space.Self);
		transform.Rotate (0, -90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
		Debug.Log (currentHitTarget.name);
	}

	// Sets bool state.
	public void statePressed(bool setPress)
	{
		keyPressed = setPress;
	}

	// Returns bool state.
	public bool statePressed()
	{
		return keyPressed;
	}
}
