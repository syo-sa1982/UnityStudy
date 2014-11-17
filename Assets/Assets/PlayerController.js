#pragma strict
pubic var speed : float = 3;
pubic var jumpPower : float = 3;

private var direction : Vector3 = Vector3.zero;
private var playerController : CharacterController;

function Start () {
	playerController = GetComponent(CharacterController);
}

function Update () {
	
	if (playerController.isGrounded) {
		var inputX : float = Input.GetAxis( "Horizontal" );
		var inputY : float = Input.GetAxis( "Vertical" );
	}
}