using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompassScript : MonoBehaviour 
{
	public float range;

	public GameObject currentHitTarget;
	public GameObject arrow;
	public List<GameObject> arrows = new List<GameObject>();

	RoomScript targetRoom;
	RoomScript room;


	void Start () 
	{
		room = this.gameObject.GetComponentInParent<RoomScript> ();
		this.GetComponent<BoxCollider> ().isTrigger = true;
	}
	
	void Update () 
	{
		/// Cast a ray forward from the compass' arrow, if it hits, what it hits is current hit target.
		RaycastHit hit;
		Vector3 forward = arrow.transform.TransformDirection(Vector3.forward) * range; 
		Debug.DrawRay (arrow.transform.position, forward, Color.green);  

		if (Physics.Raycast (arrow.transform.position, arrow.transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
	}

	// Get the name of what the ray hits.
	void sayMyName (RaycastHit hit)
	{
		Debug.Log (hit.transform.name);
	}

	// Wait function.
	public IEnumerator Wait (float seconds)
	{
		yield return new WaitForSeconds (seconds);

		if (targetRoom && currentHitTarget != room.here)
		{
			room.callPowerTransfer ();
		}
		Debug.Log (currentHitTarget.name);
	}
		
	/// Turns the dial left.
	/// Undo power transfer for original target.
	/// Renew the raycast.
	/// Transfer to new target.
	/// Debug what it hits.
	public void turnDialLeft ()
	{
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
		if (room.isPowered) 
		{
			if (targetRoom)
			{				
				if (currentHitTarget != room.here && targetRoom.chainPos > room.chainPos && targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
				}
			}
			Debug.Log (currentHitTarget.name);
		}
			
		transform.Rotate (0, -90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}

		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}

	// Same as above but turn right.
	public void turnDialRight ()
	{
		targetRoom = currentHitTarget.GetComponent<RoomScript> ();
		if (room.isPowered) 
		{
			if (targetRoom)
			{
				if (currentHitTarget != room.here && targetRoom.chainPos > room.chainPos && targetRoom.isPowered)
				{
					room.redirectPowerTransfer (currentHitTarget);  
				}
			}
			Debug.Log (currentHitTarget.name);
		}

		transform.Rotate (0, 90, 0, Space.Self);
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit)) 
		{
			currentHitTarget = hit.collider.gameObject;
		}

		if (room.isPowered) 
		{
			StartCoroutine (Wait (0.5f));
		}
	}


}
