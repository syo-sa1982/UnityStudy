using UnityEngine;
using System.Collections;

public class Joystick : MonoBehaviour 
{
	class Boundary 
	{
		Vector2 min = Vector2.zero;
		Vector2 max = Vector2.zero;
	}

	private int lastFingerId = -1;
	private float tapTimeWindow;
	private Vector2 fingerDownPos;
	private float fingerDownTime;
	private float firstDeltaTime = 0.5f;


	private GUITexture gui;
	private Rect defaultRect;
	private Boundary guiBoundary = new Boundary();
	private Vector2 guiTouchOffset;
	private Vector2 guiCenter;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
