using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	public List <GameObject> roomList = new List<GameObject> ();
	public List <GameObject> doorList = new List<GameObject> ();
	public List <GameObject> suppliers = new List<GameObject> ();
	public List <GameObject> chainLinks = new List<GameObject> ();
	public List <GameObject> tier1 = new List<GameObject> (); 
	public List <GameObject> tier2 = new List<GameObject> (); 
	public List <GameObject> tier3 = new List<GameObject> ();
<<<<<<< HEAD
	public List <GameObject> objectsToCut = new List <GameObject> ();
	public List <GameObject> uploadedDevices = new List <GameObject> ();
=======
>>>>>>> origin/master

	AngusMovement player;

	int oxygenAmount;
<<<<<<< HEAD
=======
	public int levelTimer;
>>>>>>> origin/master

	private GUIStyle style = new GUIStyle ();

	public Canvas mainMenu;
	public Canvas playerUI;
	public Canvas mapMenu;

	public Image playerHealthBar;
	public Image playerOxygenBar;
	public Image playerEnergyBar;
	public Image machineStateMeter1;
	public Image machineStateMeter2;

	public Text textGlobalSupply;
	public Text textGlobalDemand;
	public Text textAvailableGlobalSupply;
	public Text textActiveGlobalDemand;
	public Text textRoomSupply;
	public Text textRoomDemand;
	public Text textAvailableRoomSupply;
	public Text textActiveRoomDemand;

	public GameObject playerObject;

	public Color energy;
	public Color oxygen;
	public Color health;

<<<<<<< HEAD
	public int levelTimer;

=======
>>>>>>> origin/master
	public int totalLevelSupply;
	public int totalLevelDemand;

	public int availableLevelSupply;
	public int activeLevelDemand;

	public int supplyToCut = 0;
	public int numberOfDevices = 0;

	void Awake()
	{
		instance = this;
	}

	/// Identify player.
	/// Determine power figures and initiate UI.
	void Start ()
	{
		player = playerObject.GetComponent<AngusMovement> ();
        UIStart ();
		updateUI ();
		updateAllSupplyDemand (totalLevelSupply, totalLevelDemand, availableLevelSupply, activeLevelDemand);
		tallyTotalLevelPower();
		InvokeRepeating ("countdown", 1f, 1f);
<<<<<<< HEAD
=======
//		tallyActiveRoomDemands ();
>>>>>>> origin/master
	}

	/// Update Player UI and power figures.
	void Update ()
	{
        updateUI ();
		updateAllSupplyDemand (totalLevelSupply, totalLevelDemand, availableLevelSupply, activeLevelDemand);

		if (availableLevelSupply < 0 || activeLevelDemand < 0) 
		{
			availableLevelSupply = 0;
			activeLevelDemand = 0;
		}

		if (levelTimer < 0)
		{
			levelTimer = 0;
		}
    }

	/// Runs the UI.
	void UIStart ()
	{
//		playerHealthBar.color = health;
//		playerOxygenBar.color = oxygen;
//		playerEnergyBar.color = energy;
		machineStateMeter1.color = Color.white;
		machineStateMeter2.color = Color.white;
		textGlobalSupply.text = System.Convert.ToString(totalLevelSupply);
		textGlobalDemand.text = System.Convert.ToString(totalLevelDemand);
		textAvailableGlobalSupply.text = System.Convert.ToString(availableLevelSupply);
		textActiveGlobalDemand.text = System.Convert.ToString(activeLevelDemand);
	}

	/// Updates the Player UI.
	void updateUI ()
	{
		float currentHealth = player.currentHealth;
		float startHealth = player.startHealth;
		playerHealthBar.fillAmount = (float)(currentHealth / startHealth);
		playerEnergyBar.fillAmount = (float)(player.currentEnergy / player.startEnergy);
		playerOxygenBar.fillAmount = (float)(player.currentOxygen / player.startOxygen);
	}

	/// Tallies the total level power.
	/// Summed up by the total room values & door values.
	public void tallyTotalLevelPower ()
	{
		for (int x = 0; x < roomList.Count; x++) 
		{
			GameObject section = roomList[x];
			RoomScript sectionScript = section.GetComponent<RoomScript> ();
			FuseBox sectionBox = sectionScript.roomFuseBox.GetComponent<FuseBox> ();
			for (int y = 0; y < sectionBox.roomObjects.Count; y++) 
			{
				ObjectClass machine = sectionBox.roomObjects[y].GetComponent<ObjectClass> ();
				totalLevelSupply += machine.powerSupply;
				totalLevelDemand += machine.powerDemand;
<<<<<<< HEAD
				numberOfDevices += 1;
			}
		}

		for (int x = 0; x < doorList.Count; x++)
		{
			GameObject door = doorList [x];
			if (door.GetComponent<DoorScript> ())
			{
				DoorScript doorWay = door.GetComponent<DoorScript> ();
				totalLevelDemand += doorWay.powerDemand;
			}
			if (door.GetComponent<DoorCompleteScript> ())
			{
				DoorCompleteScript doorWayComp = door.GetComponent<DoorCompleteScript> ();
				totalLevelDemand += doorWayComp.powerDemand;
			}
		}
=======
			}
		}
>>>>>>> origin/master
	}

	/// Updates the room power UI.
	public void updateRoomUI (int supply, int demand, int activeSupply, int activeDemand)
    {
		textRoomSupply.text = System.Convert.ToString(supply);
		textRoomDemand.text = System.Convert.ToString(demand);
		textAvailableRoomSupply.text = System.Convert.ToString(activeSupply);
		textActiveRoomDemand.text = System.Convert.ToString(activeDemand);
    }

   /// Updates the global power UI.
	public void updateAllSupplyDemand (int supply, int demand, int availableSupply, int activeDemand)
    {
		textGlobalSupply.text = System.Convert.ToString(supply);
		textGlobalDemand.text = System.Convert.ToString(demand);
		textAvailableGlobalSupply.text = System.Convert.ToString(availableSupply);
		textActiveGlobalDemand.text = System.Convert.ToString(activeDemand);   
	}
		
	/// Increases active global supply by the values of a room.
	public void levelRoomPowerUp (int demand)
	{
		availableLevelSupply -= demand;
		activeLevelDemand += demand;
	}

	/// Decreases active global supply by the values of a room.
	public void levelRoomPowerDown (int demand)
	{
		availableLevelSupply += demand;
		activeLevelDemand -= demand;
	}

	/// Increases active global supply by the value of a single object.
	public void levelObjectPowerUp (int demand)
	{
		availableLevelSupply -= demand;
		activeLevelDemand += demand;
	}

	/// Decreases active global supply by the values of a single object.
	public void levelObjectPowerDown (int demand)
	{
		availableLevelSupply += demand;
		activeLevelDemand -= demand;
	}

<<<<<<< HEAD
	/// Increases active global supply by the value of a single door.
	public void levelDoorPowerUp (int demand)
	{
		availableLevelSupply -= demand;
		activeLevelDemand += demand;
	}

	/// Decreases active global supply by the values of a single door.
	public void levelDoorPowerDown (int demand)
	{
		availableLevelSupply += demand;
		activeLevelDemand -= demand;
	}
		
=======
>>>>>>> origin/master
	public void countdown ()
	{
		levelTimer -= 1;
	}

<<<<<<< HEAD
	// Converts timer int to real time & displays it in UI.
=======
>>>>>>> origin/master
	void OnGUI () 
	{
		style.fontSize = 20;
		style.normal.textColor = Color.cyan;

		int minutes = Mathf.FloorToInt (levelTimer / 60f);
		int seconds = Mathf.FloorToInt (levelTimer - minutes * 60);
		string niceTime = string.Format ("{0:0}:{1:00}", minutes, seconds);
		GUI.Label (new Rect (10, 10, 250, 100), niceTime, style);
<<<<<<< HEAD
=======
	}

//	public void tallyActiveRoomDemands ()
//	{
//		foreach (GameObject zone in roomList)
//		{
//			RoomScript zoneScript = zone.GetComponent<RoomScript> ();
//			activeRoomDemands.Add (zoneScript.currentRoomDemand);
//		}
//	}

	public void trackActiveRoomDemands ()
	{
//		for (int a; a < activeRoomDemands.Count; a++)
//		{
//			a = a;
//		}
>>>>>>> origin/master
	}
}


