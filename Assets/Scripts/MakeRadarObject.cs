using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MakeRadarObject : MonoBehaviour 
{
	public Image image;

	// Use this for initialization
	void Start () 
	{
		Radar.registerRadarObject (this.gameObject, image);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
