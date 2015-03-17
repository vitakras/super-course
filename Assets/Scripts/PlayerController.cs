using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, KinectGestures.GestureListenerInterface {

	public float speed = 5.0f;
	public bool kinectControl = false;
	public Text DebugInfo; // GUI Text to display the gesture messages.
	public GameStateManager gameState;
	public MarkerPath path;
	
	private float distanceTraveled;
	private CharacterMotor motor;

	private KinectManager manager;
	private float gesture_progress = -1;
	private KinectGestures.Gestures gesture = KinectGestures.Gestures.None;
	
	private uint num_steps;
	private uint num_jumps;
	
	private bool user_jumped;

	private float startTime; //time gesture started;
	private float gestureTime; //time gestrue took;

	private bool test;

	// Use this for initialization
	void Start () {
		this.gestureTime = 0.0f;
		this.distanceTraveled = 0.0f;
		this.ResetGestureProgress ();

		motor = GetComponent <CharacterMotor> ();

		motor.movement.maxForwardSpeed = 0.0f;


		if (this.manager == null) {
			this.manager = KinectManager.Instance;
		}
	}
	
	// Update is called once per frame
	void Update () {
		//motor.movement.maxForwardSpeed = this.speed;
		this.distanceTraveled = transform.localPosition.z;
		this.motor.inputJump = test;

		if (this.kinectControl) {
			Vector3 direction = (path.NextMarkerDirection(transform.position));
			//float speed = (this.gestureTime == 0.0f) ? 0 : this.speed / this.gestureTime;

			//Debug.Log("speed: " + speed);
			if(gesture == KinectGestures.Gestures.Walk) {
				//motor.movement.maxForwardSpeed = this.speed;
			} else {
				//motor.movement.maxForwardSpeed = 0f;
			}
			//if (path.NextMarker()) {
				//Rotate In the Direction
				Vector3 nextPositin = path.NextPosition();
			nextPositin.y = gameObject.transform.position.y;
				this.gameObject.transform.rotation = Quaternion.Slerp(this.gameObject.transform.rotation,
				                            Quaternion.LookRotation(nextPositin - gameObject.transform.position), 5*Time.deltaTime);
			//}

			// Walk In the direction of markers
			motor.inputMoveDirection = direction;
		} else {
			Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			
			if (directionVector != Vector3.zero) {
				float directionLength = directionVector.magnitude;
				directionVector = directionVector / directionLength;

				directionLength = Mathf.Min(1, directionLength);

				directionLength = directionLength * directionLength;

				directionVector = directionVector * directionLength;
			}
			// Apply the direction to the CharacterMotor
			motor.inputMoveDirection = transform.rotation * directionVector;
			if (Input.GetButton("Jump")) {
				Jump();
			}

			if (Input.GetKey("q")) {
				motor.movement.maxForwardSpeed = 10;
			}
		}
	}
	
	public void Move(Vector3 direction) {
		this.gameObject.transform.Translate(direction * this.speed * Time.deltaTime);
	}

	public void KinectMove(Vector3 direction) {
		float speed = (this.gestureTime == 0.0f) ? 0 : this.speed / this.gestureTime;

		this.gameObject.transform.Translate(direction * speed * Time.deltaTime);
	}
	
	public float GetDistanceTraveled() {
		return this.distanceTraveled;
	}

	public uint GetSteps() {
		return this.num_steps;
	}

	public uint GetJumps() {
		return this.num_jumps;
	}

	public bool UserJumped() {
		if (this.user_jumped) {
			this.user_jumped = false;
			return true;
		}
		
		return false;
	}

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Entered");
		Debug.Log (other.tag);

		if(other.tag.Equals("RoadBlock")) {
			//Debug.Log ("Entered");
			Jump();
		}
	}
	
	public void UserDetected(long userId, int userIndex) {

		// the gestures are allowed for the primary user only
		if (!this.manager || (userId != manager.GetPrimaryUserID ())) {
			return;
		}
		
		// detect these user specific gestures
		this.manager.DetectGesture (userId, KinectGestures.Gestures.Jump);
		this.manager.DetectGesture (userId, KinectGestures.Gestures.Walk);


		this.DebugInfo.text = "User visible";
		
	}
	
	public void UserLost(long userId, int userIndex)
	{
		if (!manager || (userId != manager.GetPrimaryUserID ())) {
			return;
		}

		if(GameStateManager.gameState == GameStateManager.State.PLAYING) {
			this.gameState.PauseGame();
		}
	}
	
	public void GestureInProgress(long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              float progress, KinectInterop.JointType joint, Vector3 screenPos)
	{
		// the gestures are allowed for the primary user only
		if (!this.manager || (userId != this.manager.GetPrimaryUserID ())) {
			return;
		}
		
		this.gesture = gesture;

		Debug.Log(""  + gesture + " " + test);

		if ((gesture == KinectGestures.Gestures.Walk) && // check when gesture walk started;
		    (progress == 0.2f) && (gesture_progress != progress)) {
			motor.movement.maxForwardSpeed = this.speed;
			this.DebugInfo.text = "" + gesture;
			this.startTime = Time.time;
		} else if ((gesture == KinectGestures.Gestures.Walk) && 
		    (progress == 0.5f) && (gesture_progress != progress)) {
			this.num_steps++;
		}  else if ((gesture == KinectGestures.Gestures.Jump) &&
		    (progress == 0.5f) && (gesture_progress != progress)) {
			motor.movement.maxForwardSpeed = 5f;
			this.DebugInfo.text = "" + gesture;
			Jump();
				this.test = true;
		} 
		
		this.gesture_progress = progress;
	}
	
	public bool GestureCompleted (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint, Vector3 screenPos)
	{	
		// the gestures are allowed for the primary user only
		if (!manager || (userId != manager.GetPrimaryUserID ())) {
			return false;
		}

		this.gestureTime = Time.time - this.startTime;
		this.DebugInfo.text = "" + gesture + " " + this.gestureTime;


		switch (gesture) {
		case KinectGestures.Gestures.Jump:
			this.num_jumps++;
			this.user_jumped = true;
			this.test = false;
			break;
		case KinectGestures.Gestures.Walk:
			this.num_steps++;
			break;
		}
		
		this.ResetGestureProgress ();
		
		return true;
	}
	
	public bool GestureCancelled (long userId, int userIndex, KinectGestures.Gestures gesture, 
	                              KinectInterop.JointType joint)
	{
		if (!manager || (userId != manager.GetPrimaryUserID ())) {
			return false;
		}

		this.gestureTime = 0;
		this.gesture = KinectGestures.Gestures.None;

		ResetGestureProgress ();
		return true;
	}
	
	/* Private Functions */
	
	private void ResetGestureProgress() {
		this.gesture_progress = -1;
	}

	private void Jump() {
		if (motor.IsGrounded ()) {
			motor.SetVelocity(motor.jumping.jumpDir * motor.CalculateJumpVerticalSpeed (motor.jumping.baseHeight + motor.jumping.extraHeight));
		}
	}
}
