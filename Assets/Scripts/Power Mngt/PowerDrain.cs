using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PowerDrain : ObjectClass
{
	//this is for anything that demands power

	RoomScript room;
	FuseBox box;
	Renderer deviceRend;

	public bool besideDevice;
	public bool uploadComplete;
	bool showGUI = false;
	bool showWin = false;

	int launchTimer = 10;
	private GUIStyle launchStyle = new GUIStyle ();
	private GUIStyle winStyle = new GUIStyle ();


	void Start()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		gameMngr = GameManager.instance;
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		box = room.roomFuseBox.GetComponent<FuseBox> ();
		deviceRend = this.gameObject.GetComponent<Renderer> ();
//		launchSequence ();
//		showWin = true;
	}

	void Update()
	{

	}

	// Change state of device.
	public void changeState (GameObject reference)
	{
		// If fusebox is active.
		if (box.stateActive ()) 
		{
			// If A.R.S is over 0.
			// Change active / damaged states and elements.
			if (!stateActive ()) 
			{
				if ((room.availableRoomSupply - powerDemand) >= 0)
				{
					drainerStateChangeCriteria ();
					if (myName == "Health_Console") 
					{
						recharge ();
					}
				} 
			} 
			else 
			{
				drainerStateChangeCriteria ();
				gameMngr.objectsToCut.Remove (this.gameObject);
			}
		} 
		else
		{
			if (!stateOn ()) 
			{
				changeRendColor (neutralColor);
				stateOn (true);
			}

			else if (stateOn ()) 
			{
				changeRendColor (offColor);
				stateOn (false);
			} 
			else // Debug error with object's name.
				Debug.Log ("ObjectScript ChangeState Error" + this.name);

			machineStateMeterCheck ();
		}
	}

	void OnTriggerEnter (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideDevice = true;
			machineStateMeterCheck ();
		}
	}

	void OnTriggerStay (Collider detector)
	{
		/// Detect player at object.
		/// Turn it on by pressing E.
		/// If it's a charge station: replenish health, suit power and oxygen.
		/// Upload files by pressing U.
		/// If it's goal console, all other devices must be uploaded first.
		/// If it's goal console and criteria met, initiate launch sequence by pressing L.
		if (detector.transform.tag == "Player")
		{
			if (Input.GetKeyDown (KeyCode.E) && !statePressed ()) 
			{
				changeState (this.gameObject);
				statePressed (true);
			}
			if (Input.GetKeyUp (KeyCode.E)) 
			{
				statePressed (false);
			}

			if (Input.GetKeyDown (KeyCode.U) && !statePressed ()) 
			{
				if (stateActive ())
				{
					if (myTag == "GoalCon") 
					{
						if (gameMngr.numberOfDevicesToUpload == 1)
						{
							startUpload ();
						}
					}
					else
					{
						startUpload ();
					}
					statePressed (true);
				}
			}

			if (Input.GetKeyUp (KeyCode.U)) 
			{
				if (stateActive ()) 
				{
					if (myTag == "GoalCon") 
					{
						statePressed (false);
					}
				}				
			}

			if (Input.GetKeyDown (KeyCode.L) && !statePressed ()) 
			{
				if (stateActive ()) 
				{
					if (myTag == "GoalCon") 
					{
						if (gameMngr.filesUploadedToConsole && 
							gameMngr.reactorLive)
						{
							launchSequence ();
							statePressed (true);
						}
					}
				}
			}
		}

		machineStateMeterCheck ();
	}

	void OnTriggerExit (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			besideDevice = false;
			machineStateMeterCheck ();
		}
	}

	public void buttonUploadConditions ()
	{
		if (stateActive ())
		{
			if (myTag == "GoalCon") 
			{
				if (gameMngr.numberOfDevicesToUpload == 1)
				{
					startUpload ();
				}
			}
			else
			{
				startUpload ();
			}
		}			
	}

	public void startUpload ()
	{
		StartCoroutine (uploadFiles (this.gameObject));
	}

	/// Uploads the files.
	/// Wait, set update true, affect device upload list and figure.
	IEnumerator uploadFiles (GameObject device)
	{
		Debug.Log ("Buffering");
		yield return new WaitForSeconds (repairTime);
		uploadComplete = true;
		machineStateMeterCheck ();
		gameMngr.uploadedDevices.Add (this.gameObject);
		gameMngr.numberOfDevicesToUpload -= 1;
		if (device.tag == "GoalCon")
		{
			gameMngr.filesUploadedToConsole = true;
		}
	}

	/// Show launch countdown label.
	/// Count timer down.
	/// Get ready to stop timer and show win label.
	public void launchSequence ()
	{
		showGUI = true;
		InvokeRepeating ("finalCountdown", 1f, 1f);
		StartCoroutine (waitForWin ());
	}

	public void finalCountdown ()
	{
		launchTimer -= 1;
	}

	/// Wait till timer hits 0.
	/// Stop timer.
	/// Show win label.
	IEnumerator waitForWin ()
	{
		yield return new WaitUntil (() => launchTimer == 0);
		CancelInvoke ("finalCountdown");
		Debug.Log ("You Win");
		showWin = true;
	}

	// Show labels when criteria are met.
	void OnGUI ()
	{
		if (showGUI)
		{
			launchStyle.fontSize = 50;
			launchStyle.normal.textColor = Color.green;
			string countdown = System.Convert.ToString (launchTimer);
			GUI.Label (new Rect (10, 70, 500, 200), countdown, launchStyle);
		}

		if (showWin)
		{
			winStyle.fontSize = 75;
			winStyle.normal.textColor = Color.green;
			GUI.Label (new Rect (100, 50, 500, 200), "You Win!!!!", winStyle);
		}
	}

	// Checks if device group is damaged or not and A.R.S is enough to turn on.
	public void massActivationCheck ()
	{
		if ((room.availableRoomSupply - powerDemand) >= 0) 
		{				
			if (!stateDamaged () && stateOn ())
			{
				Debug.Log ("Safe Activation for " + myName);
				stateActive (true);
				stateOn (false);
				changeRendColor (activeColor);
				box.roomSinglePowerUp (powerDemand);
				gameMngr.levelObjectPowerUp (powerDemand);
				gameMngr.objectsToCut.Add (this.gameObject);
				if (myTag == "HealthCon")
				{
					recharge ();
				}
			}
			else if (stateDamaged () && stateOn ()) 
			{
				box.roomCrash ();
				Debug.Log (myName + " is Damaged");
				Debug.Log ("And now Fusebox is Damaged");
			} 
		}
	}

	/// If device group is turned on:
	/// if any are damaged, changed to damaged criteria.
	/// If true and others aren't make them inactive.
	public void massCrashCheckForDevice ()
	{
		if (stateActive () && !stateDamaged () || 
			stateOn () && !stateDamaged ()) 
		{
			stateActive(false);
			stateOn(false);
			changeRendColor (offColor);
		}
		else if (stateActive () && stateDamaged () || 
			 	 stateOn () && stateDamaged ()) 
		{
			stateDamaged (true);
			stateActive(false);
			stateOn (false);
			changeRendColor (damagedColor);
		}
	}

	public void machineStateMeterCheck ()
	{
		if (besideDevice) 
		{
			if (!stateActive () && !uploadComplete) 
			{
				player.changeStateMeterColors (offColor);
			}
			else if (stateActive () && !uploadComplete) 
			{
				player.changeStateMeterColors (activeColor);
			}
			else if (stateOn () && !uploadComplete) 
			{
				player.changeStateMeterColors (neutralColor);
			}
			else if (stateDamaged ()) 
			{
				player.changeStateMeterColors (damagedColor);
			}
			else if (uploadComplete)
			{
				player.changeStateMeterColors (Color.blue);
			}
		}
		else 
		{
			player.changeStateMeterColors (Color.white);
		}
	}

	public void changeRendColor (Color32 colour)
	{
		deviceRend.material.color = colour;
	}

	// Criteria to apply on state change, dependent on current state.
	public void drainerStateChangeCriteria ()
	{
		if (stateActive () && !stateDamaged ()) 
		{
			Debug.Log (myName + " de-activated");
			changeRendColor (offColor);
			stateActive (false);
			box.roomSinglePowerDown (powerDemand);
			gameMngr.levelObjectPowerDown (powerDemand);
		}
		else if (stateActive () && stateDamaged ()) 
		{
			Debug.Log ("Turning damaged " + myName + " off");
			changeRendColor (damagedColor);
			stateActive (false);
			box.roomSinglePowerDown (powerDemand);
			gameMngr.levelObjectPowerDown (powerDemand);
		}
		else if (!stateActive () && !stateDamaged ()) 
		{
			Debug.Log (myName + " activated");
			changeRendColor (activeColor);
			stateActive (true);
			box.roomSinglePowerUp (powerDemand);
			gameMngr.levelObjectPowerUp (powerDemand);
			gameMngr.objectsToCut.Add (this.gameObject);
		}
		else if (!stateActive () && stateDamaged ()) 
		{
			Debug.Log (myName + " caused a crash");
			changeRendColor (damagedColor);
			stateActive (true);
			box.roomCrash ();
		}
		else // Debug error with object's name.
			Debug.Log ("ObjectScript ChangeState Error" + this.name);
	}

	public void recharge ()
	{
		player.changeHealth (100f);
		player.changeEnergy (100f);
		player.changeOxygen (100f);
	}
}
