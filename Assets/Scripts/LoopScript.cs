using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoopScript : MonoBehaviour 
{
	public string loop;
	private string[] loopType = new string[3]; 
	GameObject me;
	GameManager gameMngr;

	void Start () 
	{
		loopType [0] = "Primary";
		loopType [1] = "Secondary";
		loopType [2] = "Tertiary";
		me = this.gameObject;
		gameMngr = GameManager.instance;
		loopAssign ();
		tierAssign ();
	}
	
	void Update () 
	{
		
	}

	public void loopAssign ()
	{
		if (me.tag == "Reactor" || me.tag == "GoalCon")
		{
			loop = loopType [0];
		}
		else if (me.tag == "Door" || me.tag == "Generator")
		{
			loop = loopType [1];
		}
		else if (me.tag == "HealthCon" || me.tag == "ServerCon")
		{
			loop = loopType [2];
		}
	}

	public void tierAssign ()
	{
		if (loop == loopType [0])
		{
			gameMngr.tier1.Add (this.gameObject);
		}
		else if (loop == loopType [1])
		{
			gameMngr.tier2.Add (this.gameObject);
		}
		else if (loop == loopType [2])
		{
			gameMngr.tier3.Add (this.gameObject);
		}
	}
}
