#pragma strict

@script RequireComponent( GUITexture )


class Boundary 
{
	var min : Vector2 = Vector2.zero;
	var max : Vector2 = Vector2.zero;
}

static private var joysticks : Joystick[];					
static private var enumeratedJoysticks : boolean = false;
static private var tapTimeDelta : float = 0.3;				

var touchPad : boolean; 									
var touchZone : Rect;
var deadZone : Vector2 = Vector2.zero;						
var normalize : boolean = false; 							
var position : Vector2; 									
var tapCount : int;											

private var lastFingerId = -1;								
private var tapTimeWindow : float;							
private var fingerDownPos : Vector2;
private var fingerDownTime : float;
private var firstDeltaTime : float = 0.5;

private var gui : GUITexture;								
private var defaultRect : Rect;								
private var guiBoundary : Boundary = Boundary();			
private var guiTouchOffset : Vector2;						
private var guiCenter : Vector2;							

function Start()
{
		
	gui = GetComponent( GUITexture );
	

	defaultRect = gui.pixelInset;	
    
    defaultRect.x += transform.position.x * Screen.width;
    defaultRect.y += transform.position.y * Screen.height;
    
    transform.position.x = 0.0;
    transform.position.y = 0.0;
        
	if ( touchPad )
	{
	
		if ( gui.texture )
			touchZone = defaultRect;
	}
	else
	{				
	
		guiTouchOffset.x = defaultRect.width * 0.5;
		guiTouchOffset.y = defaultRect.height * 0.5;
		
		
		guiCenter.x = defaultRect.x + guiTouchOffset.x;
		guiCenter.y = defaultRect.y + guiTouchOffset.y;
		
		
		guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
		guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
		guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
		guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
	}
}

function Disable()
{
	gameObject.SetActive(false);
	enumeratedJoysticks = false;
}

function ResetJoystick()
{
	
	gui.pixelInset = defaultRect;
	lastFingerId = -1;
	position = Vector2.zero;
	fingerDownPos = Vector2.zero;
	
	if ( touchPad )
		gui.color.a = 100;	
}

function IsFingerDown() : boolean
{
	return (lastFingerId != -1);
}
	
function LatchedFinger( fingerId : int )
{

	if ( lastFingerId == fingerId )
		ResetJoystick();
}

function Update()
{	
	if ( !enumeratedJoysticks )
	{
		
		joysticks = FindObjectsOfType( Joystick ) as Joystick[];
		enumeratedJoysticks = true;
	}	
		
	var count = Input.touchCount;
	
	
	if ( tapTimeWindow > 0 )
		tapTimeWindow -= Time.deltaTime;
	else
		tapCount = 0;
	
	if ( count == 0 )
		ResetJoystick();
	else
	{
		for(var i : int = 0;i < count; i++)
		{
			var touch : Touch = Input.GetTouch(i);			
			var guiTouchPos : Vector2 = touch.position - guiTouchOffset;
	
			var shouldLatchFinger = false;
			if ( touchPad )
			{				
				if ( touchZone.Contains( touch.position ) )
					shouldLatchFinger = true;
			}
			else if ( gui.HitTest( touch.position ) )
			{
				shouldLatchFinger = true;
			}		
	
		
			if ( shouldLatchFinger && ( lastFingerId == -1 || lastFingerId != touch.fingerId ) )
			{
				
				if ( touchPad )
				{
					gui.color.a = 100;
					
					lastFingerId = touch.fingerId;
					fingerDownPos = touch.position;
					fingerDownTime = Time.time;
				}
				
				lastFingerId = touch.fingerId;
				
			
				if ( tapTimeWindow > 0 )
					tapCount++;
				else
				{
					tapCount = 1;
					tapTimeWindow = tapTimeDelta;
				}
											
			
				for ( var j : Joystick in joysticks )
				{
					if ( j != this )
						j.LatchedFinger( touch.fingerId );
				}						
			}				
	
			if ( lastFingerId == touch.fingerId )
			{	
			
				if ( touch.tapCount > tapCount )
					tapCount = touch.tapCount;
				
				if ( touchPad )
				{	
					
					position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
					position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
				}
				else
				{					
					
					gui.pixelInset.x =  Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
					gui.pixelInset.y =  Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
				}
				
				if ( touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled )
					ResetJoystick();					
			}			
		}
	}
	
	if ( !touchPad )
	{
	
		position.x = ( gui.pixelInset.x + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
		position.y = ( gui.pixelInset.y + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
	}
	

	var absoluteX = Mathf.Abs( position.x );
	var absoluteY = Mathf.Abs( position.y );
	
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