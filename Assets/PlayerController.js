#pragma strict
public var speed : float = 3;
public var jumpPower : float = 6;

private var direction : Vector3 = Vector3.zero;
private var playerController : CharacterController;

function Start () {
	playerController = GetComponent(CharacterController);
	
		Debug.Log(playerController);
	
}

function Update () {
	if (playerController.isGrounded) {
		var inputX : float = Input.GetAxis( "Horizontal" );
		var inputY : float = Input.GetAxis( "Vertical" );
		
		var inputDirection : Vector3 = Vector3( inputX, 0, inputY );
		direction = Vector3.zero;
		
		if (inputDirection.magnitude > 0.1) {
			transform.LookAt( transform.position + inputDirection );
			direction += transform.forward * speed;
		}
		
		if (Input.GetButton("Jump")) {
			Debug.Log("Jump!!");
			Debug.Log(direction.y);
			
			direction.y += jumpPower;
			
			Debug.Log(direction.y);
		}
		
	}

	Debug.Log(direction);
	direction.y = Physics.gravity.y * Time.deltaTime;
	Debug.Log(direction);
	playerController.Move(direction * Time.deltaTime);
}