using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorScript : MonoBehaviour 
{
	RoomScript roomA;
	RoomScript roomB;
	CompassScript compassA;
	CompassScript compassB;

	public GameObject doorSideA;
	public GameObject doorSideB;

	public GameObject lightSideA;
	public GameObject lightSideB;

	public int powerDemand = 5;

	public bool isActive = false;
	public bool isDirectionalReceiver = false;
	bool keyPressed;
	bool inDoorway;
	bool canCheck;

	AngusMovement player;
	GameManager gameMngr;
	RoomScript hitRoom;

	Renderer lightARend;
	Renderer lightBRend;

	float range = 10;


	void Start ()
	{
		transform.rotation = transform.rotation;
		StartCoroutine (Wait ());
		this.GetComponent<BoxCollider> ().isTrigger = true;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		gameMngr = GameManager.instance;

		if (enabled)
		{
			gameMngr.doorList.Add (this.gameObject);
		}
	}

	void Update ()
	{	
		if (canCheck)
		{
			checkReceiver ();
		}
		drawLines ();
	}

	public void drawLines ()
	{
		Debug.DrawRay (transform.position, Vector3.forward, Color.yellow);  
		Debug.DrawRay (transform.position, Vector3.right, Color.red);  
		Debug.DrawRay (transform.position, Vector3.back, Color.blue);  
		Debug.DrawRay (transform.position, Vector3.left, Color.magenta);  
	}

	IEnumerator Wait ()
	{
		yield return new WaitForSeconds (.03f);
		doorSideSetup ();
		roomA = doorSideA.GetComponent<RoomScript> ();
		roomB = doorSideB.GetComponent<RoomScript> ();
		compassA = roomA.compass.GetComponent<CompassScript> ();
		compassB = roomB.compass.GetComponent<CompassScript> ();
		lightARend = lightSideA.GetComponent<Renderer> ();
		lightBRend = lightSideB.GetComponent<Renderer> ();
		canCheck = true;
	}

	public void doorSideSetup ()
	{
		RaycastHit hit;

		if (Physics.Raycast (transform.position, Vector3.left, out hit, range)) 
		{	
			hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
			if (hit.collider.gameObject != this.gameObject)
			{
				
				Debug.Log (this.gameObject.name + ": Hit: " + hit.collider.gameObject.name);
				doorSideA = hit.collider.gameObject;
				lightSideA = hitRoom.eastLight;
			}
		}
			
		if (Physics.Raycast (transform.position, Vector3.right, out hit, range)) 
		{
			hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
			if (hit.collider.gameObject != this.gameObject)
			{
				Debug.Log (this.gameObject.name + ": Hit: " + hit.collider.gameObject.name);
				doorSideB = hit.collider.gameObject;
				lightSideB = hitRoom.westLight;
			}
		}

		{
			if (Physics.Raycast (transform.position, Vector3.forward, out hit, range)) 
			{
				hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
				if (hit.collider.gameObject != this.gameObject)
				{
					Debug.Log (this.gameObject.name + ": Hit: " + hit.collider.gameObject.name);
					doorSideA = hit.collider.gameObject;
					lightSideA = hitRoom.southLight;
				}
			}
			if (Physics.Raycast (transform.position, Vector3.back, out hit, range)) 
			{
				hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
				if (hit.collider.gameObject != this.gameObject)
				{
					Debug.Log (this.gameObject.name + ": Hit: " + hit.collider.gameObject.name);
					doorSideB = hit.collider.gameObject;
					lightSideB = hitRoom.northLight;
				}
			}
		}
	}

	/// Changes the active state of the door.
	public void changeActiveState (GameObject reference)
	{
		if (isActive)
		{
			stateActive (false);
			lightARend.material.color = Color.red;
			lightBRend.material.color = Color.red;
			gameMngr.levelDoorPowerDown (powerDemand);
			Debug.Log ("Turning " + this.gameObject.name + " off");
		} 
		else  
		{
			stateActive (true);
			lightARend.material.color = Color.green;
			lightBRend.material.color = Color.green;
			gameMngr.levelDoorPowerUp (powerDemand);
			gameMngr.objectsToCut.Add (this.gameObject);
			Debug.Log ("Turning " + this.gameObject.name + " on");
		}
	}

	void OnTriggerEnter (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			inDoorway = true;
			machineStateMeterCheck ();
		}
	}

	// While inside trigger.
	void OnTriggerStay (Collider detector)
	{
		// Detect player in doorway.
		if (detector.transform.tag == "Player")
		{
			// If E is pressed turn it on/off.
			if (Input.GetKeyDown (KeyCode.E) && !statePressed ()) 
			{
				if (!stateActive ()) 
				{
					changeActiveState (this.gameObject);
				}
				else
				{
					changeActiveState (this.gameObject);
					gameMngr.objectsToCut.Remove (this.gameObject);
				}
				statePressed (true);
			}

			if (Input.GetKeyUp (KeyCode.E)) 
			{
				statePressed (false);
			}
		}

		// Check what color the statemeters should be.
		machineStateMeterCheck ();
	}

	// When player leaves trigger:
	// Player is not beside.
	// Check what color the statemeters should be.
	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			inDoorway = false;
			machineStateMeterCheck ();
		}
	}

	public void machineStateMeterCheck ()
	{
		if (inDoorway) 
		{
			if (!stateActive ()) 
			{
				player.changeStateMeterColors (Color.red);
			}
			else if (stateActive ()) 
			{
				player.changeStateMeterColors (Color.green);
			}
		}

		else 
		{
			player.changeStateMeterColors (Color.white);
		}
	}

	// Sets bool state.
	public void statePressed (bool setPress)
	{
		keyPressed = setPress;
	}

	// Returns bool state.
	public bool statePressed ()
	{
		return keyPressed;
	}

	// Sets bool state.
	public void stateActive (bool setActive)
	{
		isActive = setActive;
	}

	// Returns bool state.
	public bool stateActive ()
	{
		return isActive;
	}

	/// Checks for a receiver.
	/// If x side's compass' target is the opposite room:
	public void checkReceiver ()
	{
		if (compassA.currentHitTarget == doorSideB) 
		{
			isDirectionalReceiver = true;
		}
		else
			isDirectionalReceiver = false;

		if (compassB.currentHitTarget == doorSideA) 
		{
			isDirectionalReceiver = true;
		}
		else
			isDirectionalReceiver = false;
	}
}



