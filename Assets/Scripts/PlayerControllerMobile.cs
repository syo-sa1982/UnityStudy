using UnityEngine;
using System.Collections;

public class PlayerControllerMobile : MonoBehaviour 
{
	public float speed = 3;
	public float jumpPower = 6;

	private Vector3 direction = Vector3.zero;
	private CharacterController playerController;
	private Joystick leftJoystick;
	private Joystick abutton;

	// Use this for initialization
	void Start () 
	{
		playerController = GetComponent<CharacterController>();
//		leftJoystick = GameObject.Find("LeftTouchPad").GetComponent( Joystick );
//		abutton = GameObject.Find("AButton").GetComponent( Joystick );
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
