using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed = 3;
	public float jumpPower = 6;

	private Vector3 direction = Vector3.zero;
	private CharacterController playerController;
	private Animator animetor;

	// Use this for initialization
	void Start () 
	{
		playerController = GetComponent<CharacterController>();
		animetor = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (playerController.isGrounded) {
			float inputX = Input.GetAxis ("Horizontal");
			float inputY = Input.GetAxis ("Vertical");

			Vector3 inputDirection = new Vector3 (inputX, 0, inputY);
			direction = Vector3.zero;

			if (inputDirection.magnitude > 0.1) {
				transform.LookAt (transform.position + inputDirection);
				direction += transform.forward * speed;
				animetor.SetFloat ("Speed", direction.magnitude);
			} else {
				animetor.SetFloat ("Speed", 0);
			}

			if (Input.GetButton("Jump")) {
				direction.y += jumpPower;
			}
		}

		direction.y += Physics.gravity.y * Time.deltaTime;
		playerController.Move (direction * Time.deltaTime);
	}
}
