using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RadarObject
{
	public Image icon { get; set; }
	public GameObject owner { get; set; }
}

public class Radar : MonoBehaviour 
{

	public Transform playerPos;
	float mapScale = 2.0f;
	Vector3 radarPos;

	public static List<RadarObject> radObjects = new List<RadarObject> ();

	public static void registerRadarObject (GameObject o, Image i)
	{
		Image image = Instantiate (i);
		radObjects.Add(new RadarObject(){owner = o, icon = image});
	}

	void drawRadarDots ()
	{
		for (int x = 0; x < radObjects.Count; x++)
		{
			RadarObject ro = radObjects[x];
			radarPos = (ro.owner.transform.position - playerPos.position);
			float distToObject = Vector3.Distance (playerPos.position, ro.owner.transform.position) * mapScale;
			float deltay = Mathf.Atan2 (radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - playerPos.eulerAngles.y;

			radarPos.x = distToObject * Mathf.Cos (deltay * Mathf.Deg2Rad) * -1;
			radarPos.z = distToObject * Mathf.Sin (deltay * Mathf.Deg2Rad);
			ro.icon.transform.SetParent (this.transform);
			ro.icon.transform.position = new Vector3 (radarPos.x, radarPos.z, 0) + this.transform.position;

/*			if (ro.owner.tag != "Room")
			{
			}
			else
			{
				radarPos.x = distToObject;
				radarPos.z = distToObject;
			}
*/
		}
	}

	void Update ()
	{
		drawRadarDots ();
	}
}
