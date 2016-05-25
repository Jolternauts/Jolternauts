using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorScript : MonoBehaviour 
{
	public List<GameObject> connectedRooms = new List<GameObject> ();
	public GameObject doorSideA;
	public GameObject doorSideB;

	CompassScript compassA;
	CompassScript compassB;
	RoomScript roomA;
	RoomScript roomB;
<<<<<<< HEAD
=======
<<<<<<< HEAD
=======
//	GameManager gameMngr;

//	public GameObject doorUpper;
//	public GameObject doorLower;
>>>>>>> origin/master
>>>>>>> origin/master

	public GameObject lightSideA;
	public GameObject lightSideB;

<<<<<<< HEAD
	public int powerDemand = 5;
=======
	public int powerDemand = 1;
>>>>>>> origin/master

	public bool isActive = false;
	public bool isDirectionalReceiver = false;
	bool keyPressed;
	bool inDoorway;

	AngusMovement player;
	GameManager gameMngr;

	Renderer lightARend;
	Renderer lightBRend;


	void Start ()
	{
		roomA = doorSideA.GetComponent<RoomScript> ();
		roomB = doorSideB.GetComponent<RoomScript> ();
		compassA = roomA.compass.GetComponent<CompassScript> ();
		compassB = roomB.compass.GetComponent<CompassScript> ();
<<<<<<< HEAD
		this.GetComponent<BoxCollider> ().isTrigger = true;
		lightARend = lightSideA.GetComponent<Renderer> ();
		lightBRend = lightSideB.GetComponent<Renderer> ();
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		gameMngr = GameManager.instance;
=======
<<<<<<< HEAD
=======
//		gameMngr = GameManager.instance;
>>>>>>> origin/master
		this.GetComponent<BoxCollider> ().isTrigger = true;
>>>>>>> origin/master
	}

	void Update ()
	{	
<<<<<<< HEAD
		checkReceiver ();
=======

>>>>>>> origin/master
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

<<<<<<< HEAD
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

=======
>>>>>>> origin/master
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



