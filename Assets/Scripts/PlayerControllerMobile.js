#pragma strict

public var speed : float = 3;
public var jumpPower : float = 6;

private var direction : Vector3 = Vector3.zero;
private var playerController : CharacterController;
//private var leftJoystick : Joystick;
//private var abutton : Joystick;


function Start () {
	playerController = GetComponent(CharacterController);
	leftJoystick = GameObject.Find("LeftTouchPad").GetComponent( Joystick );
	abutton = GameObject.Find("AButton").GetComponent( Joystick );
}

function Update () {
	
Debug.Log("Update");

	if (playerController.isGrounded) {
		var inputX : float = leftJoystick.position.x;
		var inputY : float = leftJoystick.position.y;
		var inputDirection : Vector3 = Vector3( inputX, 0, inputY );
		
		direction = Vector3.zero;
		
		if (inputDirection.magnitude > 0.1) {
			transform.LookAt( transform.position + inputDirection );
			direction += transform.forward * speed;
		}
		if (abutton.tapCount) {
			inputDirection.y += jumpPower;
		}
	}

	direction.y += Physics.gravity.y * Time.deltaTime;
	playerController.Move(direction * Time.deltaTime);
}