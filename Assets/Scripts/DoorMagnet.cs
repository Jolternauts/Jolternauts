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
}
