﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AngusMovement : MonoBehaviour 
{
	public int powerPack = 0;

    public float startHealth = 100.0f;
    public float currentHealth;

    public float startEnergy = 100.0f;
	public float currentEnergy;

    public float startOxygen = 100.0f;
	public float currentOxygen;

	public float healthDown = 1f;
	public float energyDown = 0.5f;
	public float oxygenDown = 0.3f;

	public float targetDistance = 0;
	public float maxRange = 30;

    public bool isAlive;
	public bool isGrounded;
	public bool isJumping = false;
	public bool isDoubleJumping = false;
	public bool isOnCeiling;

	public Image targetIn;
	public Camera fpsCameraIn;
	public Rigidbody rigBod;

	RaycastHit hit;
	GameObject currentHitTarget;

	RoomScript room;
	GameManager gameMngr;

    void Start () 
	{
		currentHealth = startHealth;
		currentEnergy = startEnergy;
		currentOxygen = startOxygen;
		rigBod = transform.GetComponent<Rigidbody> ();
		gameMngr = GameManager.instance;
    }
		
		void Update()
	{	
		//Press spacebar to jump.
		if (Input.GetKeyDown (KeyCode.Space) && !isJumping && isGrounded) 
		{
			isJumping = true;
			Debug.Log ("Jump around");
		}

		//Press spacebar again to double jump.
		else if (Input.GetKeyDown (KeyCode.Space) && isJumping && !isDoubleJumping) 
		{	
			isDoubleJumping = true;
			rigBod.AddForce (new Vector3 (0, 200, 0));
		}

		//Press spacebar while on the ceiling to get down.
		else if (Input.GetKeyDown (KeyCode.Space) && isOnCeiling == true) 
		{
			rigBod.AddForce (new Vector3 (0, -1000, 0));
			Debug.Log ("& Get Down");
		}

		//Me having some fun with debugging.
		if (Input.GetKeyDown (KeyCode.J))
		{
			Debug.Log ("Jump up");
		}

		//Hide cursor to aid targeting.
		//Press Esc to make it re-appear.
		Cursor.lockState = CursorLockMode.Locked;
		if (Input.GetKeyDown ("escape")) 
		{
			Cursor.visible = true;
		}

		//Commands for when target reticle is aimed at an object and left mouse is clicked.
		if (Input.GetMouseButtonDown (0)) 
		{
			currentHitTarget = hit.collider.gameObject;

			if (Physics.Raycast (fpsCameraIn.transform.position, fpsCameraIn.transform.forward, out hit)) 
			{
				//If target contains the ObjectsList script and is within range.
				if (currentHitTarget.GetComponent<ObjectClass> () && targetDistance <= maxRange) 
				{
					if (currentHitTarget.transform.tag == "FuseBox") 
					{
						currentHitTarget.GetComponent<FuseBox>().changeState(currentHitTarget);
					}

					if (currentHitTarget.transform.tag == "Generator") 
					{
						PowerGen targetGen = currentHitTarget.GetComponent<PowerGen>();
						if (!targetGen.stateActive ()) 
						{
							targetGen.poweringUp = true;
						} 
						else
							targetGen.changeState (targetGen.gameObject);
					}

					if (currentHitTarget.transform.tag == "Device") 
					{
						currentHitTarget.GetComponent<PowerDrain>().changeState(currentHitTarget);
					}
				}

				//If the object is activated by the mouse click.
				if (currentHitTarget.GetComponent<ObjectClass> () &&
					currentHitTarget.GetComponent<ObjectClass> ().stateActive()) 
				{
					//Do this function saps some of the suit's power.
					useSuitPower ();
				}
			}
		} 
		else 
		{
			/*Commands for when target reticle is just aimed at an object.
			 * Basically depending on what your aiming at change the color of the reticle.
			 * If it has the Door script, yellow.
			 * If it has the Object script, blue.
			*/
			if (Physics.Raycast (fpsCameraIn.transform.position, fpsCameraIn.transform.forward, out hit)) 
			{
				targetDistance = Vector3.Distance (transform.position, hit.collider.gameObject.transform.position);

				if (targetDistance <= maxRange) 
				{
					if (hit.collider.gameObject.GetComponent<DoorScript>()) 
					{
						targetIn.GetComponent<Image> ().color = Color.yellow;
					}
					else if (hit.collider.gameObject.GetComponent<FuseBox>()) 
					{
						targetIn.GetComponent<Image> ().color = Color.blue;
					}
					else if (hit.collider.gameObject.GetComponent<PowerGen>()) 
					{
						targetIn.GetComponent<Image> ().color = Color.green;
					}
					else if (hit.collider.gameObject.GetComponent<PowerDrain>()) 
					{
						targetIn.GetComponent<Image> ().color = Color.magenta;
					}
					else if (hit.collider.gameObject.GetComponent<RoomScript>()) 
					{
						targetIn.GetComponent<Image> ().color = Color.cyan;
					}
					else 
					{
						targetIn.GetComponent<Image> ().color = Color.white;
					}
				}
			}
		}

		//Drain the suit's power over time.
		currentEnergy -= energyDown * Time.deltaTime;

		if (currentEnergy <= 0) 
		{
			currentEnergy = 0;
		}

		if (currentOxygen <= 0)
		{
			currentOxygen = 0;
		}

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			isAlive = false;
		}
	}

	/// <summary>
	/// While the Player is colliding with a trigger.
	/// </summary>
	/// <param name="detector">Detector.</param>
	void OnTriggerStay(Collider detector)
	{
		/*If the trigger's tag is this.
			Player is Grounded, not jumping, and not double jumping.
		*/
		if (detector.transform.tag == "GravityBottom")
		{
			isGrounded = true;
			isJumping = false;
			isDoubleJumping = false;
		}
	}

	//When the Player leaves a trigger.
	void OnTriggerExit(Collider detector)
	{
		/*If the trigger's tag is this.
		Player is jumping, and is not grounded.
		*/
		if (detector.transform.tag == "GravityBottom")
		{
			isJumping = true;
			isGrounded = false;
		}
	}

	/// <summary>
	/// Player's Health increase.
	/// It can't go below 0 or above start amount.
	/// </summary>
	/// <param name="amount">positive values heal, negative values damage</param>
	public void changeHealth(float amount)
	{
		currentHealth += amount;

		if (currentHealth <= 0)
		{
			currentHealth = 0;
			isAlive = false;
		}
		else if (currentHealth > startHealth)
		{
			currentHealth = startHealth;
		}
	}

	/// <summary>
	/// Players Oxygen Level (Suit level) increase.
	/// It can't go below 0 or above start amount.
	/// </summary>
	/// <param name="amount">positive values increase, negative values Decrease</param>
	public void changeOxygen(float amount)
	{
		currentOxygen += amount;

		if (currentOxygen <= 0)
		{
			currentOxygen = 0;
		}
		else if (currentOxygen > startOxygen)
		{
			currentOxygen = startOxygen;
		}
	}

	/// <summary>
	/// Player Energy Level (Suit level) Increase.
	/// It can't go below 0 or above start amount.
	/// </summary>
	/// <param name="amount">positive values increase, negative values decrease</param>
	public void changeEnergy(float amount)
	{
		currentEnergy += amount;

		if (currentEnergy <= 0)
		{
			currentEnergy = 0;
		}
		else if (currentEnergy > startEnergy)
		{
			currentEnergy = startEnergy;
		}
	}

	//Suit power drain for when device is turned on by target click as above.
	public void useSuitPower()
	{
		currentEnergy -= 5.0f;
	}

	public void changeStateMeterColors(Color32 colour)
	{
		gameMngr.machineStateMeter1.color = colour;
		gameMngr.machineStateMeter2.color = colour;
	}
}