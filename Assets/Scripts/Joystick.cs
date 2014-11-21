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

	public bool touchPad;
	public Rect touchZone;
	public Vector2 deadZone = Vector2.zero;
	public bool normalize = false;
	public Vector2 position;
	public int tapCount;

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

		if (count == 0) {
			ResetJoystick ();
		} 
		else
		{
			for (int i = 0; i < count; i++) {
				Touch touch = Input.GetTouch(i);
				Vector2 guiTouchPos = touch.position - guiTouchOffset;

				bool shouldLatchFinger = false;
				if ( touchPad )
				{
					if (touchZone.Contains (touch.position)) {
						shouldLatchFinger = true;
					}
				}
				else if (gui.HitTest(touch.position))
				{
					shouldLatchFinger = true;
				}

				if (shouldLatchFinger && (lastFingerId == -1 || lastFingerId != touch.fingerId)) {
					if ( touchPad )
					{

						Color colorT = guiTexture.color;
						colorT.a = 100f;
						guiTexture.color = colorT;

						lastFingerId = touch.fingerId;
						fingerDownPos = touch.position;
						fingerDownTime = Time.time;
					}

					lastFingerId = touch.fingerId;

					if (tapTimeWindow > 0) 
					{
						tapCount++;
					}
					else
					{
						tapCount = 1;
						tapTimeWindow = tapTimeDelta;
					}

					foreach ( Joystick j in joysticks )
					{
						if ( j != this ){
							j.LatchedFinger( touch.fingerId );
						}
					}
				}

				if ( lastFingerId == touch.fingerId )
				{	

					if (touch.tapCount > tapCount) {
						tapCount = touch.tapCount;
					}

					if ( touchPad )
					{	

						position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
						position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
					}
					else
					{
						gui.pixelInset = new Rect (
							Mathf.Clamp( guiTouchPos.x, guiBoundary.Min.x, guiBoundary.Max.x ),
							Mathf.Clamp( guiTouchPos.y, guiBoundary.Min.y, guiBoundary.Max.y ),
							gui.pixelInset.width,
							gui.pixelInset.height
						);

//						gui.pixelInset.x =  Mathf.Clamp( guiTouchPos.x, guiBoundary.Min.x, guiBoundary.Max.x );
//						gui.pixelInset.y =  Mathf.Clamp( guiTouchPos.y, guiBoundary.Min.y, guiBoundary.Max.y );		

					}
//
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
						ResetJoystick();
					}
				}
			}
		}

		if ( !touchPad )
		{
			position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
			position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
		}


		float absoluteX = Mathf.Abs( position.x );
		float absoluteY = Mathf.Abs( position.y );

		if ( absoluteX < deadZone.x )
		{

			position.x = 0;
		}
		else if ( normalize )
		{

			position.x = Mathf.Sign( position.x ) * ( absoluteX - deadZone.x ) / ( 1 - deadZone.x );
		}

		if ( absoluteY < deadZone.y )
		{

			position.y = 0;
		}
		else if ( normalize )
		{

			position.y = Mathf.Sign( position.y ) * ( absoluteY - deadZone.y ) / ( 1 - deadZone.y );
		}


	}
}
