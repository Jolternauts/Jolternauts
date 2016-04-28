using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorScript : MonoBehaviour 
{
	public List<GameObject> connectedRooms = new List<GameObject> ();
	public GameObject doorSideA;
	public GameObject doorSideB;

	GameObject connectorsDoor;
	CompassScript compassA;
	CompassScript compassB;
	RoomScript connector;
	RoomScript roomA;
	RoomScript roomB;
//	GameManager gameMngr;

//	public GameObject doorUpper;
//	public GameObject doorLower;

	public GameObject lightSideA;
	public GameObject lightSideB;

	Vector3 positionUpperStart;
	Vector3 positionLowerStart;
	public Vector3 lerpTarget;

	public int powerDemand = 1;

	float startTime;
	float moveDistance;

	public bool Open = false;
	public bool isActive = false;
	public bool isDirectionalReceiver = false;


	void Start ()
	{
		roomA = doorSideA.GetComponent<RoomScript> ();
		roomB = doorSideB.GetComponent<RoomScript> ();
		compassA = roomA.compass.GetComponent<CompassScript> ();
		compassB = roomB.compass.GetComponent<CompassScript> ();
//		gameMngr = GameManager.instance;
		this.GetComponent<BoxCollider> ().isTrigger = true;

		//Setting the local position of the door halves.
//		positionUpperStart = doorUpper.transform.localPosition;
//		positionLowerStart = doorLower.transform.localPosition;
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
    void openDoor()
    {
//		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, new Vector3 (0,0,.24f ), .01f);
//		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 0, 0), .01f);
    }

	/// Closes the door smoothly.
	void closeDoor()
	{
//		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, positionUpperStart, .01f);
//		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, positionLowerStart, .01f);
	}

	/// Detects Player entering door collider.
    void OnTriggerEnter(Collider detector)
    {		
		if (detector.transform.tag == "Player") 
		{
			//If the player is in the room in either of the door side variables.
			//Open bool is true and the light box for that side turns green. 
/*			if (roomA.playerIsHere && roomA.roomFuseBox.GetComponent<ObjectsList> ().isActive) 
			{
				Open = true;
				lightSideA.GetComponent<Renderer> ().material.color = Color.green;
			} 
			else if (roomB.playerIsHere && roomB.roomFuseBox.GetComponent<ObjectsList> ().isActive) 
			{
				Open = true;
				lightSideB.GetComponent<Renderer> ().material.color = Color.green;
			}
*/
		}	
    }

	/// Detects Player exiting door collider
	void OnTriggerExit(Collider detector)
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
}



