using UnityEngine;
using System.Collections;

public class SerialUISummoner : MonoBehaviour 
{
	PowerDrain console;
	public bool showing = false;
	public float delay = 0.3f;
	protected Animator[] children;

	// Use this for initialization
	void Start () 
	{
		console = GetComponentInParent<PowerDrain> ();
		children = GetComponentsInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (console.stateActive())
		{
			if (console.besideDevice) 
			{
				if (showing) return;
				StartCoroutine (activateInTurn ());
			}
			else 
			{
				if (!showing) return;
				StartCoroutine (deactivateInTurn ());
			}
		}
	}

	public IEnumerator activateInTurn ()
	{
		showing = true;
		yield return new WaitForSeconds (delay);
		for (int a = 0; a < children.Length; a++) 
		{
			children [a].SetBool ("Shown", true);
			yield return new WaitForSeconds (delay);
		}
	}

	public IEnumerator deactivateInTurn ()
	{
		showing = false;
		yield return new WaitForSeconds (delay);
		for (int a = 0; a < children.Length; a++) 
		{
			children [a].SetBool ("Shown", false);
			yield return new WaitForSeconds (delay);
		}
	}

}
