using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorScript : MonoBehaviour 
{
	public List<GameObject> connectedRooms = new List<GameObject> ();
	public GameObject doorSideA;
	public GameObject doorSideB;

	GameObject me;
	GameObject connectorsDoor;
	CompassScript locator;
	DoorScript myScript;
	RoomScript connector;
	RoomScript roomA;
	RoomScript roomB;

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
		me = this.gameObject;
		myScript = me.GetComponent<DoorScript> ();
		roomA = doorSideA.GetComponent<RoomScript> ();
		roomB = doorSideB.GetComponent<RoomScript> ();
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


/*		if (doorSideA == roomA.compass.GetComponent<CompassScript> ().currentHitTarget) 
		{
			if (roomA.isPowered) 
			{
				for (int x = 0; x < roomB.doors.Count; x++) 
				{
					if (roomB.doors[x] == me)
					{
						isDirectionalReceiver = true;
					}
				}
			}
		}
		else if (doorSideB == roomB.compass.GetComponent<CompassScript> ().currentHitTarget) 
		{
			if (roomB.isPowered) 
			{
				for (int x = 0; x < roomA.doors.Count; x++) 
				{
					if (roomA.doors[x] == me)
					{
						isDirectionalReceiver = true;
					}
				}
			}
		}

		for (int x = 0; x < connectedRooms.Count; x++) 
		{
			connector = connectedRooms [x].GetComponent<RoomScript> ();
			locator = connector.compass.GetComponent<CompassScript> ();

			if (connector.isPowered) 
			{
				if (connectedRooms [x] == locator.currentHitTarget) 
				{
					for (int y = 0; y < connector.doors.Count; y++) 
					{
						connectorsDoor = connector.doors [y];
						if (connectorsDoor == me) 
						{
							myScript.isDirectionalReceiver = true;
						}
					}
				}
				else
				{
					isDirectionalReceiver = false;
				}
			}
		}
*/
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
}




