using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompassScript : MonoBehaviour 
{
	public float range;

	public GameObject currentHitTarget;
	public GameObject arrow;
	public List<GameObject> arrows = new List<GameObject>();

<<<<<<< HEAD
	RoomScript targetRoom;
	RoomScript room;
=======
<<<<<<< HEAD
	RoomScript targetRoom;
	RoomScript room;
=======
	GameObject currentReceiver;
	GameObject newReceiver;
	RoomScript targetRoom;
>>>>>>> origin/master
>>>>>>> origin/master


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
<<<<<<< HEAD
	}

	// Wait function.
=======
	}

	// Get the name of what the ray hits.
	void sayMyName (RaycastHit hit)
	{
		Debug.Log (hit.transform.name);
	}

	// Wait function.
<<<<<<< HEAD
>>>>>>> origin/master
	public IEnumerator Wait (float seconds)
	{
		yield return new WaitForSeconds (seconds);

		if (targetRoom && currentHitTarget != room.here)
		{
			room.callPowerTransfer ();
<<<<<<< HEAD
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
=======
=======
	public IEnumerator Wait(float seconds)
	{
		yield return new WaitForSeconds (seconds);
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

		if (targetRoom && currentHitTarget != room.here)
		{
			room.callPowerTransfer ();
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
		Debug.Log (currentHitTarget.name);
		room.transferPowerSupply (currentHitTarget);
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
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
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
<<<<<<< HEAD
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
		if (room.isPowered) 
		{
			if (targetRoom)
			{				
				if (currentHitTarget != room.here && targetRoom.chainPos > room.chainPos && targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
				}
			}
=======
		if (room.isPowered) 
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
			if (targetRoom && currentHitTarget != room.here)
			{
				room.redirectPowerTransfer (currentHitTarget);  
			}
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
			room.redirectPowerTransfer (currentHitTarget);  
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
			Debug.Log (currentHitTarget.name);
		}
			
>>>>>>> origin/master
		transform.Rotate (0, -90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
<<<<<<< HEAD
=======

>>>>>>> origin/master
		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}

	// Same as above but turn right.
	public void turnDialRight ()
	{
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> origin/master
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
		if (room.isPowered) 
		{
			if (targetRoom)
			{
<<<<<<< HEAD
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
=======
				if (currentHitTarget != room.here && targetRoom.chainPos > room.chainPos && targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
				}
			}
=======
		if (room.isPowered) 
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
			if (targetRoom && currentHitTarget != room.here)
			{
				room.redirectPowerTransfer (currentHitTarget);  
			}
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
=======
			room.redirectPowerTransfer (currentHitTarget);  
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
>>>>>>> origin/master
			Debug.Log (currentHitTarget.name);
		}

>>>>>>> origin/master
		transform.Rotate (0, 90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======

>>>>>>> origin/master
		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}
>>>>>>> origin/master

<<<<<<< HEAD
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

=======
		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}

<<<<<<< HEAD

=======
	// Returns bool state.
	public bool statePressed()
	{
		return keyPressed;
	}

	public void callPowerRedirection ()
	{
		
	}
>>>>>>> origin/master
>>>>>>> origin/master
}
