using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, KinectGestures.GestureListenerInterface {

	public float speed = 5.0f;
	public Text DebugInfo; // GUI Text to display the gesture messages.
	public GameStateManager gameState;
	
	private float distanceTraveled;
	private CharacterMotor motor;

	private KinectManager manager;
	private float gesture_progress = -1;
	
	private uint num_steps;
	private uint num_jumps;
	
	private bool user_jumped;

	private float startTime; //time gesture started;
	private float gestureTime; //time gestrue took;

	private bool test;

	// Use this for initialization
	void Start () {
		this.DebugInfo.text = "worsk";
		this.gestureTime = 0.0f;
		this.distanceTraveled = 0.0f;
		this.ResetGestureProgress ();

		motor = GetComponent <CharacterMotor> ();

		if (this.manager == null) {
			this.manager = KinectManager.Instance;
		}
	}
	
	// Update is called once per frame
	void Update () {
		this.distanceTraveled = transform.localPosition.z;
		this.motor.inputJump = test;

		Debug.Log("Input jump" + motor.IsJumping ());
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

	public bool UserJumped() {
		if (this.user_jumped) {
			this.user_jumped = false;
			return true;
		}
		
		return false;
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

		Debug.Log(""  + gesture + " " + test);

		if ((gesture == KinectGestures.Gestures.Walk) && // check when gesture walk started;
		    (progress == 0.2f) && (gesture_progress != progress)) {
			this.startTime = Time.time;
		} else if ((gesture == KinectGestures.Gestures.Walk) && 
		    (progress == 0.5f) && (gesture_progress != progress)) {
			this.num_steps++;
		}  else if ((gesture == KinectGestures.Gestures.Jump) &&
		    (progress == 0.5f) && (gesture_progress != progress)) {
			if (motor.IsGrounded ()) {
				motor.SetVelocity(motor.jumping.jumpDir * motor.CalculateJumpVerticalSpeed (motor.jumping.baseHeight + motor.jumping.extraHeight));
			}
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
		this.DebugInfo.text = "" + gesture;


		switch (gesture) {
		case KinectGestures.Gestures.Jump:
			this.num_jumps++;
			this.user_jumped = true;
			this.test = false;
			Debug.Log("Jumped" + num_jumps);
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

		//this.gestureTime = 0;

		ResetGestureProgress ();
		return true;
	}
	
	/* Private Functions */
	
	private void ResetGestureProgress() {
		this.gesture_progress = -1;
	}
}
