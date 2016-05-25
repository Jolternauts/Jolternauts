using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ObjectClass : MonoBehaviour
{
	public bool isOn;
	public bool isActive;
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


	void Start ()
	{
		myName = this.gameObject.transform.name;
		myTag = this.gameObject.transform.tag;
		boxColl = this.gameObject.GetComponent<BoxCollider> ();
		player = GameObject.FindWithTag ("Player").GetComponent<AngusMovement>();
		gameMngr = GameManager.instance;
		rend = this.gameObject.GetComponent<Renderer> ();
		objectStartup ();
	}

	void Update ()
	{
		
	}
		
	// While player is colliding and R is held down repair object:
	void OnTriggerStay (Collider detector)
	{
		if (detector.transform.tag == "Player") 
		{
			if (Input.GetKeyDown (KeyCode.R) && this.gameObject.GetComponent<ObjectsList> ().isDamaged) 
			{
				repairObject (this.gameObject);
			}
		}
	}

	/// Wait function.
	IEnumerator Stall ()
	{
		Debug.Log ("Buffering");
		yield return new WaitForSeconds (repairTime);
	}

	/// Repairs the object.
	public void repairObject (GameObject device)
	{
		StartCoroutine (Stall ());
		Renderer deviceRend = device.GetComponent<Renderer> ();
		isDamaged = false;
		isActive = false;
		gameMngr.machineStateMeter1.color = offColor;
		deviceRend.material.color = offColor;
	}

	/// This detects if an object has a box collider or a renderer.
	/// And changes their color and trigger status.
	void objectStartup ()
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

	/// Initializes a new instance of the <see cref="ObjectClass"/> class.
	public ObjectClass (){}

	/// Initializes a new instance of the <see cref="ObjectClass"/> class.
	public ObjectClass (bool setOn, bool setActive, bool setDamage)
	{
		isOn = setOn;
		isActive = setActive;
		isDamaged = setDamage;
	}

	// Sets bool state.
	public void stateOn (bool setOn)
	{
		isOn = setOn;
	}

	// Returns bool state.
	public bool stateOn ()
	{
		return isOn;
	}
		
	// Sets bool state.
	public void stateActive (bool setActive)
	{
		isActive = setActive;
	}

	// Returns bool state.
	public bool stateActive ()
	{
		return isActive;
	}

	// Sets bool state.
	public void stateDamaged (bool setDamage)
	{
		isDamaged = setDamage;
	}

	// Returns bool state.
	public bool stateDamaged ()
	{
		return isDamaged;
	}

	// Sets bool state.
	public void statePressed (bool setPress)
	{
		keyPressed = setPress;
	}

	// Returns bool state.
	public bool statePressed ()
	{
		return keyPressed;
	}

	/// Checks if any machines are damaged and affects the rest accordingly.
	public void massCrashCheck ()
	{
		PowerGen gen = this.gameObject.GetComponent<PowerGen> ();
		PowerDrain drain = this.gameObject.GetComponent<PowerDrain> ();

		if (stateActive () && !stateDamaged () || stateOn () && !stateDamaged ()) 
		{
			stateActive (false);
			stateOn (false);
			gen.changeRendColor (offColor);
			drain.changeRendColor (offColor);
		}
		else if (stateActive () && stateDamaged () || stateOn () && stateDamaged ()) 
		{
			stateDamaged (true);
			stateActive (false);
			stateOn (false);
			gen.changeRendColor (offColor);
			drain.changeRendColor (offColor);		
		}
	}
}


