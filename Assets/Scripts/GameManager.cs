using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour 
{
	public static GameManager instance;

	public List <GameObject> roomList = new List<GameObject>();
	public List <GameObject> doorList = new List<GameObject>();

	DoorScript Door;
	RoomScript Room;
	AngusMovement player;

	int oxygenAmount;
	int levelTimer;

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

	public int totalLevelSupply;
	public int totalLevelDemand;

	public int availableLevelSupply;
	public int activeLevelDemand;

    int number;

	void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Identify player.
	/// Determine power figures and initiate UI.
	/// </summary>
	void Start()
	{
		player = playerObject.GetComponent<AngusMovement> ();
        UIStart ();
		updateUI ();
		updateAllSupplyDemand (totalLevelSupply, totalLevelDemand, availableLevelSupply, activeLevelDemand);
		tallyTotalLevelPower();
	}

	/// <summary>
	/// Update Player UI and power figures.
	/// </summary>
	void Update()
	{
        updateUI ();
		updateAllSupplyDemand (totalLevelSupply, totalLevelDemand, availableLevelSupply, activeLevelDemand);

		if (availableLevelSupply < 0 || activeLevelDemand < 0) 
		{
			availableLevelSupply = 0;
			activeLevelDemand = 0;
		}
    }

	/// <summary>
	/// Runs the UI.
	/// </summary>
	void UIStart()
	{
		playerHealthBar.color = Color.red;
		playerOxygenBar.color = Color.blue;
		playerEnergyBar.color = Color.green;
		machineStateMeter1.color = Color.white;
		machineStateMeter2.color = Color.white;
		textGlobalSupply.text = System.Convert.ToString(totalLevelSupply);
		textGlobalDemand.text = System.Convert.ToString(totalLevelDemand);
		textAvailableGlobalSupply.text = System.Convert.ToString(availableLevelSupply);
		textActiveGlobalDemand.text = System.Convert.ToString(activeLevelDemand);
	}

	/// <summary>
	/// Updates the Player UI.
	/// </summary>
	void updateUI()
	{
		float currentHealth = player.currentHealth;
		float startHealth = player.startHealth;
		playerHealthBar.fillAmount = (float)(currentHealth / startHealth);
		playerEnergyBar.fillAmount = (float)(player.currentEnergy / player.startEnergy);
		playerOxygenBar.fillAmount = (float)(player.currentOxygen / player.startOxygen);
	}

	/// <summary>
	/// Tallies the total level power.
	/// Summed up by the total room values.
	/// </summary>
	public void tallyTotalLevelPower()
	{
		foreach(GameObject room in roomList)
		{			
			RoomScript currentRoom = room.GetComponent<RoomScript> ();
			totalLevelSupply += currentRoom.totalRoomSupply;
			totalLevelDemand += currentRoom.totalRoomDemand;
       }
	}

	/// <summary>
	/// Updates the room power UI.
	/// </summary>
	/// <param name="supply">Supply.</param>
	/// <param name="demand">Demand.</param>
	public void updateRoomUI(int supply, int demand, int activeSupply, int activeDemand)
    {
		textRoomSupply.text = System.Convert.ToString(supply);
		textRoomDemand.text = System.Convert.ToString(demand);
		textAvailableRoomSupply.text = System.Convert.ToString(activeSupply);
		textActiveRoomDemand.text = System.Convert.ToString(activeDemand);
    }

   /// <summary>
   /// Updates the global power UI.
   /// </summary>
   /// <param name="supply">Supply.</param>
   /// <param name="demand">Demand.</param>
	public void updateAllSupplyDemand(int supply, int demand, int activeSupply, int activeDemand)
    {
		textGlobalSupply.text = System.Convert.ToString(supply);
		textGlobalDemand.text = System.Convert.ToString(demand);
		textAvailableGlobalSupply.text = System.Convert.ToString(activeSupply);
		textActiveGlobalDemand.text = System.Convert.ToString(activeDemand);   
	}


	/// <summary>
	/// Increases active global supply by the values of a room.
	/// </summary>
	/// <param name="supply">Supply.</param>
	/// <param name="demand">Demand.</param>
	public void levelRoomPowerUp(int demand)
	{
		availableLevelSupply -= demand;
		activeLevelDemand += demand;
	}

	/// <summary>
	/// Decreases active global supply by the values of a room.
	/// </summary>
	/// <param name="supply">Supply.</param>
	/// <param name="demand">Demand.</param>
	public void levelRoomPowerDown(int demand)
	{
		availableLevelSupply += demand;
		activeLevelDemand -= demand;
	}

	/// <summary>
	/// Increases active global supply by the value of a single object.
	/// </summary>
	/// <param name="supply">Supply.</param>
	/// <param name="demand">Demand.</param>
	public void levelObjectPowerUp(int demand)
	{
		availableLevelSupply -= demand;
		activeLevelDemand += demand;
	}

	/// <summary>
	/// Decreases active global supply by the values of a single object.
	/// </summary>
	/// <param name="supply">Supply.</param>
	/// <param name="demand">Demand.</param>
	public void levelObjectPowerDown(int demand)
	{
		availableLevelSupply += demand;
		activeLevelDemand -= demand;
	}
}