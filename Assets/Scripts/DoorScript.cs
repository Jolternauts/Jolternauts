using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorScript : MonoBehaviour 
{
	public GameObject doorSideA;
	public GameObject doorSideB;

	RoomScript roomA;
	RoomScript roomB;

	public GameObject doorUpper;
	public GameObject doorLower;

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


	void Start ()
	{
		roomA = doorSideA.GetComponent<RoomScript> ();
		roomB = doorSideB.GetComponent<RoomScript> ();

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
	}
		
	/// <summary>
	/// Wait function.
	/// </summary>
	IEnumerator Stall()
	{
		yield return new WaitForSeconds (5);
	}

	/// <summary>
	/// Changes the active state of the door.
	/// </summary>
	public void changeActiveState()
	{
		if (isActive)
		{
			isActive = false;
			//trigger visuals/sounds
			//trigger powersupply updates
		} 
		else  
		{
			isActive = true;
			//trigger visuals/sounds
			//trigger powersupply updates
		}
	}

	/// <summary>
	/// Opens the door smoothly.
	/// </summary>
    void openDoor()
    {
		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, new Vector3 (0,0,.24f ), .01f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, new Vector3 (0, 0, 0), .01f);
    }

	/// <summary>
	/// Closes the door smoothly.
	/// </summary>
	void closeDoor()
	{
		doorUpper.transform.localPosition = Vector3.Lerp (doorUpper.transform.localPosition, positionUpperStart, .01f);
		doorLower.transform.localPosition = Vector3.Lerp (doorLower.transform.localPosition, positionLowerStart, .01f);
	}

	/// <summary>
	/// Detects Player entering door collider.
	/// </summary>
	/// <param name="detector">Detector.</param>
    void OnTriggerEnter(Collider detector)
    {		
		if (detector.transform.tag == "Player") 
		{
			//If the player is in the room in either of the door side variables.
			//Open bool is true and the light box for that side turns green. 
			if (roomA.playerIsHere && roomA.roomFuseBox.GetComponent<ObjectsList> ().isActive) 
			{
				Open = true;
				lightSideA.GetComponent<Renderer> ().material.color = Color.green;
			} 
			else if (roomB.playerIsHere && roomB.roomFuseBox.GetComponent<ObjectsList> ().isActive) 
			{
				Open = true;
				lightSideB.GetComponent<Renderer> ().material.color = Color.green;
			}
		}	
    }

	/// <summary>
	/// Detects Player exiting door collider
	/// </summary>
	/// <param name="detector">Detector.</param>
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

/*	public void transferPowerSupplyToRoomA()
	{
		if (!roomA.isPowered) 
		{
			int spareSupply = roomB.totalPowerSupply - roomB.totalPowerDemand;
			roomB.totalPowerSupply -= spareSupply;
			roomA.totalPowerSupply += spareSupply;
		} 


		Debug.Log ("Power transferred to RoomA - " + roomA.name);
	}

	public void transferPowerSupplyToRoomB()
	{
		if (!roomB.isPowered) 
		{
			int spareSupply = roomA.totalPowerSupply - roomA.totalPowerDemand;
			roomA.totalPowerSupply -= spareSupply;
			roomB.totalPowerSupply += spareSupply;
		}


		Debug.Log ("Power transferred to RoomB - " + roomB.name);
	}
*/	
}




