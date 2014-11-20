using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GUITexture))]
public class Joystick : MonoBehaviour 
{
	class Boundary 
	{
		Vector2 min = Vector2.zero;
		Vector2 max = Vector2.zero;

		public Vector2 Min
		{
			get
			{
				return min;
			}
			set
			{
				min = value;
			}
		}

		public Vector2 Max
		{
			get
			{
				return max;
			}
			set
			{
				max = value;
			}
		}
	}

	static private Joystick[] joysticks;
	static private bool enumeratedJoysticks = false;
	static private float tapTimeDelta = 0.3f;

	bool touchPad;
	Rect touchZone;
	Vector2 deadZone = Vector2.zero;
	bool normalize = false;
	Vector2 position;
	int tapCount;

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
		gui = GetComponent<GUITexture>();

		defaultRect = gui.pixelInset;

		defaultRect.x += transform.position.x * Screen.width;
		defaultRect.y += transform.position.y * Screen.height;

		if ( touchPad )
		{

			if ( gui.texture )
				touchZone = defaultRect;
		}
		else
		{				

			guiTouchOffset.x = defaultRect.width * 0.5f;
			guiTouchOffset.y = defaultRect.height * 0.5f;


			guiCenter.x = defaultRect.x + guiTouchOffset.x;
			guiCenter.y = defaultRect.y + guiTouchOffset.y;
			//
			float MinX = defaultRect.x - guiTouchOffset.x;
			float MinY = defaultRect.y - guiTouchOffset.y;
			float MaxX = defaultRect.x + guiTouchOffset.x;
			float MaxY = defaultRect.y + guiTouchOffset.y;

			guiBoundary.Min = new Vector2(MinX, MinY);
			guiBoundary.Max = new Vector2(MaxX, MaxY);


		}
	}

	void Disable()
	{
		gameObject.SetActive(false);
		enumeratedJoysticks = false;
	}

	void ResetJoystick()
	{

		gui.pixelInset = defaultRect;
		lastFingerId = -1;
		position = Vector2.zero;
		fingerDownPos = Vector2.zero;

		if (touchPad) 
		{
			Color colorT = guiTexture.color;
			colorT.a = 100f;
			guiTexture.color = colorT;
//			gui.color.a = 99f;
		}
	}

	bool IsFingerDown()
	{
		return (lastFingerId != -1);
	}

	void LatchedFinger(int fingerId )
	{

		if (lastFingerId == fingerId) {
			ResetJoystick ();
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		if ( !enumeratedJoysticks )
		{

			joysticks = FindObjectsOfType( typeof(Joystick) ) as Joystick[];
			enumeratedJoysticks = true;
		}


		int count = Input.touchCount;


		if (tapTimeWindow > 0) {
			tapTimeWindow -= Time.deltaTime;
		} else {
			tapCount = 0;
		}
	}
}
