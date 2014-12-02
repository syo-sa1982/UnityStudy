using UnityEngine;
using System.Collections;

public class SimpleRotation : MonoBehaviour 
{
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 v = transform.eulerAngles;
		v.y = Mathf.Repeat( Time.time, 1 ) * 360f;
		transform.eulerAngles = v;
	}
}
