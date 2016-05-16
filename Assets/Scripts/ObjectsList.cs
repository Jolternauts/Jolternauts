using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectsList : MonoBehaviour 
{
	public int powerDemand;
	public int powerSupply;

	public bool isNeutral = false;
	public bool isActive = false;
	public bool isDamaged = false;
	public bool poweringUp = false;
	bool keyPressed = false;

	public float repairTime;

	Color offColor = Color.red;
	Color neutralColor = Color.grey;
	Color activeColor = Color.green;
	Color damagedColor = Color.black;

	public Image stateMeter1;
	public Image stateMeter2;

	GameManager gameMngr;
	AngusMovement player;
	RoomScript Room;
	ObjectsList script;

	BoxCollider boxColl;
	Renderer rend;

	string myName;
	string myTag;

	// These are here for shortcutted reference or to initiate functions.
	void Start ()
    {
		boxColl = this.gameObject.GetComponent<BoxCollider> ();
		rend = this.gameObject.GetComponent<Renderer> ();
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		Room = this.gameObject.GetComponentInParent<RoomScript> ();
		script = this.gameObject.GetComponent<ObjectsList> ();
        gameMngr = GameManager.instance;
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		objectStartup ();
    }

	void Update()
	{
		if (poweringUp) 
		{
			powerUp ();
		}
	}

	void OnTriggerStay(Collider detector)
	{
		// Detect player at object and turn it on by pressing E.
	    // If it's a charge station: replenish health, suit power and oxygen.

		if (detector.transform.tag == "Player") 
		{
			if (Input.GetKeyDown (KeyCode.E) && !keyPressed) 
			{
				if (myTag == "Generator" && !script.isActive) 
				{
					poweringUp = true;
					keyPressed = true;
				}
				else 
				{
					if (Room.availableRoomSupply > 0) 
					{
						if (myName == "ChargeStation") 
						{
							player.changeHealth (100f);
							player.changeEnergy (100f);
							player.changeOxygen (100f);
						}
						if (myName == "FuseBox") 
						{
							Room.roomFuseBox.GetComponent<FuseBox>().roomStateChange ();
						}
						changeState(this.gameObject);
						keyPressed = true;
					}
				}
			}
			else if (Input.GetKeyUp(KeyCode.E) && keyPressed)
			{
				keyPressed = false;
			}
				
			/* While player is colliding and R is held down repair object:
			 * If colliding with a Fusebox, check if any other devices are damaged.
			 * If they are then cannot repair Fusebox until device is fixed.
			 * If not colliding with Fusebox then just repair.
			 * Not sure why yet but this constraint doesn't work.
			*/
			if (Input.GetKey (KeyCode.R) && this.gameObject.GetComponent<ObjectsList> ().isDamaged) 
			{
				if (myName == "Fusebox") 
				{
					for (int x = 0; x < Room.roomItems.Count; x++) 
					{
						ObjectsList massScript = Room.roomItems [x].GetComponent<ObjectsList> ();

						if (massScript.isDamaged) 
						{
							Debug.Log ("Trying to repair FuseBox");
							Debug.Log ("Cannot fix this yet");
						}
					}
				}
				else 
					repairObject (this.gameObject);
			}

			if (Input.GetKeyDown (KeyCode.T) && !keyPressed) 
			{
				if (myName == "FuseBox")
				{
					if (Room.availableRoomSupply > 0) 
					{
//						Room.roomFuseBox.GetComponent<FuseBox>().storePowerPackSupply ();
					}
				}
			} 
			else if (Input.GetKeyUp (KeyCode.T) && keyPressed) 
			{
				keyPressed = false;
			}

			if (Input.GetKeyDown (KeyCode.G) && !keyPressed) 
			{
				if (myName == "FuseBox") 
				{
					if (Room.availableRoomSupply == 0)
					{
//						Room.roomFuseBox.GetComponent<FuseBox>().sharePowerPackSupply ();
					}
				}
			} 
			else if (Input.GetKeyUp (KeyCode.G) && keyPressed) 
			{
				keyPressed = false;
			}
        }
    }
		
	/// <summary>
	/// This detects if an object has a box collider or a renderer.
	/// And changes their color and trigger status.
	/// </summary>
	void objectStartup()
	{
		if (boxColl && rend) 
		{
			boxColl.isTrigger = true;
			rend.material.color = offColor;
			stateMeter1.color = offColor;
		} 
		else if (boxColl && rend == null) 
		{
			boxColl.isTrigger = true;
		} 
		else if (boxColl == null && rend)
		{
			rend.material.color = offColor;
			stateMeter1.color = offColor;
		} 
	}
		
	/// <summary>
	/// Changes the state of the object based on its own state and the room's state. 
	/// </summary>
	/// <param name="reference">Reference.</param>
    public void changeState(GameObject reference)
	{
		Renderer refRend = reference.gameObject.GetComponent<Renderer> ();
		FuseBox box = Room.roomFuseBox.GetComponent<FuseBox> ();

		// If the object is a gravity, do nothing.
		if (reference.transform.name == "GravityGround" || reference.transform.name == "GravityFloor" ||
			reference.transform.name == "GravityRoof" || reference.transform.name == "GravityCeiling") 
		{
			
		} 
		else // This is where the fun starts. :P
		{
			// If the Fusebox is On or if the object is the Fusebox or a "Generator".
			if (box.stateActive() || reference.name == "FuseBox") 
			{
				if (Room.availableRoomSupply > 0) 
				{
					/* If it's Active and not Damaged:
				 * Debug.
				 * Change its color.
				 * Change its stateMeter1 color. (StateMeter is Object UI for an extra visual reference for the object's state)
				 * Turn it OFF !!!.
				 * Call function to MINUS the active room power by its values.
				*/
					if (isActive && !isDamaged) 
					{
						Debug.Log ("Change State from Active called");
						refRend.material.color = offColor;
						stateMeter1.color = offColor;
						isActive = false;
						box.roomSinglePowerDown (powerDemand);
					}

					/* If it's Active and is Damaged:
				 * Debug.
				 * Change its color.
				 * Change its stateMeter1 color.
				 * Turn it OFF !!!.
				 * Call function to MINUS the active room power by its values.
				*/
					else if (isActive && isDamaged) 
					{
						Debug.Log ("Change State from Active called");
						refRend.material.color = damagedColor;
						stateMeter1.color = damagedColor;
						isActive = false;
						box.roomSinglePowerDown (powerDemand);
					}

					/* If it's Active and Not Damaged:
				 * Debug.
				 * Change its color.
				 * Change its stateMeter1 color.
				 * Turn it ON !!!.
				 * Call function to PLUS the active room power by its values.
				*/

					else if (!isActive && !isDamaged) 
					{
						Debug.Log ("Change State to Active called");
						refRend.material.color = activeColor;
						stateMeter1.color = activeColor;
						isActive = true;
						box.roomSinglePowerUp (powerDemand);
					}

					/* If it's not Active but is Damaged:
				 * Debug.
				 * Change its color.
				 * Change its stateMeter1 color.
				 * Turn it ON !!!.
				 * Call function to crash room.
				*/
					else if (!isActive && isDamaged) 
					{
						Debug.Log ("Change State to Active called");
						refRend.material.color = damagedColor;
						stateMeter1.color = damagedColor;
						isActive = true;
						Room.roomFuseBox.GetComponent<FuseBox>().roomCrash ();
					}
					else // Debug error with object's name.
						Debug.Log ("ObjectScript ChangeState Error" + this.name);	
				}
			}

			else if (reference.tag == "Generator") 
			{
				if (isActive && !isDamaged) 
				{
					Debug.Log ("Change State from Active called");
					refRend.material.color = offColor;
					stateMeter1.color = offColor;
					isActive = false;
					box.roomSinglePowerDown (powerDemand);
				}

				else if (isActive && isDamaged) 
				{
					Debug.Log ("Change State from Active called");
					refRend.material.color = damagedColor;
					stateMeter1.color = damagedColor;
					isActive = false;
					box.roomSinglePowerDown (powerDemand);
				}

				else if (!isActive && !isDamaged) 
				{
					Debug.Log ("Change State to Active called");
					refRend.material.color = activeColor;
					stateMeter1.color = activeColor;
					isActive = true;
					Room.availableRoomSupply += powerSupply;
					box.roomSinglePowerUp (powerDemand);
				}

				else if (!isActive && isDamaged) 
				{
					Debug.Log ("Change State to Active called");
					refRend.material.color = damagedColor;
					stateMeter1.color = damagedColor;
					isActive = true;
					Room.roomFuseBox.GetComponent<FuseBox>().roomCrash ();
				}
				else // Debug error with object's name.
					Debug.Log ("ObjectScript ChangeState Error" + this.name);
			}

			else // If the room isn't powered:
			{
				/* If the object is not Neutral: (That means not off but with no power reaching it anyway.)
				 * Debug.
				 * Change its color.
				 * Change its stateMeter1 color.
				 * Turn it to Neutral !!!.
				*/
				if (!isNeutral) 
				{
					Debug.Log ("Change State to Neutral called");
					refRend.material.color = neutralColor;
					stateMeter1.color = neutralColor;
					isNeutral = true;
				}

				/* If the object is Neutral:
				 * Debug.
				 * Change its color.
				 * Change its stateMeter1 color.
				 * Turn it to OFF !!!.
				*/
				else if (isNeutral) 
				{
					Debug.Log ("Change State from Neutral called");
					refRend.material.color = offColor;
					stateMeter1.color = offColor;
					isNeutral = false;
				}
				else // Debug error with object's name.
					Debug.Log ("ObjectScript ChangeState Error" + this.name);
			}

			// If the object is a Fusebox: (some extra special treatment)
			// Call the roomCheck function.
			if (reference.name == "FuseBox") 
			{
				Room.roomFuseBox.GetComponent<FuseBox>().roomStateCheck ();
				gameMngr.levelObjectPowerDown (powerDemand);
			} 
		}
	}

	/// <summary>
	/// Deals with repairing an object.
	/// </summary>
	/// <param name="device">Device.</param>
	public void repairObject(GameObject device)
	{
		Renderer deviceRend = device.GetComponent<Renderer> ();

		// Change stateMeter2 color. (StateMeter2 is Object UI for the action of repairing it).
		// Fill stateMeter2 over time. (Duration depends on object's individual repair time).
		stateMeter2.color = offColor;
		stateMeter2.fillAmount += 1.0f / repairTime * Time.deltaTime;

		/* When meter is filled:
			 * Empty the meter.
			 * Object no longer damaged.
			 * Turn Object to OFF.
			 * Change stateMeter2 color.
			 * Change Object color.
			*/
		if (stateMeter2.fillAmount == 1.0f) 
		{
			stateMeter2.fillAmount = 0f;
			isDamaged = false;
			isActive = false;
			stateMeter1.color = offColor;
			deviceRend.material.color = offColor;
		}
	}

	public void powerUp()
	{
		// Change stateMeter2 color. (StateMeter2 is Object UI for the action of activating a power supplier).
		// Fill stateMeter2 over time. (Duration depends on object's individual activation time (as represented by repair time)).
		stateMeter2.color = activeColor;
		stateMeter2.fillAmount += 1.0f / repairTime * Time.deltaTime;

		/* When meter is filled:
		 * Empty the meter.
		 * Change Object state.
		 */
		if (stateMeter2.fillAmount == 1.0f) 
		{
			stateMeter2.fillAmount = 0f;
			changeState (this.gameObject);
			poweringUp = false;
		}
	}
}