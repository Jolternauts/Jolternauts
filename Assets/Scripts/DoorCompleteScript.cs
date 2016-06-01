using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorCompleteScript : MonoBehaviour 
{
	public RoomScript roomA;
	public RoomScript roomB;
	CompassScript compassA;
	CompassScript compassB;

	public GameObject doorSideA;
	public GameObject doorSideB;

	public GameObject lightSideA;
	public GameObject lightSideB;

	public GameObject doorUpper;
	public GameObject doorLower;
	public GameObject doorMagnet;

	Vector3 positionUpperStart;
	Vector3 positionLowerStart;
	Vector3 positionDoorMagStart;

	public int powerDemand = 5;

	public bool Open = false;
	public bool isActive = false;
	public bool isDirectionalReceiver = false;
	public bool manualOverrideAEnabled = false;
	public bool manualOverrideBEnabled = false;
	public bool doorMagReady = false;
	bool inDoorway;
	bool canCheck;

	public List<GameObject> aMagnets = new List<GameObject>();
	public List<GameObject> bMagnets = new List<GameObject>();
	public List<GameObject> chargedList = new List<GameObject>();

	public List<MagnetScript> magListA = new List<MagnetScript>();
	public List<MagnetScript> magListB = new List<MagnetScript>();

	Renderer lightARend;
	Renderer lightBRend;

	GameManager gameMngr;
	AngusMovement player;
	RoomScript hitRoom;


	void Start ()
	{
		StartCoroutine (Wait ());
		this.GetComponent<BoxCollider> ().isTrigger = true;
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();

		//Setting the local position of the door halves and door magnet.
		positionUpperStart = doorUpper.transform.localPosition;
		positionLowerStart = doorLower.transform.localPosition;
		positionDoorMagStart = doorMagnet.transform.localPosition;

		for (int x= 0; x < 8; x++)
		{
			magListA.Add (aMagnets [x].GetComponent<MagnetScript> ());
			magListB.Add (bMagnets [x].GetComponent<MagnetScript> ());
		}

		if (enabled)
		{
			gameMngr.doorList.Add (this.gameObject);
		}

	}

	void Update ()
	{	
		//If the bool Open is true, call openDoor function.
		//Else call closeDoor function.
		if (Open) 
		{
			openDoor ();
		}
		else
			closeDoor ();

		if (canCheck)
		{
			checkReceiver ();
		}
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
		if (transform.rotation.y == 0f ||
			transform.rotation.y == 180f)
		{
			if (Physics.Raycast (transform.position, Vector3.left, out hit)) 
			{
				if (hit.collider.gameObject != this.gameObject)
				{
					doorSideA = hit.collider.gameObject;
					hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
					lightSideA = hitRoom.eastLight;
				}
			}
			if (Physics.Raycast (transform.position, Vector3.right, out hit)) 
			{
				if (hit.collider.gameObject != this.gameObject)
				{
					doorSideB = hit.collider.gameObject;
					hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
					lightSideB = hitRoom.westLight;
				}
			}
		}
		else if (transform.rotation.y == 90f ||
			transform.rotation.y == 270f)
		{
			if (Physics.Raycast (transform.position, Vector3.forward, out hit)) 
			{
				if (hit.collider.gameObject != this.gameObject)
				{
					doorSideA = hit.collider.gameObject;
					hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
					lightSideA = hitRoom.southLight;
				}
			}
			if (Physics.Raycast (transform.position, Vector3.back, out hit)) 
			{
				if (hit.collider.gameObject != this.gameObject)
				{
					doorSideB = hit.collider.gameObject;
					hitRoom = hit.collider.gameObject.GetComponent<RoomScript>();
					lightSideB = hitRoom.northLight;
				}
			}
		}
	}

	/// Changes the active state of the door.
	public void changeActiveState ()
	{
		if (isActive)
		{
			isActive = false;
			lightARend.material.color = Color.red;
			lightBRend.material.color = Color.red;
			gameMngr.levelDoorPowerDown (powerDemand);
		} 
		else  
		{
			isActive = true;
			lightARend.material.color = Color.green;
			lightBRend.material.color = Color.green;
			gameMngr.levelDoorPowerUp (powerDemand);
		}
	}

	/// Opens the door smoothly.
	void openDoor ()
	{
		doorUpper.transform.localPosition = Vector3.Lerp 
			(doorUpper.transform.localPosition, new Vector3 (0, 1.1f, 0), .01f);
		doorLower.transform.localPosition = Vector3.Lerp 
			(doorLower.transform.localPosition, new Vector3 (0, 2.1f, 0), .01f);
		doorMagnet.transform.localPosition = Vector3.Lerp 
			(doorMagnet.transform.localPosition, new Vector3 (0, .00005f, 0), .00006f);
	}

	/// Closes the door smoothly.
	void closeDoor ()
	{
		doorUpper.transform.localPosition = Vector3.Lerp 
			(doorUpper.transform.localPosition, positionUpperStart, .01f);
		doorLower.transform.localPosition = Vector3.Lerp 
			(doorLower.transform.localPosition, positionLowerStart, .01f);
		doorMagnet.transform.localPosition = Vector3.Lerp 
			(doorMagnet.transform.localPosition, positionDoorMagStart, .01f);
	}

	/// Detects Player entering door collider.
	void OnTriggerStay (Collider detector)
	{		
		if (detector.transform.tag == "Player") 
		{
			// If the player is in the room in either of the door side variables.
			// Open bool is true. 
			if (roomA.playerIsHere && stateActive() || 
				roomA.playerIsHere && manualOverrideAEnabled) 
			{
				Open = true;
			} 

			if (roomB.playerIsHere && stateActive() || 
				roomB.playerIsHere && manualOverrideBEnabled) 
			{
				Open = true;
			}
			inDoorway = true;
			machineStateMeterCheck ();
		}

		if (detector.transform.tag == "Light") 
		{
			if (detector.transform.parent == doorSideA)
			{
				lightSideA = detector.gameObject;
			}
			if (detector.transform.parent == doorSideB)
			{
				lightSideB = detector.gameObject;
			}
		}
	}

	/// Detects Player exiting door collider
	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player")
		{
			// Opposite of previous function.
			// Open bool is false.
			Open = false;
			inDoorway = false;
			machineStateMeterCheck ();
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


	public bool areAllTrue (List<bool> truthListIn)
	{
		bool returnValue = true;

		foreach (bool charge in chargedList) 
		{
			if (charge == false) 
			{
				returnValue = false;
			}
		}
		return returnValue;
	}		

}
