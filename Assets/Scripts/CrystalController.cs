using UnityEngine;
using System.Collections;

public class CrystalController : MonoBehaviour 
{
	public GameObject getEffect;

	void OnTriggerEnter(Collider col) 
	{
		if(col.tag == "Player") {
			col.gameObject.SendMessage ("AddCrystal");

			Destroy (gameObject);

			GameObject obj = GameObject.Instantiate (getEffect) as GameObject;
			obj.transform.position = transform.position;

			Destroy (obj, 1);
		}
	}
}
