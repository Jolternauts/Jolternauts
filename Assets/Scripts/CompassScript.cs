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

	GameObject currentReceiver;
	GameObject newReceiver;
	RoomScript targetRoom;


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
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
	}

	// Get the name of what the ray hits.
	void sayMyName (RaycastHit hit)
	{
		Debug.Log (hit.transform.name);
	}

	// Wait function.
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
		}
		Debug.Log (currentHitTarget.name);
	}
		
	/// Turns the dial left.
	/// Renew the raycast.
	/// Debug what it hits.
	public void turnDialLeft ()
	{
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

	public void callPowerRedirection ()
	{
		
	}
}
