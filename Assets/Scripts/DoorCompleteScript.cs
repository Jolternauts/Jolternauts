using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorCompleteScript : MonoBehaviour 
{
	public List<GameObject> connectedRooms = new List<GameObject> ();
	public GameObject doorSideA;
	public GameObject doorSideB;

<<<<<<< HEAD
	CompassScript compassA;
	CompassScript compassB;
=======
	GameObject connectorsDoor;
	CompassScript compassA;
	CompassScript compassB;
	RoomScript connector;
>>>>>>> origin/master
	public RoomScript roomA;
	public RoomScript roomB;

	public GameObject lightSideA;
	public GameObject lightSideB;

	public GameObject doorUpper;
	public GameObject doorLower;
<<<<<<< HEAD
	public GameObject doorMagnet;

	Vector3 positionUpperStart;
	Vector3 positionLowerStart;
	Vector3 positionDoorMagStart;

	public int powerDemand = 1;

	public bool Open = false;
	public bool isActive = false;
	public bool isDirectionalReceiver = false;
	public bool manualOverrideAEnabled = false;
	public bool manualOverrideBEnabled = false;
	public bool doorMagReady = false;

	public List<GameObject> aMagnets = new List<GameObject>();
	public List<GameObject> bMagnets = new List<GameObject>();
	public List<GameObject> chargedList = new List<GameObject>();

	public List<MagnetScript> magListA = new List<MagnetScript>();
	public List<MagnetScript> magListB = new List<MagnetScript>();
=======

	Vector3 positionUpperStart;
	Vector3 positionLowerStart;

	public int powerDemand = 1;

	float startTime;
	float moveDistance;

	public bool Open = false;
	public bool isActive = false;
	public bool isDirectionalReceiver = false;
	public bool manualOverrideEnabled = false;

	AngusMovement player;

	public bool doorOn;
	public List<GameObject> aMagnets = new List<GameObject>();
	public List<GameObject> bMagnets = new List<GameObject>();
	public List<bool> truthList = new List<bool>();

>>>>>>> origin/master

	void Start ()
	{
		roomA = doorSideA.GetComponent<RoomScript> ();
		roomB = doorSideB.GetComponent<RoomScript> ();
		compassA = roomA.compass.GetComponent<CompassScript> ();
		compassB = roomB.compass.GetComponent<CompassScript> ();
<<<<<<< HEAD
		this.GetComponent<BoxCollider> ().isTrigger = true;

		//Setting the local position of the door halves and door magnet.
		positionUpperStart = doorUpper.transform.localPosition;
		positionLowerStart = doorLower.transform.localPosition;
		positionDoorMagStart = doorMagnet.transform.localPosition;

		for (int x= 0; x < 8; x++)
		{
			magListA.Add(aMagnets [x].GetComponent<MagnetScript> ());
			magListB.Add(bMagnets [x].GetComponent<MagnetScript> ());
		}
=======
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		this.GetComponent<BoxCollider> ().isTrigger = true;

		//Setting the local position of the door halves.
		positionUpperStart = doorUpper.transform.localPosition;
		positionLowerStart = doorLower.transform.localPosition;

>>>>>>> origin/master
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

		checkReceiver ();

	}

	/// Changes the active state of the door.
	public void changeActiveState()
	{
		if (isActive)
		{
			isActive = false;
		} 
		else  
		{
			isActive = true;
		}
	}

	/// Opens the door smoothly.
	void openDoor ()
	{
<<<<<<< HEAD
		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, new Vector3 (0, 1.1f, 0), .01f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 2.1f, 0), .01f);
		doorMagnet.transform.localPosition = Vector3.Lerp (doorMagnet.transform.localPosition, new Vector3 (0, .00005f, 0), .00006f);
=======
		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, new Vector3 (0, 1f, 0), .01f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 2f, 0), .01f);
>>>>>>> origin/master
	}

	/// Closes the door smoothly.
	void closeDoor ()
	{
		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, positionUpperStart, .01f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, positionLowerStart, .01f);
<<<<<<< HEAD
		doorMagnet.transform.localPosition = Vector3.Lerp (doorMagnet.transform.localPosition, positionDoorMagStart, .01f);
	}
		
=======
	}

	public void bottomDoorUp ()
	{
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 1.3f, 0), .001f);
	}
		
	public void bothDoorsUp ()
	{
		doorUpper.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 1f, 0), .0005f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 6f, 0), .0008f);
	}

>>>>>>> origin/master
	/// Detects Player entering door collider.
	void OnTriggerStay (Collider detector)
	{		
		if (detector.transform.tag == "Player") 
		{
			//If the player is in the room in either of the door side variables.
			//Open bool is true and the light box for that side turns green. 
			if (roomA.playerIsHere && roomA.roomFuseBox.GetComponent<FuseBox> ().stateActive() || 
<<<<<<< HEAD
				roomA.playerIsHere && manualOverrideAEnabled) 
			{
				Open = true;
				lightSideA.GetComponent<Renderer> ().material.color = Color.green;

			} 

			if (roomB.playerIsHere && roomB.roomFuseBox.GetComponent<FuseBox> ().stateActive() || 
				roomB.playerIsHere && manualOverrideBEnabled) 
=======
				roomA.playerIsHere && manualOverrideEnabled) 
			{
				Open = true;
				lightSideA.GetComponent<Renderer> ().material.color = Color.green;
			} 
			else if (roomB.playerIsHere && roomB.roomFuseBox.GetComponent<FuseBox> ().stateActive() || 
					 roomB.playerIsHere && manualOverrideEnabled) 
>>>>>>> origin/master
			{
				Open = true;
				lightSideB.GetComponent<Renderer> ().material.color = Color.green;
			}
<<<<<<< HEAD
=======

>>>>>>> origin/master
		}	
	}

	/// Detects Player exiting door collider
	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player")
		{
			//Opposite of previous function.
			//Open bool is false.
			if (roomA.playerIsHere) 
			{
				Open = false;
			} 
			else if (roomB.playerIsHere) 
			{
				Open = false;
			}
		}
	}

	/// Checks for a receiver.
	/// If x side's compass' target is the opposite room:
<<<<<<< HEAD
=======
	/// Go through Chain Links.
	/// If x side is a chain link, the door is a receiver.
	/// If it isn't the target, the door isn't a receiver.
>>>>>>> origin/master
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


<<<<<<< HEAD
=======

>>>>>>> origin/master
	public bool areAllTrue (List<bool> truthListIn)
	{
		bool returnValue = true;

<<<<<<< HEAD
		foreach (bool charge in chargedList) 
		{
			if (charge == false) 
=======
		foreach (bool truth in truthList) 
		{
			if (truth == false) 
>>>>>>> origin/master
			{
				returnValue = false;
			}
		}
		return returnValue;
	}		
		
}