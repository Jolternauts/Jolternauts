using UnityEngine;
using System.Collections;

public class CompassScript : MonoBehaviour 
{
	public float range;

	bool keyPressed;
	bool hittingTarget;

	int caseSwitch = 1;

	RoomScript room;
	GameObject currentHitTarget;

	void Start () 
	{
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		this.GetComponent<BoxCollider> ().isTrigger = true;
	}
	
	void Update () 
	{
		Debug.DrawRay(transform.position, transform.position + (transform.forward * range));  

		RaycastHit hit;
		currentHitTarget = hit.collider.gameObject;
		GameObject targetDoor;
		if (room.isPowered)
		{
			RoomScript targetRoom = currentHitTarget.GetComponent<RoomScript> ();
			for (int x = 0; x < targetRoom.doors.Count; x++) 
			{
				targetDoor = targetRoom.doors [x];
				DoorScript targetDoorScript = targetDoor.GetComponent<DoorScript> ();

				if (Physics.Raycast (transform.position, transform.forward, out hit, range)) 
				{

				}
			}

		}
	}

	void sayMyName (RaycastHit hit)
	{
		Debug.Log (hit.transform.name);
	}

	public void checkFlowReceiver (RaycastHit hit)
	{
		currentHitTarget = hit.collider.gameObject;

		if (Physics.Raycast (transform.position, transform.forward, out hit, range)) 
		{
			hittingTarget = true;
		} 
		else
			hittingTarget = false;

		if (hittingTarget)
		{
//			if (currentHitTarget = room.nei) 
			{
				
			}
		}

/*		RoomScript targetRoom = currentHitTarget.GetComponent<RoomScript>();

		for (int x = 0; x < room.doors.Count; x++) 
		{
			DoorScript targetRoomDoor = targetRoom.doors [x].GetComponent<DoorScript> ();
			if (currentHitTarget = room.north && northScript.doors.Contains(room.doors[x])) 
			{
				if (hittingTarget)
				{
					
				}
			}
		}
*/

	}

	void OnTriggerStay(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			if (Input.GetKeyDown (KeyCode.Q) && !statePressed ()) 
			{
				turnThatDial ();
				statePressed (true);
			}

			if (Input.GetKeyUp (KeyCode.Q)) 
			{
				statePressed (false);
			}
		}
	}

	public void turnThatDial ()
	{
		transform.Rotate (0, 90, 0, Space.Self);
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
