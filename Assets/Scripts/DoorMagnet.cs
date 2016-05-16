using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DoorMagnet : MonoBehaviour 
{

	MeshRenderer rend;
	Rigidbody body;

	public List<GameObject> allMagnets = new List<GameObject> ();

	public List<GameObject> magnetsTouching;

	public List<GameObject> magnetTargets = new List<GameObject>();

	public string polarity;

	public bool doorMagOn;
	public bool polarityCheck = true;

	public int currentStage;
	public int lerpStage;

	public Vector3 lerpTarget;

	public float averageAttractiveHeight = 0;

	// Use this for initialization
	void Start () {

		rend = transform.GetComponent<MeshRenderer> ();

		magnetsTouching = new List<GameObject> ();

//		lerpTarget = magnetTargets [currentStage].transform.position;

	}

	// Update is called once per frame
	void Update () 
	{
		if (doorMagOn) 
		{
			if (polarity == "positive") 
			{
				rend.material.color = Color.red;
			} 
			else if (polarity == "negative") 
			{
				rend.material.color = Color.blue;
			}
		}
		else 
		{
			rend.material.color = Color.gray;
		}
	}


/*	void FixedUpdate() 
	{
		averageAttractiveHeight = 0;
		int counter = 0;

		foreach (GameObject mag in allMagnets) 
		{
//			GameObject mag = allMagnets [x];
			MagnetScript magScript = mag.GetComponent<MagnetScript> ();
			if (magScript.polarity != polarity && magScript.charged) 
			{
				averageAttractiveHeight += (mag.transform.position.y * (magScript.magnetStrength / 100));
				counter++;
				transform.localPosition = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, averageAttractiveHeight, transform.position.z), 0.1f);
			}

		}

		if (counter > 0) 
		{
			averageAttractiveHeight = averageAttractiveHeight / counter;
		}
		Debug.Log (averageAttractiveHeight);

	}

	void OnTriggerStay(Collider col) 
	{

		if (col.gameObject.tag == "Magnet" && !magnetsTouching.Contains (col.gameObject)) 
		{
			currentStage = col.gameObject.GetComponent<MagnetScript>().stage;
			magnetsTouching.Add(col.gameObject);
		}

	}

	void OnTriggerExit(Collider col) 
	{
		if (col.gameObject.tag == "Magnet" && magnetsTouching.Contains (col.gameObject)) 
		{
			magnetsTouching.Remove(col.gameObject);
		}
	}
*/
}
