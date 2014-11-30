using UnityEngine;
using System.Collections;

public class CrystalController : MonoBehaviour 
{
	void OnTriggerEnter(Collider col) 
	{
		if(col.tag == "Player") {
			col.gameObject.SendMessage ("AddCrystal");

			Destroy (gameObject);
		}
	}
}
