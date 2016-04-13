using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectClass : MonoBehaviour
{
	//isOn defines if the device is on.
	public bool isOn;

	//isActive defines if it's able to function.
	public bool isActive;

	//isActive defines if it's damaged.
	public bool isDamaged;

	bool keyPressed;

	public float repairTime;

	public int powerSupply;
	public int powerDemand;

	public Color offColor = Color.red;
	public Color neutralColor = Color.grey;
	public Color activeColor = Color.green;
	public Color damagedColor = Color.black;

	public string myName;
	public string myTag;

	public AngusMovement player;
	public GameManager gameMngr;

	BoxCollider boxColl;
	Renderer rend;

	void Start()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		boxColl = this.gameObject.GetComponent<BoxCollider> ();
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		gameMngr = GameManager.instance;
		rend = this.gameObject.GetComponent<Renderer> ();
		objectStartup ();
	}

	void Update()
	{
		
	}



	// While player is colliding and R is held down repair object:
	void OnTriggerStay(Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			if (Input.GetKey (KeyCode.R) && this.gameObject.GetComponent<ObjectsList> ().isDamaged) 
			{
				repairObject (this.gameObject);
			}
		}
	}

	/// <summary>
	/// Repairs the object.
	/// </summary>
	/// <param name="device">Device.</param>
	public void repairObject(GameObject device)
	{
		Renderer deviceRend = device.GetComponent<Renderer> ();

		// Change stateMeter2 color. (StateMeter2 is Object UI for the action of repairing it).
		// Fill stateMeter2 over time. (Duration depends on object's individual repair time).
		gameMngr.machineStateMeter2.color = offColor;
		gameMngr.machineStateMeter2.fillAmount += 1.0f / repairTime * Time.deltaTime;

		/* When meter is filled:
			 * Empty the meter.
			 * Object no longer damaged.
			 * Turn Object to OFF.
			 * Change stateMeter2 color.
			 * Change Object color.
			*/
		if (gameMngr.machineStateMeter2.fillAmount == 1.0f) 
		{
			gameMngr.machineStateMeter2.fillAmount = 0f;
			isDamaged = false;
			isActive = false;
			gameMngr.machineStateMeter1.color = offColor;
			deviceRend.material.color = offColor;
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
		} 
		else if (boxColl && rend == null) 
		{
			boxColl.isTrigger = true;
		} 
		else if (boxColl == null && rend)
		{
			rend.material.color = offColor;
		} 
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ObjectClass"/> class.
	/// </summary>
	public ObjectClass(){}

	/// <summary>
	/// Initializes a new instance of the <see cref="ObjectClass"/> class.
	/// </summary>
	/// <param name="setOn">If set to <c>true</c> set on.</param>
	/// <param name="setActive">If set to <c>true</c> set active.</param>
	/// <param name="setDamage">If set to <c>true</c> set damage.</param>
	public ObjectClass(bool setOn, bool setActive, bool setDamage)
	{
		isOn = setOn;
		isActive = setActive;
		isDamaged = setDamage;
	}

	/// <summary>
	/// States the on.
	/// </summary>
	/// <param name="setOn">If set to <c>true</c> set on.</param>
	public void stateOn(bool setOn)
	{
		isOn = setOn;
	}

	/// <summary>
	/// States the on.
	/// </summary>
	/// <returns><c>true</c>, if on was stated, <c>false</c> otherwise.</returns>
	public bool stateOn()
	{
		return isOn;
	}
		
	/// <summary>
	/// States the active.
	/// </summary>
	/// <param name="setActive">If set to <c>true</c> set active.</param>
	public void stateActive(bool setActive)
	{
		isActive = setActive;
	}

	/// <summary>
	/// States the active.
	/// </summary>
	/// <returns><c>true</c>, if active was stated, <c>false</c> otherwise.</returns>
	public bool stateActive()
	{
		return isActive;
	}

	/// <summary>
	/// States the damaged.
	/// </summary>
	/// <param name="setDamage">If set to <c>true</c> set damage.</param>
	public void stateDamaged(bool setDamage)
	{
		isDamaged = setDamage;
	}

	/// <summary>
	/// States the damaged.
	/// </summary>
	/// <returns><c>true</c>, if damaged was stated, <c>false</c> otherwise.</returns>
	public bool stateDamaged()
	{
		return isDamaged;
	}

/*	public void stateBeside(bool setBeside)
	{
		isBesideMachine = setBeside;
	}

	public bool stateBeside()
	{
		return isBesideMachine;
	}
*/
	public void statePressed(bool setPress)
	{
		keyPressed = setPress;
	}

	public bool statePressed()
	{
		return keyPressed;
	}

/*	public void changeColors(Color32 colour)
	{
		stateMeter1.color = colour;
		stateMeter2.color = colour;
		rend.material.color = colour;
	}
*/		

	public void massCrashCheck()
	{
		PowerGen gen = this.gameObject.GetComponent<PowerGen> ();
		PowerDrain drain = this.gameObject.GetComponent<PowerDrain> ();

		if (stateActive() && !stateDamaged() || stateOn() && !stateDamaged()) 
		{
			stateActive(false);
			stateOn(false);
			gen.changeRendColor (offColor);
			drain.changeRendColor (offColor);
		}
		else if (stateActive() && stateDamaged() || stateOn() && stateDamaged()) 
		{
			stateDamaged (true);
			stateActive(false);
			stateOn(false);
			gen.changeRendColor (offColor);
			drain.changeRendColor (offColor);		
		}
	}

		
}


