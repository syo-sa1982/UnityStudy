using UnityEngine;
using System.Collections;

public class PlayerCrystal : MonoBehaviour 
{
	public int crystal;

	public int Crystal { get; private set; }

	public void AddCrystal ()
	{
		crystal++;
	}

}
