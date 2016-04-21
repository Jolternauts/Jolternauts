using UnityEngine;
using System.Collections;

public class CompassScript : MonoBehaviour 
{
	public float range;

	bool keyPressed;
	bool hittingTarget;

	RoomScript room;
	RoomScript northScript;
	RoomScript eastScript;
	RoomScript southScript;
	RoomScript westScript;
	RoomScript neighbourScript; 
	DoorScript targetDoorScript;
	DoorScript thisRoomDoorScript;
	GameObject thisRoomDoor;
	GameObject targetDoor;
	GameObject neighbour;
	public GameObject currentHitTarget;

	void Start () 
	{
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		this.GetComponent<BoxCollider> ().isTrigger = true;
//		StartCoroutine (WaitAMinute ());
	}
	
	void Update () 
	{
		RaycastHit hit;
		Vector3 forward = transform.TransformDirection(Vector3.forward) * range; 
		Debug.DrawRay(transform.position, forward, Color.green);  

		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}

	}

	void sayMyName (RaycastHit hit)
	{
		Debug.Log (hit.transform.name);
	}

	public IEnumerator WaitAMinute()
	{
		yield return new WaitForSeconds (3);
		Debug.Log (currentHitTarget.name);
	}


	public void checkFlowReceiver ()
	{
		#pragma warning disable
		if (room.isPowered)
		{
			for (int x = 0; x < room.neighbours.Count; x++) 
			{
				neighbour = room.neighbours [x];
				neighbourScript = neighbour.GetComponent<RoomScript> ();
				for (int y = 0; y < neighbourScript.doors.Count; y++) 
				{
					targetDoor = neighbourScript.doors [y];
					targetDoorScript = targetDoor.GetComponent<DoorScript> ();
				}
			}

			for (int x = 0; x < room.doors.Count; x++) 
			{
				thisRoomDoor = room.doors [x];
				thisRoomDoorScript = thisRoomDoor.GetComponent<DoorScript> ();
			}

			if (room.north)
			{
				if (currentHitTarget == room.north) 
				{
					northScript = room.west.GetComponent<RoomScript> ();
					if (northScript.doors.Contains (thisRoomDoor)) 
					{
						thisRoomDoorScript.isDirectionalReceiver = true;
					}
				}
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}

			if (room.east)
			{
				if (currentHitTarget == room.east) 
				{
					eastScript = room.west.GetComponent<RoomScript> ();
					if (eastScript.doors.Contains (thisRoomDoor)) 
					{
						thisRoomDoorScript.isDirectionalReceiver = true;
					}
				}
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}

			if (room.south)
			{
				if (currentHitTarget == room.south) 
				{
					southScript = room.west.GetComponent<RoomScript> ();
					if (southScript.doors.Contains (thisRoomDoor)) 
					{
						thisRoomDoorScript.isDirectionalReceiver = true;
					}
				}
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}

			if (room.west)
			{
				if (currentHitTarget == room.west) 
				{
					westScript = room.west.GetComponent<RoomScript> ();
					for (int x = 0; x < westScript.doors.Count; x++) 
					{
						targetDoor = westScript.doors [x];
						targetDoorScript = targetDoor.GetComponent<DoorScript> ();

						if (targetDoor == thisRoomDoor) 
						{
							targetDoorScript.isDirectionalReceiver = true;
						}
					}
				}
				else
					thisRoomDoorScript.isDirectionalReceiver = false;
			}
		}
		#pragma warning restore
	}


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
