using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorCompleteScript : MonoBehaviour 
{
	public List<GameObject> connectedRooms = new List<GameObject> ();
	public GameObject doorSideA;
	public GameObject doorSideB;

	GameObject connectorsDoor;
	CompassScript compassA;
	CompassScript compassB;
	RoomScript connector;
	public RoomScript roomA;
	public RoomScript roomB;

	public GameObject lightSideA;
	public GameObject lightSideB;

	public GameObject doorUpper;
	public GameObject doorLower;

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


	void Start ()
	{
		roomA = doorSideA.GetComponent<RoomScript> ();
		roomB = doorSideB.GetComponent<RoomScript> ();
		compassA = roomA.compass.GetComponent<CompassScript> ();
		compassB = roomB.compass.GetComponent<CompassScript> ();
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		this.GetComponent<BoxCollider> ().isTrigger = true;

		//Setting the local position of the door halves.
		positionUpperStart = doorUpper.transform.localPosition;
		positionLowerStart = doorLower.transform.localPosition;

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
		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, new Vector3 (0, 1f, 0), .01f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 2f, 0), .01f);
	}

	/// Closes the door smoothly.
	void closeDoor ()
	{
		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, positionUpperStart, .01f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, positionLowerStart, .01f);
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

	/// Detects Player entering door collider.
	void OnTriggerStay (Collider detector)
	{		
		if (detector.transform.tag == "Player") 
		{
			//If the player is in the room in either of the door side variables.
			//Open bool is true and the light box for that side turns green. 
			if (roomA.playerIsHere && roomA.roomFuseBox.GetComponent<FuseBox> ().stateActive() || 
				roomA.playerIsHere && manualOverrideEnabled) 
			{
				Open = true;
				lightSideA.GetComponent<Renderer> ().material.color = Color.green;
			} 
			else if (roomB.playerIsHere && roomB.roomFuseBox.GetComponent<FuseBox> ().stateActive() || 
					 roomB.playerIsHere && manualOverrideEnabled) 
			{
				Open = true;
				lightSideB.GetComponent<Renderer> ().material.color = Color.green;
			}

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
	/// Go through Chain Links.
	/// If x side is a chain link, the door is a receiver.
	/// If it isn't the target, the door isn't a receiver.
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

		foreach (bool truth in truthList) 
		{
			if (truth == false) 
			{
				returnValue = false;
			}
		}
		return returnValue;
	}		
		
}