using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour 
{	
//	public int roomPowerSupply,roomPowerDemand;
	
	public GameObject floorGravity;
	public GameObject ceilingGravity;

	public GameObject roomFuseBox;

	public List<GameObject> roomItems = new List<GameObject>();
	public List<GameObject> doors = new List<GameObject>();

	public bool isPowered = false;
	public bool playerIsHere = false;

	bool runOnce = false;
		
	GameManager gameMngr;
	AngusMovement player;

	public int totalRoomSupply;
	public int availableRoomSupply;

	public int totalRoomDemand;
	public int currentRoomDemand;


	//These are here for shortcutted reference to initiate functions or set values on Start-up.
	void Start()
	{	
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		this.GetComponent<BoxCollider> ().isTrigger = true;
		tallyTotalRoomPower ();
    }

	void Update ()
	{
		if (availableRoomSupply < 0 || currentRoomDemand < 0) 
		{
			availableRoomSupply = 0;
			currentRoomDemand = 0;
		}
    }

	/// <summary>
	/// Detects player entering room & updates power UI.
	/// </summary>
	/// <param name="detector">Detector.</param>
	void OnTriggerEnter(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			if (!runOnce) 
			{
				runOnce = true;
				playerIsHere = true;
				gameMngr.updateRoomUI(totalRoomSupply, totalRoomDemand, availableRoomSupply, currentRoomDemand);
			}
		}
	}

	/// <summary>
	/// Detects player still in room.
	/// Runs roomStateCheck function.
	/// If Fusebox is Active, Room is Active.
	/// </summary>
	/// <param name="detector">Detector.</param>
	void OnTriggerStay(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			playerIsHere = true;
			roomStateCheck ();
			gameMngr.updateRoomUI(totalRoomSupply, totalRoomDemand, availableRoomSupply, currentRoomDemand);

			if (this.roomFuseBox.GetComponent<ObjectClass>().stateActive())
			{
				this.isPowered = true;
			}
		}
	}

	/// <summary>
	/// Detects player leaving room & updates power UI.
	/// </summary>
	/// <param name="detector">Detector.</param>
	void OnTriggerExit(Collider detector)
	{
		if (detector.transform.tag == "Player")
		{
            runOnce = false;
			playerIsHere = false;
        }
	}
		
	/// <summary>
	/// Check active state of room.
	/// If the room is active updates are made.
	/// If the room is inactive oxygen & health are depleted.
	/// </summary>
	public void roomStateCheck()
	{
		if (isPowered) 
		{

		} 
		else 
		{
			player.currentOxygen -= player.oxygenDown * Time.deltaTime;

			if (player.currentOxygen <= 0f) 
			{
				player.currentHealth -= player.healthDown * Time.deltaTime;
			}
		}
	}

	/// <summary>
	/// Tallies the total room power.
	/// It is summed up by the values of every device in the room.
	/// </summary>
	public void tallyTotalRoomPower()
	{
		FuseBox box = roomFuseBox.GetComponent<FuseBox> ();

		foreach (GameObject machine in box.roomObjects) 
		{
			totalRoomSupply += machine.GetComponent<ObjectClass> ().powerSupply;
			totalRoomDemand += machine.GetComponent<ObjectClass> ().powerDemand;
		}
	}

}


